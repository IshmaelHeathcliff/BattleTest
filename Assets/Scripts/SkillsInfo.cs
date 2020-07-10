using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillsInfo : MonoBehaviour
{
    public static SkillsInfo Instance => _instance;
    static SkillsInfo _instance;

    public List<SkillInfo> skillsInfo;

    const string _PATH = "Assets\\Data\\Skills.json";

    [Serializable]
    class Skills
    {
        public SkillInfo[] skills = null;
    }

    [Serializable]
    public class SkillInfo
    {
        public int id;
        public string name;
        public int directionalActs;
        public int positionalActs;
        public float damageRatio;
        public int previousId;

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

        skillsInfo = new List<SkillInfo>(CreateFromJson(_PATH).skills);
    }

    public SkillInfo GetById(int id)
    {
        return skillsInfo.Find(skillInfo => skillInfo.id == id);
    }
    
    public SkillInfo GetByName(string skillName)
    {
        return skillsInfo.Find(skillInfo => skillInfo.name == skillName);
    }
}
