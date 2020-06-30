using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    public List<int> learnedSkills;
    public List<int> attackActs;
    
    Dictionary<string, int> buttonNumber = new Dictionary<string, int>
    {
        {"LeftDown", 1},
        {"Down", 2},
        {"RightDown", 3},
        {"Left", 4},
        {"Center", 5},
        {"Right", 6},
        {"LeftUp", 7},
        {"Up", 8},
        {"RightUp", 9},
    };

    void Start()
    {
        
    }

    public void AttackAct()
    {
        if (PlayerInput.Instance.Buttons["Dodge"].Held)
        {
            return;
        }

        if (PlayerInput.Instance.Buttons["Defend"].Held)
        {
            return;
        }
        
        

        foreach (var bn in buttonNumber)
        {
           if(PlayerInput.Instance.Buttons[bn.Key].Held) 
               attackActs.Add(bn.Value);
        }

        if (PlayerInput.Instance.Buttons["Confirm"].Held)
        {
            return;
        }



    }
}
