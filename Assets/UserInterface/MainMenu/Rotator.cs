using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles.x,
            transform.localRotation.eulerAngles.y + (rotationSpeed * Time.deltaTime),
            transform.localRotation.eulerAngles.z);
    }
}
