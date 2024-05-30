using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreCounter2 : MonoBehaviour
{
    public TMP_Text score;
    // public TMP_Text roomsClearedText;

    void Start()
    {
        UpdateScore();
        // UpdateRoomsCleared();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void UpdateScore()
    {
        score.text = "ENEMIES KILLED: " + Enemy.enemyKilled;
    }

    // public void UpdateRoomsCleared()
    // {
    //     roomsClearedText.text = "ROOMS CLEARED: " + PlayerController.RoomsCleared;
    // }
}