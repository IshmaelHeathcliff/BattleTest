using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattle : MonoBehaviour
{
    [HideInInspector]public List<Skill> learnedSkills = new List<Skill>();
    public int actLimit = 7;
    public float strength = 90;
    public float[] enemyDefence =
    {
        80, 80, 80, 60, 20, 60, 20, 10, 20
    };

    public bool isReset = true;
    
    int _attackActs;
    Animator _animator;
    Text _actsText;
    Text _skillText;
    float _enemyDefence;
    int _lastSkillId = -1;
   

    readonly List<string> _directions = new List<string>
    {
        "LeftDown",
        "Down",
        "RightDown",
        "Left",
        "Center",
        "Right",
        "LeftUp",
        "Up",
        "RightUp",
    };

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _actsText = GameObject.Find("AttackActs").GetComponent<Text>();
        _skillText = GameObject.Find("SkillText").GetComponent<Text>();
    }

    void Start()
    {
        SceneLinkedSMB<PlayerBattle>.Initialise(_animator, this);
        foreach (var skillInfo in SkillsInfo.Instance.skillsInfo)
        {
            learnedSkills.Add(new Skill(skillInfo));
        }
    }

    void FixedUpdate()
    {
        Act();
    }

    public void Act()
    {
        var buttons = PlayerInput.Instance.Buttons;
        if (buttons["Dodge"].Up)
        {
            return;
        }

        if (buttons["Defend"].Up)
        {
            return;
        }

        foreach (var d in _directions)
        {
            _animator.ResetTrigger(d);
        }
        

        if (_attackActs < Mathf.Pow(10, actLimit))
        {
            foreach (var d in _directions.Where(d => buttons[d].Down))
            {
                _animator.SetTrigger(d);
                return;
            }
        }

        if (buttons["Confirm"].Up)
        {
            if(_attackActs != 0)
                Strike();
            return;
        }
    }

    public void AddAct(int number, string actName)
    {
        _attackActs = _attackActs * 10 + number;
        _actsText.text += actName + " ";
        _enemyDefence += enemyDefence[number - 1];
    }

    public void Strike()
    {
        _animator.Play("Center");
        _skillText.text = "无招";

        foreach (var skill in learnedSkills.Where(skill => _attackActs == skill.Acts))
        {
            _skillText.text = skill.Name;
            Debug.Log("Damage:" + 
                      CalculateDamage(strength, _enemyDefence,  _attackActs, 
                          skill.Ratio, _lastSkillId == skill.PreviousID, 
                          skill.CountInATurn < 5 ? skill.CountInATurn : 5));
            _lastSkillId = skill.ID;
            skill.CountInATurn += 1;
            isReset = true;
            return;
        }
        
        Debug.Log("Damage:" + CalculateDamage(strength, _enemyDefence,  _attackActs));
        _lastSkillId = 0;
        isReset = true;

    }

    public void Reset()
    {
        _enemyDefence = 0;
        _attackActs = 0;
        _actsText.text = "";
    }
    
    float CalculateDamage(float str, float defence, int attackActs, float ratio=1f, bool isCombo=false, int skillCount=0)
    {
        var actTimes = attackActs > 9 ? Mathf.CeilToInt(Mathf.Log10(attackActs)) : 1;
        var enemyRatio = (100 * actTimes - defence) / (100 * actTimes);
        var comboRatio = isCombo ? 1.5f : 1f;
        var skillRatio = ((ratio - 1) * (1 - skillCount * 0.2f) + 1); 
        return  str * skillRatio * comboRatio * enemyRatio;
    }
}
