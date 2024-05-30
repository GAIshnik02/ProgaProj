using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TMP_Text score;
    
    void Start()
    {
        // Инициализация текста счета при старте
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Метод для обновления текста счета
    public void UpdateScore()
    {
        score.text = "ENEMIES KILLED: " + Enemy.enemyKilled;
    }
}