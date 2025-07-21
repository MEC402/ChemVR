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
        GameEventsManager.instance.miscEvents.OnPippetConnectedFirst += FinishTaskStep;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnPippetConnectedFirst -= FinishTaskStep;
    }
}
