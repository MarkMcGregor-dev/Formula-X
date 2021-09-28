using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    GameObject playerCar;
    Vector3 startingPosition;

    void Start()
    {
        // set the starting position
        startingPosition = transform.localPosition;

        // get the player car
        playerCar = GameObject.Find("PlayerCar");
    }

    void Update()
    {
        transform.localPosition = new Vector3(
            startingPosition.x + playerCar.transform.localPosition.x,
            startingPosition.y,
            startingPosition.z + playerCar.transform.localPosition.z);
    }
}
