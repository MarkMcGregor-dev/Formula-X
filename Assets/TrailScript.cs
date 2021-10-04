using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    private TrailRenderer trail;

    void Start()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        GameController.GameStarted += OnGameStarted;
        GameController.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        GameController.GameStarted -= OnGameStarted;
        GameController.GameEnded -= OnGameEnded;
    }

    private void OnGameStarted()
    {
        trail.emitting = true;
    }

    private void OnGameEnded(string reason)
    {
        trail.emitting = false;
    }
}
