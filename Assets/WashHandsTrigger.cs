using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashHandsTrigger : MonoBehaviour
{
    private bool leftHandWashed = false;
    private bool rightHandWashed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower().Contains("left") || other.CompareTag("Player") /*&& other.name.ToLower().Contains("hand")*/)
        {
            leftHandWashed = true;
        }

        if (other.name.ToLower().Contains("right") || other.CompareTag("Player") /*&& other.name.ToLower().Contains("hand")*/)
        {
            rightHandWashed = true;
        }
    }

    private void Update()
    {
        if (leftHandWashed && rightHandWashed) {
            GameEventsManager.instance.miscEvents.HandsinWater();
        }
    }
}
