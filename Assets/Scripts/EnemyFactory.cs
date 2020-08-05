using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    static EnemyFactory _instance;
    public static EnemyFactory Instance => _instance;
    
    public GameObject enemyPrefab;

    public int enemyAmount = 2;

    Dictionary<int, string> _enemyImagePaths;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
        _enemyImagePaths = new Dictionary<int, string>
        {
            {0, Application.dataPath + @"/Artworks/Wolf.png"},
            {1, Application.dataPath + @"/Artworks/yangzi.png"}
        };
    }

    void Start()
    {
        var randomEnemyId = Random.Range(0, enemyAmount);
        var enemy = enemyPrefab.GetComponent<Enemy>();
        var enemyImage = enemyPrefab.GetComponentInChildren<Image>();
        enemy.id = randomEnemyId;
        enemyImage.sprite = LoadTexture2Sprite(_enemyImagePaths[randomEnemyId]);
        Instantiate(enemyPrefab, GameObject.Find("Level").transform);
    }


    byte[] GetImageByte(string imagePath)
    {
        FileStream files = new FileStream(imagePath, FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();
        return imgByte;
    }
    
    Sprite LoadTexture2Sprite(string path)
    {
        Texture2D t2d = new Texture2D(1920, 1080);
        t2d.LoadImage(GetImageByte(path));
        Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), Vector2.zero);
        return sprite;
    }

}
