using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCapsuleBehaviour : MonoBehaviour
{
    public float hiddenDuration;

    private Collider fuelCollider;
    private GameObject mesh;    

    private bool isVisible;
    private float timeWhenCollected;

    // Start is called before the first frame update
    void Start()
    {
        // setup variables
        isVisible = false;
        fuelCollider = gameObject.GetComponent<SphereCollider>();
        mesh = transform.Find("Capsule").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // if the capsule should be unhidden
        if (!isVisible && (Time.time - timeWhenCollected) >= hiddenDuration)
        {
            // unhide and activate the capsule
            mesh.SetActive(true);
            fuelCollider.enabled = true;
        }
    }

    public void Collect()
    {
        // hide and disable the capsule
        mesh.SetActive(false);
        fuelCollider.enabled = false;
        isVisible = false;
        timeWhenCollected = Time.time;
    }
}
