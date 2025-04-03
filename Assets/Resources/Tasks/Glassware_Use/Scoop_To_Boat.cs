using UnityEngine.InputSystem;

public class Scoop_To_Boat : TaskStep
{
    readonly float neededAmountOfSulfur = 1.500f;
    float currentAmountOfSulfur = 0.0f;

    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld += MaterialCheck;

        GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;

    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld -= MaterialCheck;

        GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }

    private void MaterialCheck(float amount)
    {
        if (amount < 0)
        {
            currentAmountOfSulfur = 0.0f;
            return;
        }

        currentAmountOfSulfur += amount;

        // Debug.Log("Current amount of sulfur: " + currentAmountOfSulfur);

        if (currentAmountOfSulfur >= neededAmountOfSulfur - 0.15f && currentAmountOfSulfur <= neededAmountOfSulfur + 0.15f)
            FinishTaskStep();
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
