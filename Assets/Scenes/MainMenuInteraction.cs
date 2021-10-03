using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInteraction : MonoBehaviour
{
    public delegate void ToGameRequestDelegate();
    public static event ToGameRequestDelegate ToGameRequest;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && ToGameRequest != null) ToGameRequest();
    }
}
