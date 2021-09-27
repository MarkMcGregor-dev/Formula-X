using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // The top speed of the car
    public float maxSpeed;
    // The slowest running speed of the car (max turning)
    public float minSpeed;
    // The acceleration of the car (while driving straight)
    public float acceleration;
    // The decceleration of the car (while braking/turning)
    public float decceleration;
    // The turn speed of the car
    public float turnSpeed;

    private bool isX;
    private Rigidbody rb;

    // used for determining velocity
    private int lastDirection;
    private float slowFactorGrowth;
    private float slowFactorDecay;
    private float slowFactor;
    private bool isRunning;

    void Start()
    {
        // setup variables
        isRunning = true;
        isX = false;
        rb = gameObject.GetComponent<Rigidbody>();
        slowFactor = 0;
        slowFactorGrowth = 1f;
        slowFactorDecay = 10000f;
        lastDirection = 1;
    }

    void Update()
    {
        // only get input if the car is running
        if (isRunning)
        {
            // get input
            isX = Input.GetKey(KeyCode.X);

            int direction = isX ? 1 : -1;

            // if the car is turning more
            if (lastDirection == direction)
            {
                slowFactor = Mathf.Min(1, slowFactor + (slowFactorGrowth * Time.deltaTime));

            // if the car is straightening out
            } else
            {
                slowFactor = Mathf.Max(0, slowFactor - (slowFactorDecay * Time.deltaTime));
            }

            lastDirection = direction;

            Debug.DrawRay(
                transform.position, transform.forward * Mathf.Abs(slowFactor) * 10, Color.red,
                Time.deltaTime, false);
        }
    }

    private void FixedUpdate()
    {
        // only drive the car if it's running
        if (isRunning)
        {
            // handle the rotation of the car
            float turnFactor = isX ? 1 : -1;
            transform.Rotate(new Vector3(0, 1, 0), turnSpeed * turnFactor);

            rb.velocity = transform.forward * ((maxSpeed + minSpeed) / 2);

            //    // calculate the desired speed based on the slow factor
            //    Vector3 desiredVelocity = transform.forward * Mathf.Lerp(maxSpeed, minSpeed, slowFactor);

            //    Debug.Log(desiredVelocity);

            //    Vector3 newVelocity;

            //    // if the car is accelerating
            //    if (desiredVelocity.z > rb.velocity.z)
            //    {
            //        newVelocity = new Vector3(
            //            0,
            //            0,
            //            Mathf.Lerp(rb.velocity.z, desiredVelocity.z, acceleration * Time.deltaTime));

            //        // if the car is deccelerating
            //    }
            //    else
            //    {
            //        newVelocity = new Vector3(
            //            0,
            //            0,
            //            Mathf.Lerp(rb.velocity.z, desiredVelocity.z, decceleration * Time.deltaTime));
            //    }

            //    // update the velocity of the car
            //    rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
        }
    }   
}
