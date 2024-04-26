using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutOnGlovesTaskStep : TaskStep
{
    private bool playerIsWearingGloves = false;

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.onPlayerWearingGlovesChange += GlovesChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onPlayerWearingGlovesChange -= GlovesChange;
    }

    private void GlovesChange(bool wearingGloves)
    {
        playerIsWearingGloves = wearingGloves;

        if (playerIsWearingGloves)
        {
            FinishTaskStep();
        }
    }

    
    protected override void SetTaskStepState(string state)
    {
        Debug.LogWarning("This isn't fully implemented yet. Sorry.");
    }
}
