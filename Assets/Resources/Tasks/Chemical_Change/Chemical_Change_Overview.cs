using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chemical_Change_Overview : MonoBehaviour
{
    public TextMeshProUGUI ccText;
    int curStep;

    // Controllers for starting tasks
    [Header("Start Task Controllers")]
    public Start_Module taskPreper;

    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = 0;
        ccText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            ccText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAbandonTask += cc_abandonMe;
        GameEventsManager.instance.taskEvents.onAdvanceTask += AdvanceCheTask;

        //Start the Module on opening this scene
        taskPreper.Show();
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
        GameEventsManager.instance.taskEvents.StartTask("Chemical_Change_Task");
        ccText.text = text[curStep];
    }

    void OnDisable()
    {
        //Remove any listeners
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameEventsManager.instance.taskEvents.onAbandonTask -= cc_abandonMe;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= AdvanceCheTask;
    }
    public void restart()
    {
        cc_abandonMe("Chemical_Change_Overview");
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
    }

    string[] text = {"Welcome to the Physical & Chemical Change Lab!\nBefore we begin, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind gloves, goggles, and a lab coat and put them on with (X/A).\n\nPRESS B TO SKIP INSTRUCTIONS! ",
                    //"For this lab, bring the following to a table in the yellow zone:\n250mL beaker\n100mL graduated cylinder\nGlass stir rod\nStock Container of sugar (C6H12O6)\nWeigh boat and paper\n\n\nSkip with B", //disabled task step 2 (add to TaskInfoSO block and uncomment to undo)
                    //"Locate these tools that you will need in the lab:\nAnalytical Balance\nDI water\nWhen you are ready to continue, press (A).\n\n\nSkip with B", //disabled task step 3 (add to TaskInfoSO block and uncomment to undo)
                    "Collect 50mL of DI water into graduated cylinder.\n\n\nSkip with B",
                    "Configure the balance.\n\n\nSkip with B",
                    "Add weigh paper to analytical balance and tare to 0.000g.\n\n\nSkip with B",
                    "Weigh out 2g of sugar.\n\n\nSkip with B",
                    "Fold up sugar in weigh paper and add to empty beaker.\n\n\nSkip with B",
                    "Pour in 50mL water into beaker.\n\n\nSkip with B",
                    "Stir with glass rod.\n\n\nSkip with B", //Solution is clear and transparent.
                    "Is this a chemical change?\nYes or no?\n\n\nSkip with B", //TODO: Make this chooseable
                    "Gather\n250mL beaker\nGlass stir rod\n2 volumetric pipettes\nStock Containers of:\nCrCl3\nNaOH\nH2O2.\n\n\nSkip with B",
                    "Weigh out 2g of CrCl3 on weigh paper on analytical balance.\n\n\nSkip with B", //CrCl3 is Dark green crystalline dry flakes
                    "Fold paper and dump into an empty beaker.\n\n\nSkip with B",
                    "Set up 2 volumetric pipettes with a bulb on top.\n\n\nSkip with B",
                    "Draw up 25mL of NaOH stock solution and place into beaker.\n\n\nSkip with B", //NaOH is clear, transparent liquid
                    "Mix with a glass stir rod.\n\n\nSkip with B", //Color changes forming green solid clumps and the liquid is light green tinted.
                    "Using a new volumetric pipette,\n Draw up 25mL of H2O2 stock solution\n\n\nSkip with B",
                    "Transfer the H2O2 solution into the beaker.\n\n\nSkip with B",
                    "Mix with a glass stir rod.\n\n\nSkip with B", //Color changes from green to dark blue with fizzy bubbles on top.
                    "Is this a chemical change?\nYes or no?\n\n\nSkip with B", 
                    "Place the beaker of solution on a hot plate.\n\n\nSkip with B",
                    "Turns the dial to ON, level 3 on the hot plate.\n\n\nSkip with B", //Liquid slowly boils and vapor comes off top of the beaker.
                    "Is this a chemical change?\nYes or no?\n\n\nSkip with B", //Tell them if correct for each
                    "This concludes the chemical change use module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
            };
    void AdvanceCheTask(string context)
    {
        if (context.Contains("Chemical_Change_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            ccText.text = text[curStep];
        }
    }
    public void cc_abandonMe(string context)
    {
        if (context.Contains("Chemical"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
            GameObject ccPop = GameObject.Find("Chemical Change PopUp");
            ccText.text = "Don't look at me I'm inactive.";
            if (ccPop != null)
            {
                ccPop.SetActive(false);
            }
            curStep = 0;
        }
    }
}
