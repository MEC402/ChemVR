using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wash_Hands : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    private bool leftGloveOff = false;
    private bool rightGloveOff = false;
    private bool handsinwater = false;
    private GameObject WashTrigger;
    private WashHandsTrigger WHT;
    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            FinishTaskStep();
        }

        if (handsinwater)
        {
            FinishTaskStep();
        }
        /*if (leftGloveOff && rightGloveOff && handsinwater)*/ //need to create the trigger for the water part
    }
    void OnEnable()
    {
        WashTrigger = GameObject.Find("WashHandsTrigger");
        WHT = WashTrigger.GetComponent<WashHandsTrigger>();

        if (WHT != null)
        {
            WHT.ActivateWashHands();
        }



        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove += () => leftGloveOff = true;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove += () => rightGloveOff = true;
        GameEventsManager.instance.miscEvents.onHandsinWater += () =>
        {
            if (leftGloveOff && rightGloveOff)
            {
                handsinwater = true;
            }
        };
         GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;
    }
    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
