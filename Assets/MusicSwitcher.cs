using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSwitcher : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip inGameMusic;
    public float gameOverVolume;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameController.GameStarted += OnGameStarted;
        GameController.GameEnded += OnGameEnded;
        GameController.GameReset += OnGameReset;
        SceneManagerScript.SceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        GameController.GameStarted -= OnGameStarted;
        GameController.GameEnded -= OnGameEnded;
        GameController.GameReset -= OnGameReset;
        SceneManagerScript.SceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene scene)
    {
        switch (scene) {
            case Scene.Menu:
                audioSource.clip = menuMusic;
                audioSource.Play();
                break;

            case Scene.InGame:
                audioSource.clip = inGameMusic;
                break;
        }
    }

    private void OnGameStarted()
    {
        audioSource.volume = 1;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void OnGameEnded(string reason)
    {
        audioSource.volume = gameOverVolume;
    }

    private void OnGameReset()
    {
        audioSource.volume = gameOverVolume;
    }
}
