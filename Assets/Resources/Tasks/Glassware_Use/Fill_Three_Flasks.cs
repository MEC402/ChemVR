using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fill_Three_Flasks : TaskStep
{

    private Chem[] mixtureToMatch;

    private bool container1Filled = false;
    private bool container2Filled = false;
    private bool container3Filled = false;

    private bool check1 = false;
    private bool check2 = false;
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            //FinishTaskStep();
        }
    }
    void OnEnable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
        GameEventsManager.instance.chemistryEvents.onPipetteDispense += addChem;
        mixtureToMatch = new Chem[2];
        mixtureToMatch[0] = new Chem(ChemType.H2O, 13);
        mixtureToMatch[1] = new Chem(ChemType.CuSO4, 5);
        Debug.Log("Mixture to match: " + mixtureToMatch[0].type + " " + mixtureToMatch[0].volume + " and " + mixtureToMatch[1].type + " " + mixtureToMatch[1].volume);

    }
    void OnDisable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }


    private void addChem(PipetteFunctions pipette, ChemContainer container, ChemFluid chemMix)
    {
        if (container.name.ToLower().Contains("label45ml")) //what is the object we are checking
        {
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];

                if (currentChem.type == mixtureToMatch[0].type && currentChem.volume <= mixtureToMatch[0].volume && currentChem.volume > 9.0f)
                {

                    check1 = true;
                }
            }
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];
                if (currentChem.type == mixtureToMatch[1].type && currentChem.volume <= mixtureToMatch[1].volume && currentChem.volume > 0.0f)
                {
                    check2 = true;
                }
            }
            if (check1 && check2)
            {
                container1Filled = true;
            }
            check1 = false;
            check2 = false;
        }
        if (container.name.ToLower().Contains("label55ml")) //what is the object we are checking
        {
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];
                if (currentChem.type == mixtureToMatch[0].type && currentChem.volume <= mixtureToMatch[0].volume && currentChem.volume > 9.0f)
                {
                    check1 = true;
                }
            }
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];
                if (currentChem.type == mixtureToMatch[1].type && currentChem.volume <= mixtureToMatch[1].volume && currentChem.volume > 0.0f)
                {
                    check2 = true;
                }
            }
            if (check1 && check2)
            {
                container2Filled = true;
            }
            check1 = false;
            check2 = false;
        }
        if (container.name.ToLower().Contains("label65ml")) //what is the object we are checking
        {
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];
                if (currentChem.type == mixtureToMatch[0].type && currentChem.volume <= mixtureToMatch[0].volume && currentChem.volume > 9.0f)
                {
                    check1 = true;
                }
            }
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];
                if (currentChem.type == mixtureToMatch[1].type && currentChem.volume <= mixtureToMatch[1].volume && currentChem.volume > 0.0f)
                {
                    check2 = true;
                }
            }
            if (check1 && check2)
            {
                container3Filled = true;
            }
            check1 = false;
            check2 = false;
        }

        if (container1Filled && container2Filled && container3Filled)
        {
            FinishTaskStep();
        }
    }
}
