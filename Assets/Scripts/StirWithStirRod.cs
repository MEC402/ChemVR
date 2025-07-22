using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirWithStirRod : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Beaker"))
        {
            Debug.Log("event to be called");
            GameEventsManager.instance.miscEvents.StirBeaker();
            Debug.Log("event called");
        }
    }
}
