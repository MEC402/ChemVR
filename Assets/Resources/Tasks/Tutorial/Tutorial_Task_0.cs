using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial_Task_0 : TaskStep
{
    public void Complete()
    {
        Debug.LogWarning("Task 0 complete in itself");
        FinishTaskStep();
    }

    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

}
