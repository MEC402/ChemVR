using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Set_Up_Pipette : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
        GameEventsManager.instance.miscEvents.OnPippetConnectedFirst += FinishTaskStep;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;
    }
    void OnDisable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
        GameEventsManager.instance.miscEvents.OnPippetConnectedFirst -= FinishTaskStep;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }

}
