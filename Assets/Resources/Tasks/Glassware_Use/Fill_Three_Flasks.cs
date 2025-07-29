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
            FinishTaskStep();
        }
    }
    void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
        GameEventsManager.instance.chemistryEvents.onPipetteDispense += addChem;
        mixtureToMatch[0] = new Chem(ChemType.H2O, (15f * (50f / 65f)));
        mixtureToMatch[1] = new Chem(ChemType.CuSO4, (15f * (15f / 65f)));
        
    }
    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }


    private void addChem(PipetteFunctions pipette, ChemContainer container, ChemFluid chemMix)
    {
        if (container.name.ToLower().Contains("Label45mL")) //what is the object we are checking
        {
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];
                if ((currentChem.type == mixtureToMatch[0].type && currentChem.volume == mixtureToMatch[0].volume))
                {
                    check1 = true;
                }
                else if ((currentChem.type == mixtureToMatch[1].type && currentChem.volume == mixtureToMatch[1].volume))
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
        if (container.name.ToLower().Contains("Label55mL")) //what is the object we are checking
        {
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];
                if ((currentChem.type == mixtureToMatch[0].type && currentChem.volume == mixtureToMatch[0].volume))
                {
                    check1 = true;
                }
                else if ((currentChem.type == mixtureToMatch[1].type && currentChem.volume == mixtureToMatch[1].volume))
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
        if (container.name.ToLower().Contains("Label65mL")) //what is the object we are checking
        {
            for (int i = 0; i < pipette.currentFluids.GetChemArray().Length; i++)
            {
                Chem currentChem = pipette.currentFluids.GetChemArray()[i];
                if ((currentChem.type == mixtureToMatch[0].type && currentChem.volume == mixtureToMatch[0].volume))
                {
                    check1 = true;
                }
                else if ((currentChem.type == mixtureToMatch[1].type && currentChem.volume == mixtureToMatch[1].volume))
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
