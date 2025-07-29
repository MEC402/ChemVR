using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Add_WeighBoat : TaskStep
{
    bool boatPrepared = false;

    protected override void SetTaskStepState(string state)
    {
        // Not necessary for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnPaperInBoat += SetPaperInBoat;
        GameEventsManager.instance.miscEvents.OnObjectOnScale += CheckFinishTaskStep;

        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext context)
    {
        FinishTaskStep();
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnPaperInBoat -= SetPaperInBoat;
        GameEventsManager.instance.miscEvents.OnObjectOnScale -= CheckFinishTaskStep;

        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }

    /// <summary>
    /// Sets the state of the paper in the boat. If the paper is in the boat, the task step can be completed.
    /// </summary>
    /// <param name="isInBoat"></param>
    private void SetPaperInBoat(bool isInBoat) => boatPrepared = isInBoat;

    /// <summary>
    /// Checks if the paper is in the boat and if the boat is on the scale. If both are true, the task step can be completed.
    /// </summary>
    private void CheckFinishTaskStep()
    {
        if (boatPrepared)
            FinishTaskStep();
    }
}
