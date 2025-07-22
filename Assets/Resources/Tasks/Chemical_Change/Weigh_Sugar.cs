using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Weigh_Sugar : TaskStep
{
    public bool isBoatOnScale = false;
    private Scale_Plate scalePlateRef;
    float myWeight;
    float amt;
    protected override void SetTaskStepState(string state)
    {
        //not Necessary here
    }

    private void Start()
    {
        GameObject Spaced_Scale = GameObject.Find("SPACED scale using hover 1");
        scalePlateRef = Spaced_Scale.GetComponentInChildren<Scale_Plate>();
    }
    void OnEnable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn += addChem;
    }
    void OnDisable()
    {
        GameEventsManager.instance.chemistryEvents.onPourIn -= addChem;
    }
    private void Update()
    {
        myWeight = scalePlateRef.measuredWeight;
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
                if (s.Contains("SOLID_SUGAR") || s.Contains("SOLIDC6H12O6"))
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

    private void checkScaleWeight()
    {

        //Debug.Log("My scale weight is: " + myWeight);

        if (myWeight >= 2)
        {
            Debug.Log("My amount is: " + amt);
            //Debug.Log("Scale weighs 2gs");
            FinishTaskStep();
        }
        else
        {
            //Debug.Log("myWeight is not yet 2" + myWeight);

        }
    }
    //pour fluid onto weigh boat
    //check that scale weight is at 2g

}
