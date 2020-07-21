#define Sqlite
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattle : MonoBehaviour
{
    static PlayerBattle _instance;
    public static PlayerBattle Instance => _instance;
    
    public int actLimit = 7;
    public float strength = 90;
    public float[] enemyDefence =
    {
        80, 80, 80, 60, 20, 60, 20, 10, 20
    };

    public float maxHealth = 100f;
    float _health;
    

    readonly Dictionary<int,int> _skillCountInATurn = new Dictionary<int, int>();

    public bool isReset = true;
    public bool isPlayerTurn = true;
    int _attackActs;
    Animator _animator;
    Text _actsText;
    Text _skillText;
    Slider _healthSlider;
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
        if (_instance == null)
        {
            _instance = this;
        }
        _animator = GetComponent<Animator>();
        _actsText = GameObject.Find("AttackActs").GetComponent<Text>();
        _skillText = GameObject.Find("SkillText").GetComponent<Text>();
        _healthSlider = GameObject.Find("Health").GetComponent<Slider>();
    }

    void Start()
    {
        SceneLinkedSMB<PlayerBattle>.Initialise(_animator, this);
        _health = maxHealth;
    }

    void FixedUpdate()
    {
        Act();
    }

    void Act()
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
            isPlayerTurn = false;
            return;
        }
    }

    public void AddAct(int number, string actName)
    {
        _attackActs = _attackActs * 10 + number;
        _actsText.text += actName + " ";
        _enemyDefence += enemyDefence[number - 1];
    }

    void Strike()
    {
        _animator.Play("Center");
        _skillText.text = "无招";
        
#if Json
        foreach (var skill in SkillsJson.Instance.learnedSkills.Where(skill => _attackActs == skill.positionalActs))
        {
#elif Sqlite
        if (SqLiteController.Instance.ExistSkillActs(_attackActs))
        {
            var skill = SqLiteController.Instance.GetLearnedSkillByActs(_attackActs);
#endif
            _skillText.text = skill.name;
            if(!_skillCountInATurn.ContainsKey(skill.id)) _skillCountInATurn.Add(skill.id, 0);
            Debug.Log("Damage:" + 
                      CalculateDamage(strength, _enemyDefence,  _attackActs, 
                          skill.damageRatio, _lastSkillId == skill.previousId, 
                          _skillCountInATurn[skill.id] < 5 ? _skillCountInATurn[skill.id] : 5));
            _lastSkillId = skill.id;
            _skillCountInATurn[skill.id] += 1;
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

    public void HurtBy(EnemySkill enemySkill)
    {
        _health -= enemySkill.damage;
        _healthSlider.value = _health / maxHealth;
    }
}
