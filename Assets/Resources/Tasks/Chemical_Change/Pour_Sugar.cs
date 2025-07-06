using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Timeline.Actions.MenuPriority;


public class Pour_Sugar : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        //not Necessary here
    }
    void OnEnable()
    {
        GameEventsManager.instance.chemistryEvents.onPourOut += removeChem; 
    }
    void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourOut -= removeChem;
    }
    private void removeChem(ChemContainer container, ChemFluid chemMix)
    {
        //float containerFill = container.maxVolume / 1;
        if (container.name.ToLower().Contains("weigh_boat_fluids_small")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("SOLID_SUGAR") || s.Contains("SOLIDC6H12O6"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        //Debug.Log("boat amount: " + amt);
                        if (amt <= 0)
                        {
                            //Debug.Log("poured complete contents into a beaker");
                            FinishTaskStep();
                        }
                    }
                }
            }
        }
    }
}
