using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Set_Up_Two_Pipettes : TaskStep
{
    private bool First_PipetConnected = false;
    private bool Second_PipetConnected = false;
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnPippetConnectedFirst += () => First_PipetConnected = true;

        GameEventsManager.instance.miscEvents.OnPippetConnectedSecond += () => Second_PipetConnected = true;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnPippetConnectedFirst -= () => First_PipetConnected = true;

        GameEventsManager.instance.miscEvents.OnPippetConnectedSecond -= () => Second_PipetConnected = true;
    }

    private void Update()
    {
        if (First_PipetConnected && Second_PipetConnected)
        {
            FinishTaskStep();
        }
    }
    
}
