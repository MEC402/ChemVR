using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mix_With_Water : TaskStep
{
    private bool isWaterinBeaker = false;
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnStirBeaker += FinishTaskStep; 
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;

    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnStirBeaker -= FinishTaskStep; 
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;
    }

   private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }

}
