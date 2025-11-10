using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Transfer_DI_Into_beaker : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            FinishTaskStep();
        }
    }
    void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;
        GameEventsManager.instance.chemistryEvents.onPourIn += addChem;
    }
    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;
        GameEventsManager.instance.chemistryEvents.onPourIn -= addChem;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }

    private void addChem(ChemContainer container, ChemFluid chemMix)
    {
        float fiftyMlFull = container.maxVolume / 5; //max is 250mL
        if (container.name.ToLower().Contains("beakerup250ml")) //what is the object we are checking
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
                            Debug.Log("fiftymL");
                            FinishTaskStep();
                        }
                    }
                }
            }
        }
    }
}
