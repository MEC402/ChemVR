using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Start_Pour_HCl : TaskStep
{
    int pinkDrops;
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        pinkDrops = 0;
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
                if (!s.EndsWith(": 0") && (s.Contains("HYDROCHLORIC_ACID") || s.Contains("HCl")))
                {
                    pinkDrops++;
                }
            }
        }

        if (pinkDrops >= 25)
        {
            FinishTaskStepWithDelay(1, 2);
        }
        //Debug.Log(container.name + ": " + chemMix.ContentsToString());
    }
}
