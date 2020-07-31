using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsRecall : MonoBehaviour
{
    List<Skill> _learnedSkills = new List<Skill>();
    Text _text;

    void Start()
    {
        _learnedSkills = SqLiteController.Instance.GetLearnedSkills();
        _text = GetComponent<Text>();
        _text.text = "\n\n";
        foreach (var skill in _learnedSkills)
        {
            _text.text += skill;
        }
    }
}
