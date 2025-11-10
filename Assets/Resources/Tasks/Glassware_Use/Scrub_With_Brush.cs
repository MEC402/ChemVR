using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scrub_With_Brush : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            FinishTaskStep();
        }
    }
    void OnEnable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
        GameEventsManager.instance.miscEvents.OnScrubWithBrush += FinishTaskStep;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;
    }
    void OnDisable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
        GameEventsManager.instance.miscEvents.OnScrubWithBrush -= FinishTaskStep;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
