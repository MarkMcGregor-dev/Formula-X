using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void CarLappedDelegate();
    public static event CarLappedDelegate CarLapped;

    public delegate void CarDeadDelegate(string reason);
    public static event CarDeadDelegate CarDead;

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
    public float fuelCapacity;
    public float fuelConsumptionRate;

    public float currentFuel;

    private bool isX;
    private Rigidbody rb;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    // used for determining velocity
    private int lastDirection;
    private float slowFactorGrowth;
    private float slowFactorDecay;
    private float slowFactor;
    private bool isRunning;

    private void OnEnable()
    {
        // setup event listeners
        GameController.GameEnded += OnGameOver;
        GameController.GameStarted += OnGameRestart;
    }

    private void OnDisable()
    {
        // clean up event listeners
        GameController.GameEnded -= OnGameOver;
        GameController.GameStarted -= OnGameRestart;
    }

    void Start()
    {
        // setup variables
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        isRunning = true;
        isX = false;
        rb = gameObject.GetComponent<Rigidbody>();
        slowFactor = 0;
        slowFactorGrowth = 1f;
        slowFactorDecay = 10000f;
        lastDirection = 1;
        currentFuel = fuelCapacity;
    }

    void Update()
    {
        // only get input if the car is running
        if (isRunning)
        {
            // check if the car is out of fuel
            if (currentFuel == 0)
            {
                Debug.Log("Ran out of fuel");

                // kill the car
                if (CarDead != null)
                {
                    CarDead("Out of Fuel");
                }
            } else
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

                // handle the car's fuel
                currentFuel = Mathf.Max(0, currentFuel - (fuelConsumptionRate * Time.deltaTime));
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (isRunning)
        {
            switch (other.tag) {
                // if the other object is a wall
                case "Wall":
                    // kill the car
                    Debug.Log("Hit a wall!");
                    if (CarDead != null)
                    {
                        CarDead("You Crashed");
                    }

                    break;

                // if the other object is a fuel capsule
                case "FuelCapsule":
                    // replenish the fuel amount
                    currentFuel = fuelCapacity;

                    // trigger the fuel capsule's 'collect' event
                    other.GetComponent<FuelCapsuleBehaviour>().Collect();

                    break;

                // if the other object is the finish line
                case "FinishLine":
                    // lap the car
                    if (CarLapped != null)
                    {
                        CarLapped();
                    }

                    break;
            }
        }
    }

    void OnGameOver(string gameOverString)
    {
        // turn off the car
        isRunning = false;
    }

    void OnGameRestart()
    {
        // move the car to the starting position
        transform.position = startingPosition;
        transform.rotation = startingRotation;

        // reset variables
        isRunning = true;
        isX = false;
        lastDirection = 1;
        currentFuel = fuelCapacity;
    }
}
