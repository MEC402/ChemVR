using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mix_With_Water : TaskStep
{
    private bool isWaterinBeaker = false;
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn += addChem;
        GameEventsManager.instance.miscEvents.OnStirBeaker += StirEvent;
    }
    void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn -= addChem;
        GameEventsManager.instance.miscEvents.OnStirBeaker -= StirEvent;
    }

    private void addChem(ChemContainer container, ChemFluid chemMix)
    {
        float fiftyMlFull = container.maxVolume / 5; //max is 250mL
        if (container.name.ToLower().Contains("BeakerUp250mL_largerText")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("WATER") || s.Contains("H2O"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt >= fiftyMlFull)
                        {
                            isWaterinBeaker = true;
                        }
                    }
                }
            }
        }
    }
    void StirEvent()
    {
        if (isWaterinBeaker)
        {
            FinishTaskStep();
        }
    }
}
