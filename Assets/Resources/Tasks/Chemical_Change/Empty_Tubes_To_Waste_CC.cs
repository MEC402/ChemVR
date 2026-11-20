using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Module 3 cleanup step (task 29): finishes once every tube named in <see cref="tubeNames"/> (Tube 1 and Tube 2) has been
/// poured out into the chemical waste container (a disposal ChemContainer whose name contains <see cref="wasteNameContains"/>)
/// until it is empty (at most <see cref="emptyThreshold"/> left).
/// </summary>
public class Empty_Tubes_To_Waste_CC : TaskStep
{
    [SerializeField] string[] tubeNames = { "Tube 1", "Tube 2" };
    [SerializeField] string wasteNameContains = "Waste";
    [SerializeField] float emptyThreshold = 0.01f;

    ChemContainer lastSource;
    readonly HashSet<string> done = new HashSet<string>();

    protected override void SetTaskStepState(string state) { }

    void OnEnable()
    {
        GameEventsManager.instance.chemistryEvents.onPourOut += OnPourOut;
        GameEventsManager.instance.chemistryEvents.onFluidDispose += OnDispose;
    }

    void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourOut -= OnPourOut;
        GameEventsManager.instance.chemistryEvents.onFluidDispose -= OnDispose;
    }

    // PourOut (source) is raised right before FluidDispose (recipient) for the same portion.
    void OnPourOut(ChemContainer origin, ChemFluid portion) { lastSource = origin; }

    void OnDispose(ChemContainer disposal, ChemFluid portion)
    {
        if (disposal == null || lastSource == null) return;
        if (disposal.name.IndexOf(wasteNameContains, System.StringComparison.OrdinalIgnoreCase) < 0) return;

        foreach (string n in tubeNames)
        {
            if (!string.Equals(lastSource.name, n, System.StringComparison.OrdinalIgnoreCase)) continue;
            float left = lastSource.GetChemFluid().totalVolume;
            Debug.Log("[Empty_Tubes_To_Waste_CC] " + lastSource.name + " poured into " + disposal.name + ", " + left.ToString("F3") + " left");
            if (left <= emptyThreshold && done.Add(n))
                Debug.Log("[Empty_Tubes_To_Waste_CC] " + n + " emptied (" + done.Count + "/" + tubeNames.Length + ")");
        }
        if (done.Count >= tubeNames.Length) FinishTaskStep();
    }
}
