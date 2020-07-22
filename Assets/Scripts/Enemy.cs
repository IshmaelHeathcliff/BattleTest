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
    public float maxHealth = 1000f;
    float _health;

    Text _skillText;
    Slider _healthSlider;
    Animator _animator;
    static readonly int _Attack = Animator.StringToHash("Attack");
    static readonly int _Hurt = Animator.StringToHash("Hurt");

    void Awake()
    {
        _skillText = GetComponentInChildren<Text>();
        _healthSlider = GetComponentInChildren<Slider>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        SceneLinkedSMB<Enemy>.Initialise(_animator,this);
        var skillList = SqLiteController.Instance.GetEnemySkills(id);
        foreach (var enemySkill in skillList)
        {
            enemySkills.Enqueue(enemySkill);
        }

        _health = maxHealth;
    }

    public void EndTurn()
    {
        PlayerBattle.Instance.isPlayerTurn = true;
    }

    public void StartTurn()
    {
        PlayerBattle.Instance.isPlayerTurn = false;
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
    public string skillName;
    public int sequence;

    public int CompareTo(EnemySkill other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return sequence.CompareTo(other.sequence);
    }
}
