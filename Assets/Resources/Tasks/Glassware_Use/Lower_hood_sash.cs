using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lower_hood_sash : TaskStep
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
        GameEventsManager.instance.miscEvents.OnHoodSashHeightSet += FinishTaskStep;
        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnHoodSashHeightSet -= FinishTaskStep;
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
