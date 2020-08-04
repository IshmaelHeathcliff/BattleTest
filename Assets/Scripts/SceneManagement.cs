using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    static SceneManagement _instance;

    public static SceneManagement Instance => _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PlayerInput.Instance.ReleaseControl();
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        PlayerInput.Instance.GainControl();
    }

    public void Lose()
    {
        GameObject.Find("Menu").transform.Find("Lose").gameObject.SetActive(true);
        Pause();
    }

    public void Win()
    {
        GameObject.Find("Menu").transform.Find("Win").gameObject.SetActive(true);
        Pause();
    }
}
