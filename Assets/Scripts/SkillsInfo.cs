using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SkillsInfo : MonoBehaviour
{
    public static SkillsInfo Instance => _instance;
    static SkillsInfo _instance;

    public Skill[] skillsInfo;

    const string _PATH = "Assets\\Data\\Skills.json";
    
    [Serializable] class Skills
    {
        public Skill[] skills = null;
    }

    [Serializable] public class Skill
    {
        public int id;
        public string name;
        public int directionalActs;
        public int positionalActs;

    }
    
    static Skills CreateFromJson(string jsonPath)
    {
        var jsonData = File.ReadAllText(jsonPath);
        return JsonUtility.FromJson<Skills>(jsonData);
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        skillsInfo = CreateFromJson(_PATH).skills;
    }
}
