using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineAudioScript : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        GameController.PlayerLapped += OnLap;
    }

    private void OnDisable()
    {
        GameController.PlayerLapped -= OnLap;
    }

    private void OnLap()
    {
        audioSource.Play();
    }
}
