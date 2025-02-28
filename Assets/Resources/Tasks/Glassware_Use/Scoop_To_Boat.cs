public class Scoop_To_Boat : TaskStep
{
    readonly float neededAmountOfSulfur = 1.500f;

    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld += MaterialCheck;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld -= MaterialCheck;
    }

    private void MaterialCheck(float amount)
    {
        if (amount >= neededAmountOfSulfur - 0.05f && amount <= neededAmountOfSulfur + 0.05f)
            FinishTaskStep();
    }
}
