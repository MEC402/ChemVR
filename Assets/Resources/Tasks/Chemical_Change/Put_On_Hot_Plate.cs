using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Put_On_Hot_Plate : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnBeakerOnHotPlate += FinishTaskStep;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnBeakerOnHotPlate -= FinishTaskStep;
    }

}
