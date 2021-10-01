using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIController : MonoBehaviour
{
    public delegate void RestartDelegate();
    public static event RestartDelegate RestartRequest;

    public float restartDelay;

    private GameController gameController;
    private GameObject gameOverUI;
    private bool canRestart;
    private float timeWhenEnabled;
    private bool isEnabled;

    private void Start()
    {
        // setup variables
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameOverUI = transform.Find("Game Over Panel").gameObject;
        gameOverUI.SetActive(false);
        canRestart = false;
        isEnabled = false;
    }

    private void Update()
    {
        // check if the player is allowed to restart yet
        if (isEnabled && canRestart) {
            // check for restart
            if (Input.GetKey(KeyCode.X))
            {
                Debug.Log("Restart requested!");

                if (RestartRequest != null)
                {
                    RestartRequest();
                }
            }
        // check if the time to show the restart button is done
        } else if (isEnabled && (Time.time - timeWhenEnabled) >= restartDelay)
        {
            canRestart = true;
        }
    }

    private void OnEnable()
    {
        // create event listeners
        GameController.GameEnded += OnGameOver;
        GameController.GameStarted += OnGameRestart;
    }

    private void OnDisable()
    {
        // clean up event listeners
        GameController.GameEnded -= OnGameOver;
        GameController.GameStarted -= OnGameRestart;
    }

    void OnGameOver(string reason)
    {
        EnableUI(reason);
    }

    void OnGameRestart()
    {
        DisableUI();
    }

    private void EnableUI(string message)
    {
        gameOverUI.SetActive(true);

        // set the end game text
        Text titleText = gameOverUI.transform.Find("Title Text").gameObject.GetComponent<Text>();
        titleText.text = message;
        
        // set the stats
        Text bestLapTimeText = gameOverUI.transform.Find("StatsPanel").Find("BestLapTime").gameObject.GetComponent<Text>();
        bestLapTimeText.text = TimeSpan.FromSeconds(gameController.bestLapTime).ToString(@"mm\:ss\.ff");
        Text numOfLapsText = gameOverUI.transform.Find("StatsPanel").Find("NumOfLaps").gameObject.GetComponent<Text>();
        numOfLapsText.text = gameController.numberOfLaps.ToString();
        Text finalScoreText = gameOverUI.transform.Find("StatsPanel").Find("FinalScore").gameObject.GetComponent<Text>();
        finalScoreText.text = gameController.score.ToString();

        timeWhenEnabled = Time.time;
        isEnabled = true;
    }

    private void DisableUI()
    {
        gameOverUI.SetActive(false);
        isEnabled = false;
        canRestart = false;
    }
}
