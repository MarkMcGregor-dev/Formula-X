using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUIController : MonoBehaviour
{
    public delegate void RestartDelegate();
    public static event RestartDelegate RestartRequest;

    public float restartDelay;

    private GameObject gameOverUI;
    private bool canRestart;
    private float timeWhenEnabled;
    private bool isEnabled;

    private void Start()
    {
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
    }

    private void OnDisable()
    {
        // clean up event listeners
        GameController.GameEnded -= OnGameOver;
    }

    void OnGameOver()
    {
        EnableUI();
    }

    private void EnableUI()
    {
        gameOverUI.SetActive(true);
        timeWhenEnabled = Time.time;
        isEnabled = true;
    }
}
