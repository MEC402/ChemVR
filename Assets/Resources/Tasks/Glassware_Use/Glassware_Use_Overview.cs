using TMPro;
using UnityEngine;

public class Glassware_Use_Overview : MonoBehaviour
{
    public TextMeshProUGUI guText;
    int curStep;
    public bool isWebGL = false;

    // Controllers for starting tasks
    [Header("Start Task Controllers")]
    public Start_Module taskPreper;

     public static bool IsRunningOnWebGL()
     {
         return Application.platform == RuntimePlatform.WebGLPlayer;
     }


    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = 0;
        guText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            guText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAbandonTask += gu_abandonMe;
        GameEventsManager.instance.taskEvents.onAdvanceTask += AdvanceGlaTask;

        //Start the Module on opening this scene
        taskPreper.Show();
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
        GameEventsManager.instance.taskEvents.StartTask("Glassware_Use_Task");
        guText.text = isWebGL ? webGLText[curStep] : text[curStep];
    }

    void OnDisable()
    {
        //Remove any listeners
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameEventsManager.instance.taskEvents.onAbandonTask -= gu_abandonMe;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= AdvanceGlaTask;
    }
    public void restart()
    {
        gu_abandonMe("Glassware_Use_Overview");
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
    }

    //Do you want to add a new instruction here? Follow these steps!
    //add the string text here -- make sure to use a comma after!
    //add a new element to the Glassware_Use_Task task step Prefabs array and move its position in the array until it is correct!
    //duplicate any script in use in the assets > Resources > Tasks > Glassware folder --recommend using one similar to the one you want!
    //change the name of the script in the unity inspector AND in the script itsself (if you dont change it in the script near where monobehavior/Task is the script wont compile)
    //Duplicate a prefab inside the assets > Resources > Tasks > Glassware > Prefabs folder
    //rename the prefab to match the script name --makes it easy to find both
    //replace the prefab's script with the one you want it to have
    //drag the prefab into the task step Prefabs array where the new step you added is

    static string[] text = {/*0*/"Welcome!\nBefore we begin the lab, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind gloves, goggles, and a lab coat and put them on using the left and right triggers on the controllers.",
                    //"For this lab, you will need to bring the required materials to a table in the blue zone, with tables labelled B:\nNote: Inspect all glassware for cracks or chips.\n\nStart with:\n3 250mL Volumetric Flasks.\n\n\nSkip with A", //disabled task step 2 (add to TaskInfoSO block and uncomment to undo)
                    //"Next bring:\n1 100mL Graduated Cylinder\n1 Glass Stir Rod\n1 10mL Volumetric Pipette\n to the table.\n\n\nSkip with A", //disabled task step 3 (add to TaskInfoSO block and uncomment to undo)
                    //"Finally you will need:\nSmall Weigh Boat With Paper\nCopper Sulfate\nPaper Towels\n\n\nSkip with A", //disabled task step 4 (add to TaskInfoSO block and uncomment to undo)
                    //"Locate these tools that you will need in the lab:\nAnalytical Balance\nBalance Brush\nDI water\nWhen you are ready to continue, press (A).\n\n\nSkip with A", //disabled task step 5 (add to TaskInfoSO block and uncomment to undo)
                    /*1*/"Move over to the main work table and inspect the 250 ml Beaker for chips and cracks.\n\n\nPick it up to inspect it.",
                    /*2*/"Turn on the analytical balance, and allow for it to stabilize.\n\n\nTo do this, press the on button on the left side of the scale.",
                    /*3*/"Place the weigh paper in the weigh boat, and then place the weigh boat on the scale.",
                    /*4*/"Set the scale units to grams by pressing the mode button on the front of the scale.",
                    /*5*/"Press the tare button on the front of the scale to reset the weight to zero.",
                    /*6*/"Take the lid off the Copper Sulfate jar.  Pour 1.500g copper sulfate from the jar into the weigh boat.\n\nRotate the jar upside down to pour.",
                    /*7*/"Pick up the Jar Lid and put the lid back on the jar.",
                    /*8*/"Pour the contents of the weigh paper boat into the 250mL beaker. \n\nRotate the weigh boat over the beaker to pour.",
                    /*9*/"Turn off the balance and clean it with the brush. \nPickup the brush and press it to the surface of the scale.",
                    /*10*/"Using the DI water bottle, \nfill the graduated cylinder \nuntil the meniscus is at 50mL.  Pouring will stop when full.",
                    /*11*/"Next, move over to the fume hood, to the right of your main workstation. \n Then, open the fume hood using the open/close button on the left side.",
                    /*12*/"Bring the graduated cylinder and 250mL beaker back to your hood and set them down inside it.",
                    /*13*/"Pick up and inspect the glassware inside the hood.\n\n Place all broken glass in the disposal bin to the right of the fume hood.",
                    /*14*/"Set up two volumetric pipettes. \n\nThe blue bulb should go on the 15mL pipette (larger one) \n\nThe red bulb should go on the 10mL pipette (smaller one).",
                    //"Set up a volumetric pipette with a bulb top.\n\nDraw up water to more than 50mL\nPush excess water out until 50mL\nTouch excess drip to side of the glass\nWipe the tip with folded paper towel.\n\n\nSkip with A",


                    //"Set up a volumetric pipette with a bulb top.\n\nDraw up water to more than 50mL\nPush excess water out until 50mL\nTouch excess drip to side of the glass\nWipe the tip with folded paper towel.\n\n\nSkip with A",
                    //"Use the pipette to transfer water into the 250mL Beaker",
                    /*14*/"Pour 50mL of DI water from the Graduated Cylinder into 250mL beaker.",
                    /*15*/"Mix copper sulfate and water with the glass stir rod.\n\nYou may need to tap the beaker a few times.",
                    //"Mix copper sulfate and water with the glass stir rod and write down any chemical changes in your notebook.\n\n\nSkip with A",
                    /*16*/"With the blue 15mL pipette, use the trigger on your controller to fill each flask with 15mL of the Copper Sulfate & DI Water solution \n(found inside the 250mL beaker)",
                    /*17*/"Then, using the red 10mL pipette, add to the flasks with 30mL, 40mL, & 50mL of DI water respectively.", //25, 35 & 45 are not divisible by 10 or 15 --> we could change this to 10, 20 & 30ml to reduce repetition on this step
                    ///*17*/"Fill flasks with 25mL, 35mL, 45mL water respectively and observe and record changes.\n\n\nSkip with A",
                    /*18*/"Time to clean up! Use the tray to carry the glassware to the sink and pour them out.\nTurn on the sink by clicking on the red button.  Then fill the glassware with sink water.\n\nYou will only need to clean the flasks, 250mL beaker, and graduated cylinder for this module.",
                    /*19*/"Empty the glassware into the sink.\n\n Fill your glassware with soap to remove any residues.",
                    /*20*/"Scrub the inside of the glassware with a brush.\n\nJust tap the glassware with the brush.",
                    /*21*/"Empty the glassware into the sink.\n\nFill the glassware with the DI bottle.",
                    /*22*/"Empty the glassware into the sink.",
                    /*23*/"Return materials to the cabinet on your right, and close the glass doors.\n\nThis will protect them from dust and contaminants.",
                    /*24*/"Go to the trash to remove your gloves. Place your hands inside the trashcan and press the trigger buttons.\n\n Then wash your hands in the sink.",
                    /*25*/"This concludes the glassware use module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
                };

    // Updated to use WebGLText if isWebGL is true
    //string[] webGLText = text;
    string[] webGLText = {/*0*/"Welcome!\nBefore we begin the lab, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind the gloves, goggles, and a lab coat from the PPE table, and put them on using the Mouse button.",
                    //"For this lab, you will need to bring the required materials to a table in the blue zone, with tables labelled B:\nNote: Inspect all glassware for cracks or chips.\n\nStart with:\n3 250mL Volumetric Flasks.\n\n\nSkip with A", //disabled task step 2 (add to TaskInfoSO block and uncomment to undo)
                    //"Next bring:\n1 100mL Graduated Cylinder\n1 Glass Stir Rod\n1 10mL Volumetric Pipette\n to the table.\n\n\nSkip with A", //disabled task step 3 (add to TaskInfoSO block and uncomment to undo)
                    //"Finally you will need:\nSmall Weigh Boat With Paper\nCopper Sulfate\nPaper Towels\n\n\nSkip with A", //disabled task step 4 (add to TaskInfoSO block and uncomment to undo)
                    //"Locate these tools that you will need in the lab:\nAnalytical Balance\nBalance Brush\nDI water\nWhen you are ready to continue, press (A).\n\n\nSkip with A", //disabled task step 5 (add to TaskInfoSO block and uncomment to undo)
                    /*1*/"Move over to the main work table and inspect the 250 ml Beaker for chips and cracks.\n\n\nPick it up to inspect it.",
                    /*2*/"Turn on the analytical balance, and allow for it to stabilize.\n\n\nTo do this, press the on button on the left side of the scale.",
                    /*3*/"Place the weigh paper in the weigh boat, and then place the weigh boat on the scale.",
                    /*4*/"Set the scale units to grams by pressing the mode button on the front of the scale.",
                    /*5*/"Press the tare button on the front of the scale to reset the weight to zero.",
                    /*6*/"Take the lid off the Copper Sulfate jar.  Pour 1.500g copper sulfate from the jar into the weigh boat.\n\nRotate the jar upside down to pour.",
                    /*7*/"Pick up the Jar Lid and press the F key to put the lid back on the jar.",
                    /*8*/"Pour the contents of the weigh paper boat into the 250mL beaker. \n\nRotate the weigh boat over the beaker to pour.",
                    /*9*/"Turn off the balance and clean it with the brush. \nPickup the brush and press it to the surface of the scale.",
                    /*10*/"Using the DI water bottle, \nfill the graduated cylinder \nuntil the meniscus is at 50mL.  Pouring will stop when full. \n\nRotate the DI bottle with T, and use the Red/Blue indicator to line it up.",
                    /*11*/"Next, move over to the fume hood, to the right of your main workstation. \n\n Then, open the fume hood using the open/close button on the left side.",
                    /*12*/"Bring the graduated cylinder and 250mL beaker of copper back to the fume hood and set them down inside it.",
                    /*13*/"Pick up and inspect the glassware inside the hood.\n\n Place all broken glass in the disposal bin to the right of the fume hood.",
                    /*14*/"Set up two volumetric pipettes.\n\nTo attach a bulb to a pipette, first pick up the bulb, then press the F button. \n Then, do the same for the second bulb.",
                    //"Set up a volumetric pipette with a bulb top.\n\nDraw up water to more than 50mL\nPush excess water out until 50mL\nTouch excess drip to side of the glass\nWipe the tip with folded paper towel.\n\n\nSkip with A",
                    //"Use the pipette to transfer water into the 250mL Beaker",
                    /*14*/"Pour 50mL of DI water from the Graduated Cylinder into 250mL beaker.",
                    /*15*/"Mix copper sulfate and water with the glass stir rod.\n\nYou may need to tap the beaker a few times.",
                    //"Mix copper sulfate and water with the glass stir rod and write down any chemical changes in your notebook.\n\n\nSkip with A",
                    /*16*/"With the blue 15mL pipette, rotate the pipette to be horizontal to be able to pull fluid.  Use the F key to fill each flask with 15mL of the Copper Sulfate & DI Water solution \n(found inside the 250mL beaker)",
                    /*17*/"Then, using the red 10mL pipette, add to the flasks with 30mL, 40mL, & 50mL of DI water respectively.", //25, 35 & 45 are not divisible by 10 or 15 --> we could change this to 10, 20 & 30ml to reduce repetition on this step
                    ///*17*/"Fill flasks with 25mL, 35mL, 45mL water respectively and observe and record changes.\n\n\nSkip with A",
                    /*18*/"Time to clean up! Put the glassware onto the brown tray opposite the hood.\n\nTake the full tray to the sink and empty out the glassware.\n\nTurn on the sink by clicking on the red button.  Then fill the glassware with sink water.\n\nYou will only need to clean the flasks, 250mL beaker, and graduated cylinder for this module.",
                    /*19*/"Empty the glassware into the sink.\n\n Fill your glassware with soap to remove any residues.",
                    /*20*/"Scrub the inside of the glassware with a brush.\n\nJust tap the glassware with the brush.",
                    /*21*/"Empty the glassware into the sink.\n\nFill the glassware with the DI bottle.",
                    /*22*/"Empty the glassware into the sink.",
                    /*23*/"Return materials to the cabinet on your right, and close the glass doors.\n\nThis will protect them from dust and contaminants.",
                    /*24*/"Go to the trash to remove your gloves with the F key\n\n Then wash your hands in the sink.",
                    /*25*/"This concludes the glassware use module. Thank you for your time.\n\n"
                };


    void AdvanceGlaTask(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            guText.text = isWebGL ? webGLText[curStep] : text[curStep];
        }
    }
    public void gu_abandonMe(string context)
    {
        if (context.Contains("Glass"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
            GameObject guPop = GameObject.Find("Glassware Use PopUp");
            guText.text = "Don't look at me I'm inactive.";
            if (guPop != null)
            {
                guPop.SetActive(false);
            }
            curStep = 0;
        }
    }

}
