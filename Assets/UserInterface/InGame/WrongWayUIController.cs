using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrongWayUIController : MonoBehaviour
{
    Text wrongWayText;
    Image wrongWayImage;

    private void Start()
    {
        wrongWayText = transform.Find("Text").GetComponent<Text>();
        wrongWayImage = gameObject.GetComponent<Image>();
        OnRightWay();
    }

    private void OnEnable()
    {
        GameController.PlayerWrongWay += OnWrongWay;
        GameController.PlayerRightWay += OnRightWay;
        GameController.GameReset += OnGameReset;
    }

    private void OnDisable()
    {
        GameController.PlayerWrongWay -= OnWrongWay;
        GameController.PlayerRightWay -= OnRightWay;
        GameController.GameReset -= OnGameReset;
    }

    private void OnGameReset()
    {
        OnRightWay();
    }

    private void OnWrongWay()
    {
        wrongWayImage.enabled = true;
        wrongWayText.enabled = true;
    }

    private void OnRightWay()
    {
        wrongWayImage.enabled = false;
        wrongWayText.enabled = false;
    }
}
