using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerBattle : MonoBehaviour
{
    static PlayerBattle _instance;
    public static PlayerBattle Instance => _instance;

    readonly Dictionary<int, int> _skillCountInATurn = new Dictionary<int, int>();
    public int actLimit = 8;
    bool _isDodge;
    bool _isDefend;
    bool _isStrike;
    long _attackActs;
    float _enemyDefence;
    int _lastSkillId = -1;
    Animator _animator;
    Text _actsText;
    Text _skillText;
    Slider _healthSlider;
    Enemy _enemy;
    
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

    public bool IsTurnReset { get; set;} = true;
    public bool IsPlayerTurn { get; set;} = true;
    public PlayerProperty Property { get; } = new PlayerProperty();
    
    static readonly int _Dodge = Animator.StringToHash("Dodge");
    static readonly int _Defend = Animator.StringToHash("Defend");
    static readonly int _Strike = Animator.StringToHash("Strike");

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
        Pause();

        _animator = GetComponent<Animator>();
        _actsText = GameObject.Find("AttackActs").GetComponent<Text>();
        _skillText = GameObject.Find("SkillText").GetComponent<Text>();
        _healthSlider = GameObject.Find("Health").GetComponent<Slider>();
    }

    void Start()
    {
        SceneLinkedSMB<PlayerBattle>.Initialise(_animator, this);
        _enemy = FindObjectOfType<Enemy>();
    }

    void FixedUpdate()
    {
        Act();
    }

    void Act()
    {
        if (!IsPlayerTurn) return;

        var buttons = PlayerInput.Instance.Buttons;
        if (buttons["Dodge"].Up)
        {
            _animator.SetTrigger(_Dodge);
        }
        else if (buttons["Defend"].Up)
        {
            _animator.SetTrigger(_Defend);
        }

        foreach (var d in _directions)
        {
            _animator.ResetTrigger(d);
        }

        if (_attackActs < Mathf.Pow(10, actLimit-1))
        {
            foreach (var d in _directions.Where(d => buttons[d].Down))
            {
                _animator.SetTrigger(d);
                return;
            }
        }

        if (buttons["Confirm"].Up)
        {
            if (_attackActs != 0)
                _animator.SetTrigger(_Strike);
        }
    }

    void ResetTurn()
    {
        IsTurnReset = true;
        _enemyDefence = 0;
        _attackActs = 0;
        _actsText.text = "";
    }

    void ExecuteDamage(float dmg)
    {
        Property.health -= dmg;
        Debug.Log("Enemy Damage:" + dmg);
        _healthSlider.value = Property.health / Property.maxHealth;
    }

    float CalculateDamage(float str, float defence, long attackActs, float ratio = 1f, bool isCombo = false,
        int skillCount = 0)
    {
        var actTimes = attackActs > 9 ? Mathf.CeilToInt(Mathf.Log10(attackActs)) : 1;
        var enemyRatio = (100 * actTimes - defence) / (100 * actTimes);
        var comboRatio = isCombo ? 1.5f : 1f;
        var skillRatio = ((ratio - 1) * (1 - skillCount * 0.2f) + 1);
        return str * skillRatio * comboRatio * enemyRatio;
    }

    public void Strike()
    {
        if (SqLiteController.Instance.ExistSkillActs(_attackActs))
        {
            var skill = SqLiteController.Instance.GetLearnedSkillByActs(_attackActs);
            _skillText.text = skill.name;
            if (!_skillCountInATurn.ContainsKey(skill.id)) _skillCountInATurn.Add(skill.id, 0);
            _enemy.GetHurt(CalculateDamage(Property.strength, _enemyDefence, _attackActs,
                skill.damageRatio, _lastSkillId == skill.previousId,
                _skillCountInATurn[skill.id] < 5 ? _skillCountInATurn[skill.id] : 5));
            _lastSkillId = skill.id;
            _skillCountInATurn[skill.id] += 1;
        }
        else
        {
            _skillText.text = "无招";
            _enemy.GetHurt(CalculateDamage(Property.strength, _enemyDefence, _attackActs));
            _lastSkillId = 0;
        }

        ResetTurn();
    }

    public void Dodge()
    {
        _isDodge = true;
        _skillText.text = "闪避";
        ResetTurn();
        IsPlayerTurn = false;
    }

    public void Defend()
    {
        _isDefend = true;
        _skillText.text = "防御";
        ResetTurn();
        IsPlayerTurn = false;
    }

    public void AddAct(int number, string actName)
    {
        _attackActs = _attackActs * 10 + number;
        _actsText.text += actName + " ";
        _enemyDefence += _enemy.enemyDefence[number - 1];
    }

    public void HurtBy(EnemySkill enemySkill)
    {
        if (_isDodge)
        {
            var rate = Property.speed / (Property.speed + enemySkill.accuracy);
            var random = Random.Range(0f, 1f);
            if (random > rate)
            {
                ExecuteDamage(enemySkill.damage);
                Debug.Log("闪避失败");
            }
            else
            {
                Debug.Log("闪避成功");
            }

            _isDodge = false;
        }
        else if (_isDefend)
        {
            ExecuteDamage(enemySkill.damage * (Property.defence / (Property.defence + enemySkill.penetration)));
            _isDefend = false;
        }
        else
            ExecuteDamage(enemySkill.damage);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PlayerInput.Instance.ReleaseControl();
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        PlayerInput.Instance.GainControl();
    }
}

public class PlayerProperty
{
    public float maxHealth;
    public float health;
    public float strength;
    public float defence;
    public float speed;
    public PlayerProperty(float maxH= 100f, float str = 100f, float def = 10f, float spd = 10f)
    {
        maxHealth = maxH;
        health = maxH;
        strength = str;
        defence = def;
        speed = spd;
    }
}

public class Skill
{
    public int id;
    public string name;
    public long directionalActs;
    public long positionalActs;
    public float damageRatio;
    public int previousId;
    public int count;

    readonly string[] _position = {"LD", "D", "RD", "L", "C", "R", "LU", "U", "RU"};

    string PosActs(long posActs)
    {
        var posStr = "";
        var pos = new Stack<long>();
        while (posActs > 0)
        {
            pos.Push(posActs % 10);
            posActs /= 10;
        }

        while (pos.Count > 0)
        {
            posStr += _position[pos.Pop() - 1] + " ";
        }

        return posStr;
    }

    public override string ToString()
    {
        return previousId <= 0 ? 
            $"剑技：{name}\nid：{id}\n轨迹：{PosActs(positionalActs)}\n\n" : 
            $"剑技：{name}\nid：{id}\n轨迹：{PosActs(positionalActs)}\n前一连招id：{previousId}\n\n";
    }
}