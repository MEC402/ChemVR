using UnityEngine;

/// <summary>
/// Generic "pour X into container Y" step for the Physical &amp; Chemical Changes module. Finishes when a pour lands in a
/// ChemContainer whose name contains <see cref="targetNameContains"/> (case-insensitive) and that container then holds at least
/// <see cref="minVolume"/> of <see cref="chemType"/>. Used by tasks 10 (K2CrO4 into the graduated cylinder),
/// 11 (cylinder into Tube 1) and 12 (cylinder into Tube 2).
/// </summary>
public class Pour_Chem_Into_Container_CC : TaskStep
{
    [SerializeField] string targetNameContains = "Cylinder";
    [SerializeField] ChemType chemType = ChemType.POTASSIUM_CHROMATE;
    [SerializeField] float minVolume = 1f;

    protected override void SetTaskStepState(string state) { }

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
        if (container.name.IndexOf(targetNameContains, System.StringComparison.OrdinalIgnoreCase) < 0)
        {
            Debug.Log("[Pour_Chem_Into_Container_CC] ignoring pour into '" + container.name + "' (waiting for '" + targetNameContains + "')");
            return;
        }

        float vol = 0f;
        foreach (Chem c in container.GetChemFluid().GetChemArray())
            if (c.type == chemType) vol += c.volume;

        Debug.Log("[Pour_Chem_Into_Container_CC] " + container.name + " now holds " + vol.ToString("F2") + " " + chemType + " (need >= " + minVolume + ")");
        if (vol >= minVolume) FinishTaskStep();
    }
}
