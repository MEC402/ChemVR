using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pour_To_Beaker : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    private void OnEnable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn += pouring;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn -= pouring;
    }

    private void pouring(ChemContainer container, ChemFluid transferredChem)
    {
        if (container.name.ToLower().Contains("beaker"))
        {
            FinishTaskStep();
        }
    }
}
