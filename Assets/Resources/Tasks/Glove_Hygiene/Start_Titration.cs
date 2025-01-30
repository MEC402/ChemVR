using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Start_Titration : TaskStep
{
    int blueDrops;
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        blueDrops = 0;
        GameEventsManager.instance.chemistryEvents.onPourIn += addChem;
    }

    void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn -= addChem;
    }

    private void addChem(ChemContainer container, ChemFluid chemMix)
    {
        if (container.name.ToLower().Contains("beaker"))
        {
            string contents = chemMix.ContentsToString();
            string[] sepContents = contents.Split('\n');
            foreach (string s in sepContents)
            {
                if (!s.EndsWith(": 0") && (s.Contains("WATER")||s.Contains("H2O")))
                {
                    blueDrops++;
                }
            }
        }

        if (blueDrops >= 1)
        {
            FinishTaskStep();
        }
    }
}
