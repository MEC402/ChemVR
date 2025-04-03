using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tare_Scale : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not necessary for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScaleTare += FinishTaskStep;
        GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScaleTare -= FinishTaskStep;
        GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
