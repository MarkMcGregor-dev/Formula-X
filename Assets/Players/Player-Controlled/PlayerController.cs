using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // The speed of the car
    public float maxSpeed;
    public float acceleration;
    public float turnSpeed;

    private bool isX;
    private Rigidbody rb;

    // used for determining velocity
    private float slowFactor;

    void Start()
    {
        // setup variables
        isX = false;
        rb = gameObject.GetComponent<Rigidbody>();
        slowFactor = 0;
    }

    void Update()
    {
        // get input
        isX = Input.GetKey(KeyCode.X);


    }

    private void FixedUpdate()
    {
        // handle the rotation of the car
        float turnFactor = isX ? 1 : -1;
        transform.Rotate(new Vector3(0, 1, 0), turnSpeed * turnFactor);

        // update the velocity of the car
        rb.velocity = transform.forward * maxSpeed;

        //rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * maxSpeed,
        //    0.001f * acceleration);
    }
}
