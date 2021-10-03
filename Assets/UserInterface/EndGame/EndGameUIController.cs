using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIController : MonoBehaviour
{
    public delegate void RestartDelegate();
    public static event RestartDelegate ResetRequest;

    public float restartDelay;

    private GameController gameController;
    private GameObject gameOverUI;
    private bool canReset;
    private float timeWhenEnabled;
    private bool isEnabled;

    private void Start()
    {
        // setup variables
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameOverUI = transform.Find("Game Over Panel").gameObject;
        gameOverUI.SetActive(false);
        canReset = false;
        isEnabled = false;
    }

    private void Update()
    {
        // check if the player is allowed to restart yet
        if (isEnabled && canReset) {
            // check for restart
            if (Input.GetKey(KeyCode.X))
            {
                Debug.Log("Reset requested!");

                if (ResetRequest != null)
                {
                    ResetRequest();
                }
            }
        // check if the time to show the restart button is done
        } else if (isEnabled && (Time.time - timeWhenEnabled) >= restartDelay)
        {
            canReset = true;
        }
    }

    private void OnEnable()
    {
        // create event listeners
        GameController.GameEnded += OnGameOver;
        GameController.GameReset += OnGameReset;
    }

    private void OnDisable()
    {
        // clean up event listeners
        GameController.GameEnded -= OnGameOver;
        GameController.GameReset -= OnGameReset;
    }

    void OnGameOver(string reason)
    {
        EnableUI(reason);
    }

    void OnGameReset()
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
        canReset = false;
    }
}
