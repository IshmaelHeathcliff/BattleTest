using System;
using Unity;
[Serializable]
internal class Skills
{
    public Skill[] skills = null;
}

[Serializable]
public class Skill
{
    public int id;
    public string name;
    public int directionalActs;
    public int positionalActs;
    public float damageRatio;
    public int previousId;
    public int count;
}
