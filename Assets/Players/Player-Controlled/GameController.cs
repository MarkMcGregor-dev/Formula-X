using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // events
    public delegate void StartGameDelegate();
    public static event StartGameDelegate GameStarted;

    public delegate void ResetGameDelegate();
    public static event ResetGameDelegate GameReset;

    public delegate void EndGameDelegate(string GameOverString);
    public static event EndGameDelegate GameEnded;
    
    public delegate void ScoreUpdatedDelegate(float newScore, float lastLapDuration);
    public static event ScoreUpdatedDelegate ScoreUpdated;

    public delegate void PlayerRightWayDelegate();
    public static PlayerRightWayDelegate PlayerRightWay;

    public delegate void PlayerWrongWayDelegate();
    public static event PlayerWrongWayDelegate PlayerWrongWay;

    public delegate void PlayerLappedDelegate();
    public static event PlayerLappedDelegate PlayerLapped;

    // variables
    public float scoreMultiplier;
    public float lowestScorableLapDuration;
    public float lapActivationPauseDuration;
    public float score;

    public float timeOfLastLap;
    public float bestLapTime;
    public int numberOfLaps;

    public float delayAfterReset;

    public int indexOfLastCheckpoint;

    private bool waitingForIgnition;
    private float timeWhenStartedWaiting;
    private int lastCheckpoint;

    private void OnEnable()
    {
        // setup event listeners
        PlayerController.CarDead += EndGame;
        PlayerController.CarCrossedLine += OnCarCrossedLine;
        PlayerController.CarIgnition += OnCarIgnition;
        EndGameUIController.ResetRequest += RestartGame;
        CheckpointScript.CheckpointCrossed += OnCheckpointCrossed;
    }

    private void OnDisable()
    {
        // clean up event listeners
        PlayerController.CarDead -= EndGame;
        PlayerController.CarCrossedLine -= OnCarCrossedLine;
        PlayerController.CarIgnition -= OnCarIgnition;
        EndGameUIController.ResetRequest -= RestartGame;
        CheckpointScript.CheckpointCrossed -= OnCheckpointCrossed;
    }

    // Start is called before the first frame update
    void Start()
    {
        // setup variables
        lastCheckpoint = 0;
        timeOfLastLap = Time.time;
        numberOfLaps = 0;
        bestLapTime = 0f;
        waitingForIgnition = true;
        timeWhenStartedWaiting = Time.time;
    }

    private void OnCarIgnition()
    {
        if (waitingForIgnition && (Time.time - timeWhenStartedWaiting) >= 0.25f)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        timeOfLastLap = Time.time;
        waitingForIgnition = false;

        if (GameStarted != null)
        {
            GameStarted();
        }
    }

    private void OnCheckpointCrossed(int checkpointNum)
    {
        Debug.Log("Last checkpoint: " + lastCheckpoint);

        // check if the player is headed the correct way
        //if (checkpointNum >= lastCheckpoint)
        if ((checkpointNum - lastCheckpoint) == 1 || (checkpointNum - lastCheckpoint) == 0)
        {
            // send a right way message
            if (PlayerRightWay != null) PlayerRightWay();

            // update lastCheckpoint
            lastCheckpoint = checkpointNum;

        // if the player is headed the wrong way
        } else
        {
            // send a wrong way message
            if (PlayerWrongWay != null) PlayerWrongWay();
        }
    }

    private void ResetGame()
    {
        // reset variables
        lastCheckpoint = 0;

        if (GameReset != null)
        {
            GameReset();
        }
    }

    private void EndGame(string gameOverString)
    {
        if (GameEnded != null)
        {
            GameEnded(gameOverString);
        }
    }

    private void OnCarCrossedLine()
    {
        // check that the player came from the last checkpoint
        if (lastCheckpoint == indexOfLastCheckpoint)
        {
            // send a right way event
            if (PlayerRightWay != null) PlayerRightWay();

            // send a player lapped event
            if (PlayerLapped != null) PlayerLapped();

            // update the last checkpoint
            lastCheckpoint = 0;

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
                if (bestLapTime == 0 || lapDuration < bestLapTime)
                {
                    bestLapTime = lapDuration;
                }
            } 

        // the player crossed the line the wrong way
        } else if (lastCheckpoint != 0)
        {
            // send a wrong way message
            if (PlayerWrongWay != null) PlayerWrongWay();
        }
    }

    void RestartGame()
    {
        ResetGame();
        waitingForIgnition = true;
        timeWhenStartedWaiting = Time.time;
    }
}
