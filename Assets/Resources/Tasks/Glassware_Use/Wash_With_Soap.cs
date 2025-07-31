using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wash_With_Soap : TaskStep
{
    private bool flask45filled = false;
    private bool flask55filled = false;
    private bool flask65filled = false;
    private bool graduatedCylinderFilled = false;
    private bool beakerFilled = false;
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

        if (flask45filled && flask55filled && flask65filled && graduatedCylinderFilled && beakerFilled)
        {
            Debug.Log("All are filled with tap.");
            FinishTaskStep();
        }
    }
    void OnEnable()
    {
       // GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
        GameEventsManager.instance.chemistryEvents.onPourIn += addChem;
    }
    void OnDisable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
        GameEventsManager.instance.chemistryEvents.onPourIn -= addChem;
    }

    private void addChem(ChemContainer container, ChemFluid chemMix)
    {
        float oneThirdFull = container.maxVolume / 3 / 3; //max is 250mL
        if (container.name.ToLower().Contains("beakerup250ml")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("Soap") || s.Contains("SOAP"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt >= oneThirdFull)
                        {
                            beakerFilled = true;
                        }
                    }
                }
            }
        }
        if (container.name.ToLower().Contains("graduated")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("Soap") || s.Contains("SOAP"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt >= oneThirdFull)
                        {
                            graduatedCylinderFilled = true;
                        }
                    }
                }
            }
        }
        if (container.name.ToLower().Contains("volumetricflaskwraplabel45ml")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("Soap") || s.Contains("SOAP"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt >= oneThirdFull)
                        {
                            flask45filled = true;
                        }
                    }
                }
            }
        }
        if (container.name.ToLower().Contains("volumetricflaskwraplabel55ml")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("Soap") || s.Contains("SOAP"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt >= oneThirdFull)
                        {
                            flask55filled = true;
                        }
                    }
                }
            }
        }
        if (container.name.ToLower().Contains("volumetricflaskwraplabel65ml")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("Soap") || s.Contains("SOAP"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt >= oneThirdFull)
                        {
                            flask65filled = true;
                        }
                    }
                }
            }
        }
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
