using UnityEngine;

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
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld -= MaterialCheck;
    }

    private void MaterialCheck(float amount)
    {
        currentAmountOfSulfur += amount;

        // Debug.Log("Current amount of sulfur: " + currentAmountOfSulfur);

        if (currentAmountOfSulfur >= neededAmountOfSulfur - 0.05f && currentAmountOfSulfur <= neededAmountOfSulfur + 0.05f)
            FinishTaskStep();
    }
}
