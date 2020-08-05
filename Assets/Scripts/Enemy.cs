using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Queue<EnemySkill> enemySkills = new Queue<EnemySkill>();
    public EnemyProperty Property { get; set; } = new EnemyProperty();

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
        var skillList = SqLiteController.Instance.GetEnemySkills(Property.id);
        foreach (var enemySkill in skillList)
        {
            enemySkills.Enqueue(enemySkill);
        }

        Indicate();
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
        Property.health -= damage;
        _healthSlider.value = Property.health / Property.maxHealth;
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

public class EnemyProperty
{
    public int id;
    public string enemyName;
    public int[] defence = new int[9];
    public float maxHealth;
    public string imagePath;
    public float health;
}
