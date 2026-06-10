using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataContainer : MonoBehaviour
{
    [SerializeField] private Transform[] objectResetLocations;
    
    private ChemContainer chemContainer;
    private ChemFluid lastChemFluid;


    private void Start()
    {
        chemContainer = GetComponent<ChemContainer>();

        
        GameEventsManager.instance.taskEvents.onAdvanceTask += SetNewChemFluid;
    }

        /*We can't literally set lastChem to newChem since that would completely change the overall reference to our new chemFluid object. Instead, we want to instead fill this one with identical data,
            then on reset, go back into the ChemContainer and set that components ChemFluid to what is recorded here. This should look similar to how the pipette does it, but simpler since we aren't having
            get a different measure of volume. 

            There's probably an even simpler way to do this because ChemFluid does have overloads for the Add() function to just straight up add ChemFluid to ChemFluid, but I prefer setting it up like this
            since it guarentees things are emptied and filled with the exact amounts for each Chem in the arrays.
        */


    //Called when objects need reset.
    private void ResetContainerChemFluid(string taskID)
    {
        //First, convert saved ChemFluid into an array of Chems, then empty the container's ChemFluid
        Chem[] chemArray = lastChemFluid.GetChemArray();
        chemContainer.EmptyChem();

        //Then, use the chemArray to refill the chemContainer.
        for(int i = 0; i < chemArray.Length; i++)
        {
            chemContainer.AddChem(chemArray[i].type, chemArray[i].volume);
        }
        chemContainer.UpdateChem();

    }
    
    //Called when successfully moving to a new task step (basically the exact same thing but in reverse)
    private void SetNewChemFluid(string taskID)
    {
        //First, get the chemArray from the container, then reset our saved chemFluid.
        Chem[] chemArray = chemContainer.GetChemFluid().GetChemArray();
        lastChemFluid.SetToEmpty();

        //Then, fill the saved chemFluid with the data from the container.
        for(int i = 0; i < chemArray.Length; i++)
        {
            lastChemFluid.Add(chemArray[i].type, chemArray[i].volume);
        }
    }
}
