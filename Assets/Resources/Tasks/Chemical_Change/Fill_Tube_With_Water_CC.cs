using UnityEngine;

/// <summary>
/// Task 7 of the Physical &amp; Chemical Changes module (Estimate 1 mL): pour DI water into a test tube.
/// Finishes when a container whose name contains <see cref="tubeNameContains"/> (the tubes in TubesInHolder,
/// named "Tube 1", "Tube 2", "Tube 3" in the scene, "Tube (Particle) (n)" in the prefab) receives water and holds at least <see cref="minWater"/> of it.
/// </summary>
public class Fill_Tube_With_Water_CC : TaskStep
{
    [SerializeField] string tubeNameContains = "Tube";
    [SerializeField] float minWater = 1f;

    protected override void SetTaskStepState(string state)
    {
        // No resume state.
    }

    void OnEnable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn += OnPourIn;
    }

    void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn -= OnPourIn;
    }

    void OnPourIn(ChemContainer container, ChemFluid portion)
    {
        if (container == null) return;
        if (container.name.IndexOf(tubeNameContains, System.StringComparison.OrdinalIgnoreCase) < 0)
        {
            Debug.Log("[Fill_Tube_With_Water_CC] ignoring pour into '" + container.name + "' (name does not contain '" + tubeNameContains + "')");
            return;
        }

        float water = 0f;
        foreach (Chem c in container.GetChemFluid().GetChemArray())
            if (c.type == ChemType.WATER) water += c.volume;

        Debug.Log("[Fill_Tube_With_Water_CC] " + container.name + " now holds " + water.ToString("F2") + " water (need >= " + minWater + ")");
        if (water >= minWater) FinishTaskStep();
    }
}
