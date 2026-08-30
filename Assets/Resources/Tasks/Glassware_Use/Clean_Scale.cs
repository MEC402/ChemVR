using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Clean_Scale : TaskStep
{
    private Scale_Manager scaleManager;

    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        if (scaleManager == null)
            scaleManager = FindFirstObjectByType<Scale_Manager>();

        GameEventsManager.instance.miscEvents.OnCleanScale += HandleCleanScale;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnCleanScale -= HandleCleanScale;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;
    }

    private void HandleCleanScale()
    {
        // The scale just needs to be off right now - it doesn't matter whether that happened
        // before or after this step started.
        if (scaleManager == null || !scaleManager.IsPoweredOn)
            FinishTaskStep();
    }

    private void SkipTask(InputAction.CallbackContext context)
    {
        FinishTaskStep();
    }
}
