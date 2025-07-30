using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassDisposal : MonoBehaviour
{
    private bool objectiveComplete = false;
    private int objectsDisposed = 0;

    public Inspect_Glassware_And_Dispose iGD;

    private void CollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CrackedGlass"))
        {
            objectsDisposed++;
            Debug.Log($"Glassware disposed: {objectsDisposed}");
            CheckObjectiveComplete();
            Destroy(collision.gameObject);
        }
    }

    private void CheckObjectiveComplete()
    {
        if (objectsDisposed >= 2) // Assuming the objective is to dispose of 3 glassware items
        {
            objectiveComplete = true;
            Debug.Log("Objective Complete: All required glassware disposed.");
            // Trigger any additional logic for completing the task, e.g., updating UI or notifying the player
        }

        if (objectiveComplete)
        {
            iGD.GlasswareDisposed();
        }
    }

    public bool IsObjectiveComplete()
    {
        return objectiveComplete;
    }
}
