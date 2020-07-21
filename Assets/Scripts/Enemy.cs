using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int id;
    public string enemyName;
    public Stack<EnemySkill> enemySkills = new Stack<EnemySkill>();
    public float health = 100f;

    void Start()
    {
        var skillList = SqLiteController.Instance.GetEnemySkills(id);
        foreach (var enemySkill in skillList)
        {
            enemySkills.Push(enemySkill);
        }
    }

    void Update()
    {
        if (!PlayerBattle.Instance.isPlayerTurn && enemySkills.Count != 0)
        {
           PlayerBattle.Instance.HurtBy(enemySkills.Pop());
           PlayerBattle.Instance.isPlayerTurn = true;
        }

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
        return -sequence.CompareTo(other.sequence);
    }
}
