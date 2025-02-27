public class Balance_Scale : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOn += FinishTaskStep;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOn -= FinishTaskStep;
    }
}
