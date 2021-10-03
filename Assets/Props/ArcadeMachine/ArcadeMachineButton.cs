using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMachineButton : MonoBehaviour
{
    public float deflection;
    public float pressSpeed;
    public float releaseSpeed;

    private float distanceThreshold;
    private bool pressedLastFrame;
    private Vector3 startingPosition;

    void Start()
    {
        // setup variables
        distanceThreshold = 0.001f;
        pressedLastFrame = false;
        startingPosition = transform.position;
    }

    void Update()
    {
        // get x key state
        bool isPressed = Input.GetKey(KeyCode.X);

        // determine the desired position and speed of the button
        Vector3 desiredPosition = isPressed ?
            startingPosition + new Vector3(0, deflection, 0) :
            startingPosition;
        float desiredSpeed = isPressed ? pressSpeed : releaseSpeed;

        // handle onClick and onRelease
        if (isPressed && !pressedLastFrame)
        {
            // play a click sound

            pressedLastFrame = true;
        } else if (!isPressed && pressedLastFrame)
        {
            // play a release sound

            pressedLastFrame = false;
        }

        // ensure the button is not close enough to the target position
        if (Mathf.Abs((transform.position - desiredPosition).y) > distanceThreshold)
        {
            // nudge the button towards it's target position
            transform.position = Vector3.Lerp(
                transform.position, desiredPosition, desiredSpeed * Time.deltaTime);
        }
    }
}
