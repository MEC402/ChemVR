using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Add_Water_To_Flasks : TaskStep
{
    private Chem mixtureMatch1;
    private Chem mixtureMatch2;
    private Chem mixtureMatch3;

    private bool container1Filled = false;
    private bool container2Filled = false;
    private bool container3Filled = false;
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
        GameEventsManager.instance.chemistryEvents.onPourIn += addChem;

        mixtureMatch1 = new Chem(ChemType.H2O, 41f);
        mixtureMatch2 = new Chem(ChemType.H2O, 51f);
        mixtureMatch3 = new Chem(ChemType.H2O, 61f);
    }
    void OnDisable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
        //GameEventsManager.instance.chemistryEvents.onPourIn -= addChem;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }


     private void addChem(ChemContainer container, ChemFluid chemMix)
    {
        if (container.name.ToLower().Contains("label45ml") || container.name.ToLower().Contains("label55ml") || container.name.ToLower().Contains("label65ml")) //what is the object we are checking
        {
            for (int i = 0; i < chemMix.GetChemArray().Length; i++)
            {
                Chem currentChem = chemMix.GetChemArray()[i];

                if (container.name.ToLower().Contains("label45ml") && currentChem.type == mixtureMatch1.type && currentChem.volume >= mixtureMatch1.volume)
                {
                    container1Filled = true;
                }
                else if (container.name.ToLower().Contains("label55ml") && currentChem.type == mixtureMatch2.type && currentChem.volume >= mixtureMatch2.volume)
                {
                    container2Filled = true;
                }
                else if (container.name.ToLower().Contains("label65ml") && currentChem.type == mixtureMatch3.type && currentChem.volume >= mixtureMatch3.volume)
                {
                    container3Filled = true;
                }
            }
            if (container1Filled && container2Filled && container3Filled)
            {
                FinishTaskStep();
            }
        }
    }
}
