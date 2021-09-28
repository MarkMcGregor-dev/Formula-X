using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private bool shouldUpdate;
    private PlayerController playerController;
    private GameController gameController;
    private Text scoreText;
    private Text lapTimeText;
    private Text lastLapTimeText;

    private void OnEnable()
    {
        // setup event listeners
        GameController.ScoreUpdated += OnScoreUpdated;
        GameController.GameEnded += OnGameOver;
    }

    private void OnDisable()
    {
        // clean up event listeners
        GameController.ScoreUpdated -= OnScoreUpdated;
        GameController.GameEnded -= OnGameOver;
    }

    void Start()
    {
        // setup variables
        shouldUpdate = true;
        playerController = GameObject.Find("PlayerCar").GetComponent<PlayerController>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        scoreText = transform.Find("ScorePanel").Find("Score").GetComponent<Text>();
        lapTimeText = transform.Find("TimePanel").Find("Time").GetComponent<Text>();
        lastLapTimeText = transform.Find("TimePanel").Find("LastTime").GetComponent<Text>();
    }

    void Update()
    {
        // check if the score should be updating (the game is not over)
        if (shouldUpdate)
        {
            // update the current lap time
            float currentLapTime = Time.time - gameController.timeOfLastLap;
            lapTimeText.text = TimeSpan.FromSeconds(currentLapTime).ToString(@"mm\:ss\.ff");
        }
    }

    private void OnScoreUpdated(float newScore, float lastLapDuration)
    {
        if (shouldUpdate)
        {
            scoreText.text = newScore.ToString();
            lastLapTimeText.text = TimeSpan.FromSeconds(lastLapDuration).ToString(@"mm\:ss\.ff");
        }
    }

    private void OnGameOver(string gameOverString)
    {
        shouldUpdate = false;

        // hide the score panel
        gameObject.SetActive(false);
    }
}
