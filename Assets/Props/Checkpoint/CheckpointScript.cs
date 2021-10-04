using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public delegate void CheckpointCrossedDelegate(int num);
    public static event CheckpointCrossedDelegate CheckpointCrossed;

    public int checkpointNum;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnCheckpointTriggered();
        }
    }

    private void OnCheckpointTriggered()
    {
        if (CheckpointCrossed != null) CheckpointCrossed(checkpointNum);
        Debug.Log("CHECKPOINT " + checkpointNum);
    }
}
