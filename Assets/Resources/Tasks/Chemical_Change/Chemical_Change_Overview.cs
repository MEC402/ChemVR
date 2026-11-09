using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Instruction popup for the Physical & Chemical Changes module (Lab 3, Reaction #1:
// the chromate-dichromate equilibrium). Step list: docs/module_environment.md.
// Every step is currently a click-through placeholder (Module3_ClickThroughStep);
// see docs/module3_task_sequence.md for how to replace each with its working piece.
//
// IMPORTANT: text[] and webGLText[] must have exactly one more entry than the number of
// taskStepPrefabs on Chemical_Change_Task.asset (the extra last entry is the completion message).
public class Chemical_Change_Overview : MonoBehaviour
{
    public TextMeshProUGUI ccText;
    int curStep;

    // Controllers for starting tasks
    [Header("Start Task Controllers")]
    public Start_Module taskPreper;

    // WebGL builds show the keyboard/mouse wording.
    static bool IsWebGL => Application.platform == RuntimePlatform.WebGLPlayer;
    string[] CurrentText => IsWebGL ? webGLText : text;

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
        ccText.text = CurrentText[curStep];
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

    // Hook a UI Button's OnClick to this to advance a click-through placeholder step.
    public void ClickNext()
    {
        GameEventsManager.instance.miscEvents.ClickThroughNext();
    }

    // VR wording (controllers)
    string[] text = {
                    /*0*/"Step 1 of 11: Background\nStation: PPE\n\nWelcome to Physical and Chemical Changes, Reaction #1: the chromate-dichromate equilibrium.\nLe Chatelier's Principle: if stress is applied to a system at equilibrium, it shifts to relieve the stress.\n\n[placeholder step - press A to continue]",
                    /*1*/"Step 2 of 11: Put on PPE\nStation: PPE\nLabware: goggles, lab coat, gloves\n\nPut on your safety goggles, lab coat and gloves.\n\n[placeholder step - press A to continue]",
                    /*2*/"Step 3 of 11: Gather equipment\nStation: TUBE station (5), Mixing bench (2), fume hood (3)\nLabware: 2 test tubes, test tube rack, stir rod, squirt bottle (DI), bench pipette, Sharpie, K2CrO4, NaOH, H2SO4 bottles, notebook + pen\n\nGather your equipment. Label two test tubes #1 and #2 and bring the three reagent bottles from the fume hood to the bench.\n\n[placeholder step - press A to continue]",
                    /*3*/"Step 4 of 11: Measure 1 mL potassium chromate\nStation: Mixing bench\nLabware: graduated cylinder, K2CrO4 bottle, test tubes #1 and #2\n\nUse the graduated cylinder to measure 1 mL of 0.1 M potassium chromate and pour it into test tubes #1 and #2.\n\n[placeholder step - press A to continue]",
                    /*4*/"Step 5 of 11: Add 4 drops of 1 M NaOH to test tube #1\nStation: Mixing bench\nLabware: 1 M NaOH bottle + dropper, test tube #1\n\nHold the NaOH dropper over test tube #1 and add 4 drops. The solution should stay yellow.\n\n[placeholder step - press A to continue]",
                    /*5*/"Step 6 of 11: Stir the solution\nStation: Mixing bench\nLabware: stir rod, test tube #1\n\nStir test tube #1 with the glass rod for at least 5 seconds.\n\n[placeholder step - press A to continue]",
                    /*6*/"Step 7 of 11: Record observations\nStation: Mixing bench\nLabware: lab notebook, pen, test tube #1\n\nIs this a physical or a chemical change? Then record what you see in the lab notebook.\n\n[placeholder step - press A to continue]",
                    /*7*/"Step 8 of 11: Add 3 M sulfuric acid to test tube #2\nStation: Mixing bench\nLabware: 3 M H2SO4 bottle + dropper, test tube #2, stir rod, notebook + pen\n\nAdd 4 drops of 3 M sulfuric acid to test tube #2, stir, and record what you see. The solution should turn orange.\n\n[placeholder step - press A to continue]",
                    /*8*/"Step 9 of 11: Add 1 M NaOH to test tube #2\nStation: Mixing bench\nLabware: 1 M NaOH bottle + dropper, test tube #2, stir rod, notebook + pen\n\nAdd 4 drops of 1 M NaOH to test tube #2, stir, and record what you see. The solution should return to yellow.\n\n[placeholder step - press A to continue]",
                    /*9*/"Step 10 of 11: Waste disposal\nStation: Fume hood, sink and cleanup\nLabware: both test tubes, chemical waste container, glass waste bin, sink + soap\n\nEmpty both test tubes into the chemical waste container in the fume hood, then put the tubes in the glass waste. Never pour metal ion waste down the drain.\n\n[placeholder step - press A to continue]",
                    /*10*/"Step 11 of 11: Wrap-up\nStation: Any\n\nNaOH removes H+, shifting the equilibrium toward yellow chromate; acid shifts it toward orange dichromate. No precipitate forms.\n\n[placeholder step - press A to continue]",
                    /*11*/"This concludes the Physical and Chemical Changes module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
            };

    // WebGL wording (keyboard / mouse)
    string[] webGLText = {
                    /*0*/"Step 1 of 11: Background\nStation: PPE\n\nWelcome to Physical and Chemical Changes, Reaction #1: the chromate-dichromate equilibrium.\nLe Chatelier's Principle: if stress is applied to a system at equilibrium, it shifts to relieve the stress.\n\n[placeholder step - press N to continue]",
                    /*1*/"Step 2 of 11: Put on PPE\nStation: PPE\nLabware: goggles, lab coat, gloves\n\nPut on your safety goggles, lab coat and gloves.\n\n[placeholder step - press N to continue]",
                    /*2*/"Step 3 of 11: Gather equipment\nStation: TUBE station (5), Mixing bench (2), fume hood (3)\nLabware: 2 test tubes, test tube rack, stir rod, squirt bottle (DI), bench pipette, Sharpie, K2CrO4, NaOH, H2SO4 bottles, notebook + pen\n\nGather your equipment. Label two test tubes #1 and #2 and bring the three reagent bottles from the fume hood to the bench.\n\n[placeholder step - press N to continue]",
                    /*3*/"Step 4 of 11: Measure 1 mL potassium chromate\nStation: Mixing bench\nLabware: graduated cylinder, K2CrO4 bottle, test tubes #1 and #2\n\nUse the graduated cylinder to measure 1 mL of 0.1 M potassium chromate and pour it into test tubes #1 and #2.\n\n[placeholder step - press N to continue]",
                    /*4*/"Step 5 of 11: Add 4 drops of 1 M NaOH to test tube #1\nStation: Mixing bench\nLabware: 1 M NaOH bottle + dropper, test tube #1\n\nHold the NaOH dropper over test tube #1 and add 4 drops. The solution should stay yellow.\n\n[placeholder step - press N to continue]",
                    /*5*/"Step 6 of 11: Stir the solution\nStation: Mixing bench\nLabware: stir rod, test tube #1\n\nStir test tube #1 with the glass rod for at least 5 seconds.\n\n[placeholder step - press N to continue]",
                    /*6*/"Step 7 of 11: Record observations\nStation: Mixing bench\nLabware: lab notebook, pen, test tube #1\n\nIs this a physical or a chemical change? Then record what you see in the lab notebook.\n\n[placeholder step - press N to continue]",
                    /*7*/"Step 8 of 11: Add 3 M sulfuric acid to test tube #2\nStation: Mixing bench\nLabware: 3 M H2SO4 bottle + dropper, test tube #2, stir rod, notebook + pen\n\nAdd 4 drops of 3 M sulfuric acid to test tube #2, stir, and record what you see. The solution should turn orange.\n\n[placeholder step - press N to continue]",
                    /*8*/"Step 9 of 11: Add 1 M NaOH to test tube #2\nStation: Mixing bench\nLabware: 1 M NaOH bottle + dropper, test tube #2, stir rod, notebook + pen\n\nAdd 4 drops of 1 M NaOH to test tube #2, stir, and record what you see. The solution should return to yellow.\n\n[placeholder step - press N to continue]",
                    /*9*/"Step 10 of 11: Waste disposal\nStation: Fume hood, sink and cleanup\nLabware: both test tubes, chemical waste container, glass waste bin, sink + soap\n\nEmpty both test tubes into the chemical waste container in the fume hood, then put the tubes in the glass waste. Never pour metal ion waste down the drain.\n\n[placeholder step - press N to continue]",
                    /*10*/"Step 11 of 11: Wrap-up\nStation: Any\n\nNaOH removes H+, shifting the equilibrium toward yellow chromate; acid shifts it toward orange dichromate. No precipitate forms.\n\n[placeholder step - press N to continue]",
                    /*11*/"This concludes the Physical and Chemical Changes module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
            };

    void AdvanceCheTask(string context)
    {
        if (context.Contains("Chemical_Change_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            ccText.text = CurrentText[Mathf.Min(curStep, CurrentText.Length - 1)];
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
