using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class SqLiteController : MonoBehaviour
{
    static SqLiteController _instance;
    public static SqLiteController Instance => _instance;

    SqLiteHelper _skillsHelper;

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        _skillsHelper = new SqLiteHelper("URI=file:" + Application.dataPath + "\\Data\\Data.db");
    }

    void OnDestroy()
    {
        _skillsHelper.CloseConnection();
    }

    public void LearnSkillById(int id)
    {
        if (_skillsHelper.ExecuteQuery($"SELECT id FROM skill WHERE id = {id}").Read())
        {
            _skillsHelper.ExecuteQuery($"INSERT INTO learned_skill(id) VALUES({id})");
        }
    }

    public bool ExistSkillActs(int acts)
    {
        return _skillsHelper.ExecuteQuery($"SELECT * FROM learned_skill_view WHERE positional_acts = {acts}").Read();
    }

    public Skill GetLearnedSkillById(int id)
    {
        var reader = _skillsHelper.ExecuteQuery($"SELECT * FROM learned_skill_view WHERE id = {id}");
        reader.Read();
        return new Skill
        {
            id = reader.GetInt32(reader.GetOrdinal("id")),
            name = reader.GetString(reader.GetOrdinal("skill_name")),
            directionalActs = reader.GetInt32(reader.GetOrdinal("directional_acts")),
            positionalActs = reader.GetInt32(reader.GetOrdinal("positional_acts")),
            damageRatio = reader.GetFloat(reader.GetOrdinal("ratio")),
            previousId = reader.GetInt32(reader.GetOrdinal("previous_id")),
            count = reader.GetInt32(reader.GetOrdinal("count"))
        };
    }


    public Skill GetLearnedSkillByActs(int acts)
    {
        var reader = _skillsHelper.ExecuteQuery($"SELECT * FROM learned_skill_view WHERE positional_acts = {acts}");
        reader.Read();
        return new Skill
        {
            id = reader.GetInt32(reader.GetOrdinal("id")),
            name = reader.GetString(reader.GetOrdinal("skill_name")),
            directionalActs = reader.GetInt32(reader.GetOrdinal("directional_acts")),
            positionalActs = reader.GetInt32(reader.GetOrdinal("positional_acts")),
            damageRatio = reader.GetFloat(reader.GetOrdinal("ratio")),
            previousId = reader.GetInt32(reader.GetOrdinal("previous_id")),
            count = reader.GetInt32(reader.GetOrdinal("count"))
        };
    }

    public IEnumerable<EnemySkill> GetEnemySkills(int id)
    {
        var enemySkills = new List<EnemySkill>();
        var reader = _skillsHelper.ExecuteQuery($"SELECT * FROM enemy_skill WHERE enemy_id = {id}");
        while(reader.Read())
        {
            enemySkills.Add(new EnemySkill
                {
                    damage = reader.GetFloat(reader.GetOrdinal("damage")),
                    skillName = reader.GetString(reader.GetOrdinal("skill_name")),
                    accuracy = reader.GetFloat(reader.GetOrdinal("accuracy")),
                    sequence = reader.GetInt32(reader.GetOrdinal("sequence"))
                }
            );
        }

        enemySkills.Sort();
        return enemySkills;
    }

}
