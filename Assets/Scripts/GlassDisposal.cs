using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassDisposal : MonoBehaviour
{
    //The following script is used to dispose of the cracked glass containers used in the simulation.

    private bool objectiveComplete = false;
    private int objectsDisposed = 0;

    public Inspect_Glassware iG;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CrackedGlass"))
        {
            Debug.Log("Glass disposed: " + other.gameObject.name);
            Destroy(other.gameObject);
            objectsDisposed++;
            Debug.Log("Objects disposed: " + objectsDisposed);
            CheckObjectiveCompletion();
        }
    }

    private void CheckObjectiveCompletion()
    {
        if (objectsDisposed >= 2)
        {
            objectiveComplete = true;
            Debug.Log("Objective complete: All cracked glass disposed.");
            iG.AllGlasswareDisposed(); // Notify the Inspect_Glassware script that all glassware has been disposed.
        }
    }

    public bool IsObjectiveComplete()
    {
        return objectiveComplete;
    }
}
