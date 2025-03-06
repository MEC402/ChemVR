using UnityEngine;

public class Close_Jar : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnJarClosed += CloseJar;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnJarClosed -= CloseJar;
    }

    private void CloseJar(GameObject jar, bool isClosed)
    {
        if (isClosed)
            FinishTaskStep();
    }
}
