using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void OnDisable()
    {
        // clean up event listeners
        PlayerController.CarDead -= EndGame;
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
}
