using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Put_Away : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    /*private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            FinishTaskStep();
        }
    }*/
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnObjsPutAway += FinishTaskStep;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnObjsPutAway -= FinishTaskStep;

    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
