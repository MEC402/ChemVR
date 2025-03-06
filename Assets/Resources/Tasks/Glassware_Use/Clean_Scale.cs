public class Clean_Scale : TaskStep
{
    bool hasBeenTurnedOff;

    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOff += () => hasBeenTurnedOff = true;
        GameEventsManager.instance.miscEvents.OnCleanScale += () =>
        {
            if (hasBeenTurnedOff)
                FinishTaskStep();
        };
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOff -= () => hasBeenTurnedOff = true;
        GameEventsManager.instance.miscEvents.OnCleanScale -= () =>
        {
            if (hasBeenTurnedOff)
                FinishTaskStep();
        };
    }
}
