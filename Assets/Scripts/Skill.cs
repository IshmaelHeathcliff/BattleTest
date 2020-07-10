using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    readonly SkillsInfo.SkillInfo _skillInfo;

    public int ID => _skillInfo.id;
    public string Name => _skillInfo.name;
    public int Acts => _skillInfo.positionalActs;
    public float Ratio => _skillInfo.damageRatio;
    public int PreviousID => _skillInfo.previousId;

    public Skill(SkillsInfo.SkillInfo skillInfo)
    {
        _skillInfo = skillInfo;
    }

    public int CountInATurn { get; set; }
    
    public int Count { get; set; }
}
