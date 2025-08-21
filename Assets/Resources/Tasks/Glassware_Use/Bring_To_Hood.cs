using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bring_To_Hood : TaskStep
{
    //  /*11*/"Bring the copper sulfate  (250mL beaker) and DI water (graduated cylinder) back to your hood, and then open the hood and lower it to a working height.\n\n\nSkip with A",

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
        GameEventsManager.instance.miscEvents.OnObjsBroughtToHood += FinishTaskStep;
        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnObjsBroughtToHood -= FinishTaskStep;
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
