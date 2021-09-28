using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // events
    public delegate void StartGameDelegate();
    public static event StartGameDelegate GameStarted;
    
    public delegate void EndGameDelegate(string GameOverString);
    public static event EndGameDelegate GameEnded;
    
    public delegate void ScoreUpdatedDelegate(float newScore, float lastLapDuration);
    public static event ScoreUpdatedDelegate ScoreUpdated;

    // variables
    public float scoreMultiplier;
    public float lowestScorableLapDuration;
    public float lapActivationPauseDuration;
    public float score;

    public float timeOfLastLap;
    public float bestLapTime;
    public int numberOfLaps;

    private bool isGameOver;
    

    private void OnEnable()
    {
        // setup event listeners
        PlayerController.CarDead += EndGame;
        PlayerController.CarLapped += OnCarLapped;
        EndGameUIController.RestartRequest += RestartGame;
    }

    private void OnDisable()
    {
        // clean up event listeners
        PlayerController.CarDead -= EndGame;
        PlayerController.CarLapped -= OnCarLapped;
        EndGameUIController.RestartRequest -= RestartGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        // setup variables
        isGameOver = false;
        timeOfLastLap = Time.time;
        numberOfLaps = 0;
        bestLapTime = 0f;
    }

    private void StartGame()
    {
        if (GameStarted != null)
        {
            GameStarted();
        }
    }

    private void EndGame(string gameOverString)
    {
        if (GameEnded != null)
        {
            GameEnded(gameOverString);
        }
    }

    private void OnCarLapped()
    {
        // calculate the duration of the lap
        float currentTime = Time.time;
        float lapDuration = currentTime - timeOfLastLap;

        // check that the time is within the lap activation pause duration
        if (lapDuration >= lapActivationPauseDuration)
        {
            Debug.Log("Lap Duration: " + lapDuration.ToString());

            // calculate the score increment given the lapDuration
            float newScorePoints = Mathf.Round(scoreMultiplier * Mathf.Pow((Mathf.Max(1, lowestScorableLapDuration - lapDuration)), 2));

            // update the score
            score += newScorePoints;
            if (ScoreUpdated != null)
            {
                ScoreUpdated(score, lapDuration);
            }

            // update the number of laps
            numberOfLaps++;

            // update the last lap time
            timeOfLastLap = currentTime;

            // update the best lap time if applicable
            if (bestLapTime == 0 || timeOfLastLap < bestLapTime)
            {
                bestLapTime = timeOfLastLap;
            }
        } 
    }

    void RestartGame()
    {
        SceneManager.LoadScene("InGame");
    }
}
