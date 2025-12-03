using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Stir_Sugar : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnStirBeaker += FinishTaskStep;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnStirBeaker -= FinishTaskStep;
    }

}
