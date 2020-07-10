using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInput : InputComponent
{
    static PlayerInput _instance;
    public static PlayerInput Instance => _instance;

    [Serializable]
    public struct ButtonKeyCode
    {
        public string name;
        public KeyCode key;

        public ButtonKeyCode(string name, KeyCode key)
        {
            this.name = name;
            this.key = key;
        }
    }
    
    public ButtonKeyCode[] buttonsKeyCode =
    {
        new ButtonKeyCode("Left", KeyCode.J),
        new ButtonKeyCode("LeftUp", KeyCode.U),
        new ButtonKeyCode("LeftDown", KeyCode.M),
        new ButtonKeyCode("Right", KeyCode.L),
        new ButtonKeyCode("RightUp", KeyCode.O),
        new ButtonKeyCode("RightDown", KeyCode.Period),
        new ButtonKeyCode("Up", KeyCode.I),
        new ButtonKeyCode("Center", KeyCode.K),
        new ButtonKeyCode("Down", KeyCode.Comma),
        new ButtonKeyCode("Dodge", KeyCode.H),
        new ButtonKeyCode("Defend", KeyCode.Semicolon),
        new ButtonKeyCode("Confirm", KeyCode.Return),
    };

    public Dictionary<string, InputButton> Buttons { get; } = new Dictionary<string, InputButton>();

    public bool HaveControl { get; private set; } = true;

    void Awake()
    {
        if (_instance == null)
            _instance = this;

        foreach (var button in buttonsKeyCode)
            Buttons.Add(button.name, new InputButton(button.key));
    }

    protected override void GetInputs(bool fixedUpdateHappened)
    {
        foreach (var button in Buttons) 
            button.Value.Get(fixedUpdateHappened);
    }

    public override void GainControl()
    {
        HaveControl = true;

        foreach (var button in Buttons) 
            GainControl(button.Value);
    }

    public override void ReleaseControl(bool resetValue = true)
    {
        HaveControl = false;

        foreach (var button in Buttons)
            ReleaseControl(button.Value, resetValue);
    }
}
