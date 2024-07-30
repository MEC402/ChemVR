using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glove_Hygiene_Task_8 : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.SetHint(null);

        GameEventsManager.instance.chemistryEvents.onPourIn += addChem;
    }

    void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn -= addChem;
    }

    private void addChem(ChemContainer container, ChemFluid chemMix)
    {
        float quarterFull = container.maxVolume / 4;
        if (container.name.ToLower().Contains("beaker"))
        {
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                if (s.Contains("HYDROCHLORIC_ACID"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt >= quarterFull)
                        {
                            FinishTaskStep();
                        }
                    }
                }
            }
        }
    }
}
