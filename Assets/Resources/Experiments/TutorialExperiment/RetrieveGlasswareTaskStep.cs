using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveGlasswareTaskStep : TaskStep
{
    private bool flaskCollected = false;
    private bool beakerCollected = false;

    private void OnEnable()
    {
        GameEventsManager.instance.interactableEvents.onPlayerGrabInteractable += InteractableGrabbed;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.interactableEvents.onPlayerGrabInteractable -= InteractableGrabbed;
    }

    private void InteractableGrabbed(GameObject interactable)
    {
        if (interactable.name.StartsWith("Beaker"))
        {
            beakerCollected = true;
        }
        else if (interactable.name.StartsWith("Flask"))
        {
            flaskCollected = true;
        }

        if (flaskCollected && beakerCollected)
        {
            FinishTaskStep();
        }

    }


    protected override void SetTaskStepState(string state)
    {
        Debug.LogWarning("This isn't fully implemented yet. Sorry.");
    }
}
