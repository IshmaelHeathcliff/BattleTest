using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    static SceneController _instance;
    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SceneController>();

            if (_instance != null)
                return _instance;

            Create();

            return _instance;
        }
    }

    static void Create()
    {
        GameObject sceneControllerGameObject = new GameObject("SceneController");
        _instance = sceneControllerGameObject.AddComponent<SceneController>();
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
}
