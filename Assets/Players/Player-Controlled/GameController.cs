using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // events
    public delegate void EndGameDelegate();
    public static event EndGameDelegate GameEnded;

    private bool isGameOver;

    private void OnEnable()
    {
        // setup event listeners
        PlayerController.CarDead += EndGame;
        EndGameUIController.RestartRequest += RestartGame;
    }

    private void OnDisable()
    {
        // clean up event listeners
        PlayerController.CarDead -= EndGame;
        EndGameUIController.RestartRequest -= RestartGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EndGame()
    {
        if (GameEnded != null)
        {
            GameEnded();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene("InGame");
    }
}
