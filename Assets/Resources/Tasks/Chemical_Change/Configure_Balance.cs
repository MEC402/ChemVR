using Unity.VisualScripting;
using UnityEngine;

public class Configure_Balance : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
        //this is the script for chem change
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScaleModeChanged += CheckScaleMode;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScaleModeChanged -= CheckScaleMode;
    }

    private void CheckScaleMode(string mode)
    {
            if (mode == "grams")
                FinishTaskStep();
    }
}
