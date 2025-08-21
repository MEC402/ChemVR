using System;
using UnityEngine.InputSystem;

public class Clean_Scale : TaskStep
{
    bool hasBeenTurnedOff;

    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOff += () => hasBeenTurnedOff = true;
        GameEventsManager.instance.miscEvents.OnCleanScale += () =>
        {
            if (hasBeenTurnedOff)
                FinishTaskStep();
        };

        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOff -= () => hasBeenTurnedOff = true;
        GameEventsManager.instance.miscEvents.OnCleanScale -= () =>
        {
            if (hasBeenTurnedOff)
                FinishTaskStep();
        };

        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext context)
    {
        FinishTaskStep();
    }
}
