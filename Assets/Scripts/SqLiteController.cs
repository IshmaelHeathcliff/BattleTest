using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class SqLiteController : MonoBehaviour
{
    static SqLiteController _instance;
    public static SqLiteController Instance => _instance;

    SqLiteHelper _sqliteHelper;

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        _sqliteHelper = new SqLiteHelper("URI=file:" + Application.dataPath + "\\Data\\Data.db");
    }

    void OnDestroy()
    {
        _sqliteHelper.CloseConnection();
    }
    
    #region player
    public void LearnSkillById(int id)
    {
        if (_sqliteHelper.ExecuteQuery($"SELECT id FROM skill WHERE id = {id}").Read())
        {
            _sqliteHelper.ExecuteQuery($"INSERT INTO learned_skill(id) VALUES({id})");
        }
    }

    public bool ExistSkillActs(long acts)
    {
        return _sqliteHelper.ExecuteQuery($"SELECT * FROM learned_skill_view WHERE positional_acts = {acts}").Read();
    }

    public List<Skill> GetLearnedSkills()
    {
        var learnedSkills = new List<Skill>();
        var reader = _sqliteHelper.ExecuteQuery($"SELECT * FROM learned_skill_view");
        while (reader.Read())
        {
            learnedSkills.Add(new Skill
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                name = reader.GetString(reader.GetOrdinal("skill_name")),
                directionalActs = reader.GetInt64(reader.GetOrdinal("directional_acts")),
                positionalActs = reader.GetInt64(reader.GetOrdinal("positional_acts")),
                damageRatio = reader.GetFloat(reader.GetOrdinal("ratio")),
                previousId = reader.GetInt32(reader.GetOrdinal("previous_id")),
                count = reader.GetInt32(reader.GetOrdinal("count"))
            });
        }

        return learnedSkills;
    }
    
    public Skill GetLearnedSkillById(int id)
    {
        var reader = _sqliteHelper.ExecuteQuery($"SELECT * FROM learned_skill_view WHERE id = {id}");
        reader.Read();
        return new Skill
        {
            id = reader.GetInt32(reader.GetOrdinal("id")),
            name = reader.GetString(reader.GetOrdinal("skill_name")),
            directionalActs = reader.GetInt64(reader.GetOrdinal("directional_acts")),
            positionalActs = reader.GetInt64(reader.GetOrdinal("positional_acts")),
            damageRatio = reader.GetFloat(reader.GetOrdinal("ratio")),
            previousId = reader.GetInt32(reader.GetOrdinal("previous_id")),
            count = reader.GetInt32(reader.GetOrdinal("count"))
        };
    }

    public Skill GetLearnedSkillByActs(long acts)
    {
        var reader = _sqliteHelper.ExecuteQuery($"SELECT * FROM learned_skill_view WHERE positional_acts = {acts}");
        reader.Read();
        return new Skill
        {
            id = reader.GetInt32(reader.GetOrdinal("id")),
            name = reader.GetString(reader.GetOrdinal("skill_name")),
            directionalActs = reader.GetInt64(reader.GetOrdinal("directional_acts")),
            positionalActs = reader.GetInt64(reader.GetOrdinal("positional_acts")),
            damageRatio = reader.GetFloat(reader.GetOrdinal("ratio")),
            previousId = reader.GetInt32(reader.GetOrdinal("previous_id")),
            count = reader.GetInt32(reader.GetOrdinal("count"))
        };
    }
    #endregion
    
    #region enemy
    public IEnumerable<EnemySkill> GetEnemySkills(int id)
    {
        var enemySkills = new List<EnemySkill>();
        var reader = _sqliteHelper.ExecuteQuery($"SELECT * FROM enemy_skill WHERE enemy_id = {id}");
        while(reader.Read())
        {
            enemySkills.Add(new EnemySkill
                {
                    damage = reader.GetFloat(reader.GetOrdinal("damage")),
                    skillName = reader.GetString(reader.GetOrdinal("skill_name")),
                    accuracy = reader.GetFloat(reader.GetOrdinal("accuracy")),
                    sequence = reader.GetInt32(reader.GetOrdinal("sequence")),
                    penetration = reader.GetFloat(reader.GetOrdinal("penetration"))
                }
            );
        }

        enemySkills.Sort();
        return enemySkills;
    }
    #endregion
}
