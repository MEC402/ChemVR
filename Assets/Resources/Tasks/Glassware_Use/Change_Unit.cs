using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Change_Unit : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not necessary for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScaleModeChanged += CheckScaleMode;

        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScaleModeChanged -= CheckScaleMode;

        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext context)
    {
        FinishTaskStep();
    }

    private void CheckScaleMode(string mode)
    {
        if (mode == "grams")
            FinishTaskStep();
    }
}
