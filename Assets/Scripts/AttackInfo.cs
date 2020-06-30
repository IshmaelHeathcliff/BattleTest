using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackInfo : MonoBehaviour
{
    Text _text;
    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = SkillsInfo.Instance.skillsInfo[0].name;
    }
}
