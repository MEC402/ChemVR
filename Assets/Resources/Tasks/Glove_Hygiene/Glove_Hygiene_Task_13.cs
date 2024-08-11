using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glove_Hygiene_Task_13 : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not necessary
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("clip_board"));

        GameEventsManager.instance.miscEvents.onPencilOnPaper += FinishTaskStep;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPencilOnPaper -= FinishTaskStep;
    }
}
