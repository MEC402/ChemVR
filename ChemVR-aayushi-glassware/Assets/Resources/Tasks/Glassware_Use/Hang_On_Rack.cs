using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hang_On_Rack : TaskStep
{
    private bool flask45empty = false;
    private bool flask55empty = false;
    private bool flask65empty = false;
    private bool graduatedCylinderempty = false;
    private bool beakerempty = false;
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

        if(beakerempty && graduatedCylinderempty && flask45empty && flask55empty && flask65empty)
        {
            FinishTaskStep();
        }
    }
    void OnEnable()
    {
        GameEventsManager.instance.chemistryEvents.onPourOut += removeChem;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;
    }
    void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourOut -= removeChem;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }


    private void removeChem(ChemContainer container, ChemFluid chemMix)
    {
        //graduated /volumetricflaskwraplabel45ml /volumetricflaskwraplabel55ml /volumetricflaskwraplabel65ml
        if (container.name.ToLower().Contains("beakerup250ml")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("WATER") || s.Contains("H2O") || s.Contains("SOAP") || s.Contains("Soap"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt <= 0)
                        {
                            beakerempty = true;
                        }
                    }
                }
            }
        }
        // /volumetricflaskwraplabel45ml /volumetricflaskwraplabel55ml /volumetricflaskwraplabel65ml
        if (container.name.ToLower().Contains("graduated")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("WATER") || s.Contains("H2O") || s.Contains("SOAP") || s.Contains("Soap"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt <= 0)
                        {
                            graduatedCylinderempty = true;
                        }
                    }
                }
            }
        }
        // / /volumetricflaskwraplabel55ml /volumetricflaskwraplabel65ml
        if (container.name.ToLower().Contains("volumetricflaskwraplabel45ml")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("WATER") || s.Contains("H2O") || s.Contains("SOAP") || s.Contains("Soap"))

                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt <= 0)
                        {
                            flask45empty = true;
                        }
                    }
                }
            }
        }
        // / /volumetricflaskwraplabel55ml /volumetricflaskwraplabel65ml
        if (container.name.ToLower().Contains("volumetricflaskwraplabel55ml")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("WATER") || s.Contains("H2O") || s.Contains("SOAP") || s.Contains("Soap"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt <= 0)
                        {
                            flask55empty = true;
                        }
                    }
                }
            }
        }
        // / /volumetricflaskwraplabel55ml /volumetricflaskwraplabel65ml
        if (container.name.ToLower().Contains("volumetricflaskwraplabel65ml")) //what is the object we are checking
        {
            //NOTE: CHEM_MIX IS THE MIXTURE OF CHEMICALS ADDED TO THE BEAKER, NOT THE TOTAL CHEMICAL MIXTURE. USE CONTAINER.GETCONTENTS()
            string wholeContents = container.getContents();
            string[] sepWholeContents = wholeContents.Split('\n');
            foreach (string s in sepWholeContents)
            {
                //if the object being poured contains the "compoundName" or "chemicalName" 
                if (s.Contains("WATER") || s.Contains("H2O") || s.Contains("SOAP") || s.Contains("Soap"))
                {
                    string strAmnt = s.Split(' ')[1];
                    float amt;
                    if (float.TryParse(strAmnt, out amt))
                    {
                        if (amt <= 0)
                        {
                            flask65empty = true;
                        }
                    }
                }
            }
        }
    }
}
