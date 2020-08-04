using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int id;
    public string enemyName;
    public Queue<EnemySkill> enemySkills = new Queue<EnemySkill>();
    public float[] enemyDefence = 
    {
        80, 80, 80, 60, 20, 60, 20, 10, 20
    };
    public float maxHealth = 1000f;
    float _health;

    public float Health => _health;

    Text _skillText;
    Slider _healthSlider;
    Animator _animator;
    Image _image;
    static readonly int _Hurt = Animator.StringToHash("Hurt");

    void Awake()
    {
        _skillText = GetComponentInChildren<Text>();
        _healthSlider = GetComponentInChildren<Slider>();
        _animator = GetComponentInChildren<Animator>();
        _image = GetComponentInChildren<Image>();
    }

    void Start()
    {
        SceneLinkedSMB<Enemy>.Initialise(_animator,this);
        var skillList = SqLiteController.Instance.GetEnemySkills(id);
        foreach (var enemySkill in skillList)
        {
            enemySkills.Enqueue(enemySkill);
        }

        Indicate();
        _health = maxHealth;
    }

    public void EndTurn()
    {
        PlayerBattle.Instance.IsPlayerTurn = true;
    }

    public void StartTurn()
    {
        PlayerBattle.Instance.IsPlayerTurn = false;
    }

    public void GetHurt(float damage)
    {
        _health -= damage;
        _healthSlider.value = _health / maxHealth;
        _animator.SetTrigger(_Hurt);
    }

    public void Attack()
    {
        var nextSkill = enemySkills.Dequeue();
        PlayerBattle.Instance.HurtBy(nextSkill);
        enemySkills.Enqueue(nextSkill);
    }

    public void Indicate()
    {
        var nextSkill = enemySkills.Peek();
        _skillText.text = nextSkill.skillName;
    }
}

public class EnemySkill: IComparable<EnemySkill>
{
    public float damage;
    public float accuracy;
    public float penetration;
    public string skillName;
    public int sequence;

    public int CompareTo(EnemySkill other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return sequence.CompareTo(other.sequence);
    }
}
