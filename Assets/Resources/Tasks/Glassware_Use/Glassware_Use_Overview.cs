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

    string[] text = {/*0*/"Welcome!\nBefore we begin the lab, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind gloves, goggles, and a lab coat and put them on with (B).",
                    //"For this lab, you will need to bring the required materials to a table in the blue zone, with tables labelled B:\nNote: Inspect all glassware for cracks or chips.\n\nStart with:\n3 250mL Volumetric Flasks.\n\n\nSkip with A", //disabled task step 2 (add to TaskInfoSO block and uncomment to undo)
                    //"Next bring:\n1 100mL Graduated Cylinder\n1 Glass Stir Rod\n1 10mL Volumetric Pipette\n to the table.\n\n\nSkip with A", //disabled task step 3 (add to TaskInfoSO block and uncomment to undo)
                    //"Finally you will need:\nSmall Weigh Boat With Paper\nCopper Sulfate\nPaper Towels\n\n\nSkip with A", //disabled task step 4 (add to TaskInfoSO block and uncomment to undo)
                    //"Locate these tools that you will need in the lab:\nAnalytical Balance\nBalance Brush\nDI water\nWhen you are ready to continue, press (A).\n\n\nSkip with A", //disabled task step 5 (add to TaskInfoSO block and uncomment to undo)
                    /*1*/"Inspect all glassware for cracks or chips.\n\n\nPick them up to inspect them.",
                    /*2*/"Turn on the analytical balance, and allow for it to stabilize.\n\n\nTo do this, press the on button on the left side of the scale.",
                    /*3*/"Place a weigh paper in a weigh boat, and then place the weigh boat on the scale.",
                    /*4*/"Set the scale units to grams by pressing the mode button on the front of the scale.",
                    /*5*/"Press the tare button on the front of the scale to reset the weight to zero.",
                    /*6*/"Use the scoopula to transfer 1.500g copper sulfate to the boat.\n\nTransfer small amounts at a time to avoid transferring too much.",
                    /*7*/"Remember to close the copper sulfate jar.",
                    /*8*/"Pour the contents of the weigh paper into a 250ml beaker.",
                    /*9*/"Turn off the balance and clean it with a brush.",
                    /*10*/"Bring a graduated cylinder over to the DI water.\n\nFill it until the meniscus is at 50mL.\n\n\nSkip with A",
                    /*11*/"Bring the copper sulfate and DI water back to your hood, and then open the hood and lower it to a working height.\n\n\nSkip with A",
                    /*12*/"Set up a volumetric pipette with a bulb top.\n\n\nSkip with A",
                    //"Set up a volumetric pipette with a bulb top.\n\nDraw up water to more than 50mL\nPush excess water out until 50mL\nTouch excess drip to side of the glass\nWipe the tip with folded paper towel.\n\n\nSkip with A",
                    "Pour the DI Water from the graduated cylinder into the 250mL beaker with the Copper Sulfate",
                    "Mix copper sulfate and water with the glass stir rod\n\n\nSkip with A",
                    //"Mix copper sulfate and water with the glass stir rod /*and write down any chemical changes in your notebook.*/\n\n\nSkip with A",
                    "Line up 3 volumetric flasks.\n\nFill each flask with 15mL of the blue fluid using the volumetric pipette.\n\n\nSkip with A",
                    "Fill flasks with 25mL, 35mL, 45mL water respectively and observe and record changes.\n\n\nSkip with A",
                    "Time to clean up! Pre-rinse your glasses with tap water.\n\n\nSkip with A",
                    "Wash your glassware with soapy water to remove any residues.\n\n\nSkip with A",
                    "Scrub the inside of the glassware with a brush.\n\n\nSkip with A",
                    "Rinse the glassware with the DI bottle.\n\n\nSkip with A",
                    "Invert the glassware to allow excess water to drain.\n\n\nSkip with A",
                    "Return materials to their original places, and close the glass doors.\n\nThis will protect them from dust and contaminants.\n\n\nSkip with A",
                    "Go to the trash to remove your gloves, and then wash your hands in the sink with soap.\n\n\nSkip with A",
                    "This concludes the glassware use module. Thank you for your time.\n\n(L Trigger) Hides Popup\n(R Trigger) Opens Menu"
                };

    // Updated to use WebGLText if isWebGL is true
    string[] webGLText = {
                    "Welcome!\nBefore we begin the lab, please hit Escape to pause the game and read the controls. After that, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind gloves, goggles, and a lab coat and put them on with (LeftClick/E).\n\n\nSkip with F",
                    //"For this lab, you will need to bring the required materials to a table in the blue zone, with tables labelled B:\nNote: Inspect all glassware for cracks or chips.\n\nStart with:\n3 250mL Volumetric Flasks.\n\n\nSkip with A", //disabled task step 2 (add to TaskInfoSO block and uncomment to undo)
                    //"Next bring:\n1 100mL Graduated Cylinder\n1 Glass Stir Rod\n1 10mL Volumetric Pipette\n to the table.\n\n\nSkip with A", //disabled task step 3 (add to TaskInfoSO block and uncomment to undo)
                    //"Finally you will need:\nSmall Weigh Boat With Paper\nCopper Sulfate\nPaper Towels\n\n\nSkip with A", //disabled task step 4 (add to TaskInfoSO block and uncomment to undo)
                    //"Locate these tools that you will need in the lab:\nAnalytical Balance\nBalance Brush\nDI water\nWhen you are ready to continue, press (A).\n\n\nSkip with A", //disabled task step 5 (add to TaskInfoSO block and uncomment to undo)
                    "Inspect all glassware for cracks or chips.\n\n\nPick them up to inspect them.\n\n\nSkip with F",
                    "Turn on the analytical balance, and allow for it to stabilize.\n\n\nTo do this, press the on button on the left side of the scale.\n\n\nSkip with F",
                    "Place a weigh paper in a weigh boat, and then place the weigh boat on the scale.\n\n\nSkip with F",
                    "Set the scale units to grams by pressing the mode button on the front of the scale.\n\n\nSkip with F",
                    "Press the tare button on the front of the scale to reset the weight to zero.\n\n\nSkip with F",
                    "Use the scoopula to transfer 1.500g copper sulfate to the boat.\n\nTransfer small amounts at a time to avoid transferring too much.\n\n\nSkip with F",
                    "Remember to close the copper sulfate jar.\n\n\nSkip with F",
                    "Pour the contents of the weigh paper into a 250ml flask.\n\n\nSkip with F",
                    "Turn off the balance and clean it with a brush.\n\n\nSkip with F",
                    "Bring a graduated cylinder over to the DI water.\n\nFill it until the meniscus is at 50mL.\n\n\nSkip with F",
                    "Bring the copper sulfate and DI water back to your hood, and then open the hood and lower it to a working height.\n\n\nSkip with F",
                    "Set up a volumetric pipette with a bulb top.\n\nDraw up water to more than 50mL\nPush excess water out until 50mL\nTouch excess drip to side of the glass\nWipe the tip with folded paper towel.\n\n\nSkip with F",
                    "Mix copper sulfate and water with the glass stir rod and write down any chemical changes in your notebook.\n\n\nSkip with F",
                    "Line up 3 volumetric flasks.\n\nFill each flask with 15mL of the blue fluid using the volumetric pipette.\n\n\nSkip with F",
                    "Fill flasks with 25mL, 35mL, 45mL water respectively and observe and record changes.\n\n\nSkip with F",
                    "Time to clean up! Pre-rinse your glasses with tap water.\n\n\nSkip with F",
                    "Wash your glassware with soapy water to remove any residues.\n\n\nSkip with F",
                    "Scrub the inside of the glassware with a brush.\n\n\nSkip with F",
                    "Rinse the glassware with the DI bottle.\n\n\nSkip with F",
                    "Invert the glassware to allow excess water to drain.\n\n\nSkip with F",
                    "Return materials to their original places, and close the glass doors.\n\nThis will protect them from dust and contaminants.\n\n\nSkip with F",
                    "Go to the trash to remove your gloves, and then wash your hands in the sink with soap.\n\n\nSkip with F",
                    "This concludes the glassware use module. Thank you for your time.\n\n(L Trigger) Hides Popup\n(R Trigger) Opens Menu"
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
