using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glassware_Use_Task_1 : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    private void Update()
    {
        // If wearing both gloves, the coat, and goggles.
        if ((AddLeftGlove.HasGloves() && AddRightGlove.HasGloves()) && (WearCoat.IsWearing()) && (WearGoggles.IsWearing()))
        {
            FinishTaskStep();
        }
    }
}
