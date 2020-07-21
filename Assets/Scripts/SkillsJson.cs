using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillsJson : MonoBehaviour
{
    public static SkillsJson Instance => _instance;
    static SkillsJson _instance;
    public List<Skill> skillsInfo;

    const string _PATH = "Assets\\Data\\Skills.json";
    public List<Skill> learnedSkills = new List<Skill>();

    static Skills CreateFromJson(string jsonPath)
    {
        var jsonData = File.ReadAllText(jsonPath);
        return JsonUtility.FromJson<Skills>(jsonData);
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        skillsInfo = new List<Skill>(CreateFromJson(_PATH).skills);
    }

    void Start()
    {
        foreach (var skillInfo in skillsInfo)
        {
            learnedSkills.Add(skillInfo);
        }
    }

    public Skill GetById(int id)
    {
        return skillsInfo.Find(skillInfo => skillInfo.id == id);
    }
    
    public Skill GetByName(string skillName)
    {
        return skillsInfo.Find(skillInfo => skillInfo.name == skillName);
    }
}
