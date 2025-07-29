using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassDisposal : MonoBehaviour
{
    //The following script is used to dispose of the cracked glass containers used in the simulation.

    private bool objectiveComplete = false;
    private int objectsDisposed = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CrackedGlass"))
        {
            Destroy(other.gameObject);
            objectsDisposed++;
            CheckObjectiveCompletion();
        }
    }

    private void CheckObjectiveCompletion()
    {
        if (objectsDisposed >= 2)
        {
            objectiveComplete = true;
        }
    }

    public bool IsObjectiveComplete()
    {
        return objectiveComplete;
    }
}
