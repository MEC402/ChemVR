using UnityEngine.InputSystem;

public class Pour_Into_Flask : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    void OnEnable()
    {
        /*    GameEventsManager.instance.miscEvents.AllowBoatTransfer();
            GameEventsManager.instance.miscEvents.EnableFlaskTrigger(true);

            GameEventsManager.instance.miscEvents.OnTransferMaterialToGlass += FinishTaskStep;
*/

        GameEventsManager.instance.chemistryEvents.onPourOut += removeChem;
    }

    void OnDisable()
    {/*
        GameEventsManager.instance.miscEvents.EnableFlaskTrigger(false);

        GameEventsManager.instance.miscEvents.OnTransferMaterialToGlass -= FinishTaskStep;
*/
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
                if (s.Contains("COPPER_SULFATE") || s.Contains("CuSO4"))
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
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
