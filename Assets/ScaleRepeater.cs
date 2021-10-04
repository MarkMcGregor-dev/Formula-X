using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleRepeater : MonoBehaviour
{
    public float pulseSpeed;
    public float variation;

    private Text text;
    private float baseFontSize;

    private void Start()
    {
        text = gameObject.GetComponent<Text>();
        baseFontSize = text.fontSize;
    }

    void Update()
    {
        text.fontSize = (int)Mathf.Lerp(baseFontSize, baseFontSize + variation, Mathf.Sin(Time.time * pulseSpeed));
    }
}
