using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Add_And_Tare : TaskStep
{
    bool isBoatPrepared = false;
    bool isBoatOnScale = false;
    protected override void SetTaskStepState(string state)
    {
        //not Necessary here
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnPaperInBoat += SetPaperInBoat;
        GameEventsManager.instance.miscEvents.OnObjectOnScale += SetObjectOnScale;
        GameEventsManager.instance.miscEvents.OnScaleTare += CheckFinishTaskStep;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnPaperInBoat -= SetPaperInBoat;
    }
    private void SetPaperInBoat(bool isInBoat) => isBoatPrepared = isInBoat; //set a bool based on if the paper is in the boat
    private void SetObjectOnScale () => isBoatOnScale = true;
    private void CheckFinishTaskStep()
    {
        if (isBoatPrepared && isBoatOnScale)
            FinishTaskStep();
    }
}
