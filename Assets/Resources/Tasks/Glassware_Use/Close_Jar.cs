using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Close_Jar : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnJarClosed += CloseJar;

       // GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnJarClosed -= CloseJar;

        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext context)
    {
        FinishTaskStep();
    }

    private void CloseJar(GameObject jar, bool isClosed)
    {
        if (isClosed)
            FinishTaskStep();
    }
}
