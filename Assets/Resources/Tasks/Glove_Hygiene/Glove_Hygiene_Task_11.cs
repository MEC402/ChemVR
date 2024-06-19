using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Glove_Hygiene_Task_11 : TaskStep
{
    int slapCount;
    protected override void SetTaskStepState(string state)
    {
        //Not needed
    }
    void OnEnable()
    {
        slapCount = 0;
        GameEventsManager.instance.miscEvents.onPrinterSlap += Slap; 
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPrinterSlap -= Slap;
    }

    private void Slap()
    {
        slapCount += 1;
        if (slapCount >= 3)
        {
            FinishTaskStep();
        }
    }

}
