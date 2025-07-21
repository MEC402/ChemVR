using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Scoop_To_Boat : TaskStep
{/*
    readonly float neededAmountOfSulfur = 1.500f;
    float currentAmountOfSulfur = 0.0f;*/
    float amt;
    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        /*GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld += MaterialCheck;

        GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;*/
        GameEventsManager.instance.chemistryEvents.onPourIn += addChem;
    }

    void OnDisable()
    {
        /*GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld -= MaterialCheck;

        GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;*/
        GameEventsManager.instance.chemistryEvents.onPourIn -= addChem;
    }
    private void addChem(ChemContainer container, ChemFluid chemMix)
    {
        float twoThirdsFull = container.maxVolume / 1.5f; //if you divide by 1.5 you essentially multiply by 2/3
        if (container.name.ToLower().Contains("weigh_boat_fluids_small")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("COPPER_SULFATE") || s.Contains("CuSO4"))
                {
                    string strAmnt = s.Split(' ')[1];

                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt >= twoThirdsFull)
                        {
                            //Debug.Log("the amount poured was at least 2/3 of the amount necessary");
                            //checkScaleWeight();
                            //Invoke("checkScaleWeight", .5f);
                            FinishTaskStep();
                        }
                    }
                }
            }
        }
    }
    /*   private void MaterialCheck(float amount)
       {
           if (amount < 0)
           {
               currentAmountOfSulfur = 0.0f;
               return;
           }

           currentAmountOfSulfur += amount;

           // Debug.Log("Current amount of sulfur: " + currentAmountOfSulfur);

           if (currentAmountOfSulfur >= neededAmountOfSulfur - 0.15f && currentAmountOfSulfur <= neededAmountOfSulfur + 0.15f)
               FinishTaskStep();
       }

       private void SkipTask(InputAction.CallbackContext obj)
       {
           FinishTaskStep();
       }*/
}
