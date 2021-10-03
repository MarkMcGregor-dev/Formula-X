using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelMeterController : MonoBehaviour
{
    private PlayerController playerController;
    private Slider fuelSlider;
    private bool shouldUpdate;

    private void OnEnable()
    {
        // setup event listeners
        GameController.GameEnded += OnGameOver;
        GameController.GameStarted += OnGameStart;
    }

    private void OnDisable()
    {
        // clean up event listeners
        GameController.GameEnded -= OnGameOver;
        GameController.GameStarted -= OnGameStart;
    }

    // Start is called before the first frame update
    void Start()
    {
        // setup variables
        shouldUpdate = true;
        playerController = GameObject.Find("PlayerCar").GetComponent<PlayerController>();
        fuelSlider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if the fuel should be updating (the game is running)
        if (shouldUpdate)
        {
            // get the fuel amount from the player car
            float fuelValue = playerController.currentFuel / playerController.fuelCapacity;
            fuelSlider.value = fuelValue;
        }
    }

    private void OnGameStart()
    {
        shouldUpdate = true;
    }

    private void OnGameOver(string reason)
    {
        shouldUpdate = false;
    }
}
