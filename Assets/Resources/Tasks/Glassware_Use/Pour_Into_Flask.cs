using UnityEngine.InputSystem;

public class Pour_Into_Flask : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.AllowBoatTransfer();
        GameEventsManager.instance.miscEvents.EnableFlaskTrigger(true);

        GameEventsManager.instance.miscEvents.OnTransferMaterialToGlass += FinishTaskStep;

        GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.EnableFlaskTrigger(false);

        GameEventsManager.instance.miscEvents.OnTransferMaterialToGlass -= FinishTaskStep;

        GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
