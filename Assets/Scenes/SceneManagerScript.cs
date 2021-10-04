using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Driving,
    Dead,
    WaitingForIgnition,
    AtMenu
}

public enum Scene
{
    InGame,
    Menu
}

public class SceneManagerScript : MonoBehaviour
{
    public delegate void SceneChangedDelegate(Scene scene);
    public static event SceneChangedDelegate SceneChanged;

    public float idleTimeToMenu;

    private GameState currentState;
    private float timeOfLastAction;

    private void OnEnable()
    {
        // setup event listeners
        MainMenuInteraction.ToGameRequest += OnToGameRequest;
        GameController.GameStarted += OnGameStart;
        GameController.GameReset += OnGameReset;
        GameController.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        // clean up event listeners
        MainMenuInteraction.ToGameRequest -= OnToGameRequest;
        GameController.GameStarted -= OnGameStart;
        GameController.GameReset -= OnGameReset;
        GameController.GameEnded -= OnGameEnded;
    }

    void Start()
    {
        // Setup variables
        currentState = GameState.AtMenu;
        timeOfLastAction = Time.time;

        // go to the main menu
        ToMenu();
        //ToGame();
    }

    void Update()
    {
        // get input
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.X))
        {
            timeOfLastAction = Time.time;
        }

        float currentTime = Time.time;

        // check if the player has been idle for too long
        if ((currentTime - timeOfLastAction) >= idleTimeToMenu && currentState != GameState.AtMenu)
        {
            // switch to the main menu scene
            ToMenu();
            timeOfLastAction = Time.time;
        }
    }

    private void ClearScenes()
    {
        try
        {
            SceneManager.UnloadSceneAsync("Menu");
        }
        catch { }

        try
        {
            SceneManager.UnloadSceneAsync("InGame");
        }
        catch { }
    }

    private void ToMenu()
    {
        ClearScenes();
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
        currentState = GameState.AtMenu;
        if (SceneChanged != null) SceneChanged(Scene.Menu);
    }

    private void ToGame()
    {
        ClearScenes();
        SceneManager.LoadSceneAsync("InGame", LoadSceneMode.Additive);
        currentState = GameState.WaitingForIgnition;
        if (SceneChanged != null) SceneChanged(Scene.InGame);
    }

    private void OnToGameRequest()
    {
        ToGame();
    }

    private void OnGameStart()
    {
        currentState = GameState.Driving;
    }

    private void OnGameReset()
    {
        currentState = GameState.WaitingForIgnition;
    }

    private void OnGameEnded(string reason)
    {
        currentState = GameState.Dead;
    }
}
