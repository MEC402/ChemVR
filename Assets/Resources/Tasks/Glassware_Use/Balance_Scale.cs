using System;
using UnityEngine.InputSystem;

public class Balance_Scale : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOn += FinishTaskStep;

        GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOn -= FinishTaskStep;

        GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext context)
    {
        FinishTaskStep();
    }
}
