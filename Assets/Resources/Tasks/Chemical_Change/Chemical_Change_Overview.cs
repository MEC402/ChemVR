using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Instruction popup for the Physical & Chemical Changes module (Lab 3, Reaction #1:
// the chromate-dichromate equilibrium). Popup text follows the module document
// (docs/VR Chemistry Module 3 - Physical and Chemical Changes UPDATED.docx), see docs/chemmodule_tasks.md.
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
                    /*0*/"Step 1 of 11: Background\nStation: 1 PPE\n\nIn this experiment, 9 reactions at equilibrium will be perturbed, and the resulting chemical and physical changes will be observed. A physical change occurs when matter changes form. A chemical change occurs when new products are formed. In this simulation you will do reaction #1 from Lab 3: The Law of Mass Action: Le Chatelier's Principle.\n\nLe Chatelier's Principle allows us to predict the effect of change upon a chemical system in equilibrium. If stress is imposed on a system at equilibrium, the equilibrium concentrations of reactants and products shift in such a manner as to relieve the stress.\n\n[placeholder step - press A to continue]",
                    /*1*/"Step 2 of 11: Safety first: put on PPE\nStation: 1 PPE\nLabware: goggles, lab coat, gloves\n\nBefore starting any chemistry experiment, you must wear proper Personal Protective Equipment (PPE). Put on your safety goggles, lab coat and gloves (your hands turn blue).\n\nSAFETY PRECAUTION: several reactions in this experiment use acids and bases at 1 M or stronger. Be extremely careful with corrosive acids and bases. Clean up all spills or drips immediately as directed by your TA, and do not leave drops of acid or base on the sides of the bottles.\n\nGoggles protect your eyes from splashes; lab coats protect your skin and clothing. In this lab you will generate metal ion waste, disposed of in the fume hood, and glass waste (the test tubes).\n\n[placeholder step - press A to continue]",
                    /*2*/"Step 3 of 11: Gather equipment\nStation: 5 TUBE, 2 MIXING, 3 HOOD\nLabware: 2 test tubes, test tube rack, glass stir rod, squirt bottle of water, disposable pipette, Sharpie, potassium chromate (K2CrO4), sodium hydroxide (NaOH), sulfuric acid (H2SO4), notebook + pen\n\nThis is a qualitative experiment: measurements do not have to be exact, detailed observations matter most. You will mix approximately 1 mL solutions.\n\nGather two test tubes, the test tube rack and the glass stir rod, plus the squirt bottle of water and a disposable pipette on the bench. Place both test tubes in the rack. Use the pipette to measure about 1 mL of water into one test tube to estimate the amounts you will add later. Label one test tube #1 and the other #2 with the Sharpie.\n\nThen find potassium chromate, sodium hydroxide and sulfuric acid in the fume hood. They are in two trays labeled acids and bases, and each bottle has a disposable pipette taped to it. Bring all three bottles to your work area.\n\n[placeholder step - press A to continue]",
                    /*3*/"Step 4 of 11: Measure 1 mL of 0.1 M potassium chromate\nStation: 2 MIXING\nLabware: graduated cylinder, potassium chromate bottle, test tubes #1 and #2\n\nUse the graduated cylinder to collect 1 mL of 0.1 M potassium chromate solution. Hold the cylinder under the reagent bottle and pour; the flow stops automatically at 1 mL.\n\nPour the yellow solution into test tube #1, then repeat for test tube #2.\n\n[placeholder step - press A to continue]",
                    /*4*/"Step 5 of 11: Add 1 M sodium hydroxide to test tube #1\nStation: 2 MIXING\nLabware: 1 M sodium hydroxide bottle + dropper, test tube #1\n\nOpen the reagent bottle that says 1 M sodium hydroxide and use the dropper.\n\nPick up the bottle and its dropper, put the bottle down, and hold the dropper over test tube #1. Push the button to add drops; the system stops automatically after 4 drops.\n\n[placeholder step - press A to continue]",
                    /*5*/"Step 6 of 11: Stir the solution\nStation: 2 MIXING\nLabware: stir rod, test tube #1\n\nUse the glass stir rod to mix the solution inside the test tube until the drops completely dissolve.\n\nPick up the stir rod, place it in test tube #1, and stir (hold the button for automatic stirring) for at least 5 seconds. Then place the rod on the counter.\n\n[placeholder step - press A to continue]",
                    /*6*/"Step 7 of 11: Record your observations\nStation: 2 MIXING\nLabware: lab notebook, pen, test tube #1\n\nIs this a physical change or a chemical change? Select the statement that matches what you see in test tube #1. If it is not right, think about whether a new substance was created and try again.\n\nThen pick up the pen and record your observations in the lab notebook.\n\n[placeholder step - press A to continue]",
                    /*7*/"Step 8 of 11: Add 3 M sulfuric acid to test tube #2\nStation: 2 MIXING\nLabware: 3 M sulfuric acid bottle + dropper, test tube #2, stir rod, notebook + pen\n\nOpen the reagent bottle that says 3 M sulfuric acid and use the dropper.\n\nPick up the bottle and its dropper, put the bottle down, and hold the dropper over test tube #2. Push the button to add drops; the system stops automatically after 4 drops.\n\nPick up the stir rod, place it in test tube #2, and stir (hold the button for automatic stirring) for at least 5 seconds. Put the rod on the counter.\n\nThen pick up the pen and record what you see in the lab notebook.\n\n[placeholder step - press A to continue]",
                    /*8*/"Step 9 of 11: Add 1 M sodium hydroxide to test tube #2\nStation: 2 MIXING\nLabware: 1 M sodium hydroxide bottle + dropper, test tube #2, stir rod, notebook + pen\n\nOpen the reagent bottle that says 1 M sodium hydroxide and use the dropper.\n\nPick up the bottle and its dropper, put the bottle down, and hold the dropper over test tube #2. Push the button to add drops; the system stops automatically after 4 drops.\n\nPick up the stir rod, place it in test tube #2, and stir (hold the button for automatic stirring) for at least 5 seconds. Put the rod on the counter.\n\nThen pick up the pen and record what you see in the lab notebook.\n\n[placeholder step - press A to continue]",
                    /*9*/"Step 10 of 11: Waste disposal\nStation: 3 HOOD, 4 SINK\nLabware: both test tubes, chemical waste container, glass waste bin\n\nWASTE REMINDER: dispose of waste in the designated waste containers. Metal ion waste should NEVER be poured down the drain. You are not yet qualified to distinguish when acids and bases may be poured down the drain, so always dispose of acid and base waste in the designated containers.\n\nMetal ion waste goes in the fume hood. The test tubes are thrown out in glass waste.\n\n[placeholder step - press A to continue]",
                    /*10*/"Step 11 of 11: Wrap-up: what happened\n\nMixing chromate ions (CrO4 2-) with sodium hydroxide keeps the chromium in its yellow chromate form. NaOH is a base: it adds hydroxide ions (OH-), which remove free hydrogen ions (H+). That prevents the solution from becoming acidic, which would turn yellow chromate into orange dichromate (Cr2O7 2-). The equilibrium shifts toward chromate. No precipitate forms; the solution stays a clear yellow liquid.\n\nSAFETY: sodium hydroxide is a strong alkali and causes severe skin burns. Chromate contains hexavalent chromium, which is toxic and harmful to the environment. Always wear gloves and eye protection.\n\n[placeholder step - press A to continue]",
                    /*11*/"This concludes the Physical and Chemical Changes module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
            };

    // WebGL wording (keyboard / mouse)
    string[] webGLText = {
                    /*0*/"Step 1 of 11: Background\nStation: 1 PPE\n\nIn this experiment, 9 reactions at equilibrium will be perturbed, and the resulting chemical and physical changes will be observed. A physical change occurs when matter changes form. A chemical change occurs when new products are formed. In this simulation you will do reaction #1 from Lab 3: The Law of Mass Action: Le Chatelier's Principle.\n\nLe Chatelier's Principle allows us to predict the effect of change upon a chemical system in equilibrium. If stress is imposed on a system at equilibrium, the equilibrium concentrations of reactants and products shift in such a manner as to relieve the stress.\n\n[placeholder step - press N to continue]",
                    /*1*/"Step 2 of 11: Safety first: put on PPE\nStation: 1 PPE\nLabware: goggles, lab coat, gloves\n\nBefore starting any chemistry experiment, you must wear proper Personal Protective Equipment (PPE). Put on your safety goggles, lab coat and gloves (your hands turn blue).\n\nSAFETY PRECAUTION: several reactions in this experiment use acids and bases at 1 M or stronger. Be extremely careful with corrosive acids and bases. Clean up all spills or drips immediately as directed by your TA, and do not leave drops of acid or base on the sides of the bottles.\n\nGoggles protect your eyes from splashes; lab coats protect your skin and clothing. In this lab you will generate metal ion waste, disposed of in the fume hood, and glass waste (the test tubes).\n\n[placeholder step - press N to continue]",
                    /*2*/"Step 3 of 11: Gather equipment\nStation: 5 TUBE, 2 MIXING, 3 HOOD\nLabware: 2 test tubes, test tube rack, glass stir rod, squirt bottle of water, disposable pipette, Sharpie, potassium chromate (K2CrO4), sodium hydroxide (NaOH), sulfuric acid (H2SO4), notebook + pen\n\nThis is a qualitative experiment: measurements do not have to be exact, detailed observations matter most. You will mix approximately 1 mL solutions.\n\nGather two test tubes, the test tube rack and the glass stir rod, plus the squirt bottle of water and a disposable pipette on the bench. Place both test tubes in the rack. Use the pipette to measure about 1 mL of water into one test tube to estimate the amounts you will add later. Label one test tube #1 and the other #2 with the Sharpie.\n\nThen find potassium chromate, sodium hydroxide and sulfuric acid in the fume hood. They are in two trays labeled acids and bases, and each bottle has a disposable pipette taped to it. Bring all three bottles to your work area.\n\n[placeholder step - press N to continue]",
                    /*3*/"Step 4 of 11: Measure 1 mL of 0.1 M potassium chromate\nStation: 2 MIXING\nLabware: graduated cylinder, potassium chromate bottle, test tubes #1 and #2\n\nUse the graduated cylinder to collect 1 mL of 0.1 M potassium chromate solution. Hold the cylinder under the reagent bottle and pour; the flow stops automatically at 1 mL.\n\nPour the yellow solution into test tube #1, then repeat for test tube #2.\n\n[placeholder step - press N to continue]",
                    /*4*/"Step 5 of 11: Add 1 M sodium hydroxide to test tube #1\nStation: 2 MIXING\nLabware: 1 M sodium hydroxide bottle + dropper, test tube #1\n\nOpen the reagent bottle that says 1 M sodium hydroxide and use the dropper.\n\nPick up the bottle and its dropper, put the bottle down, and hold the dropper over test tube #1. Push the button to add drops; the system stops automatically after 4 drops.\n\n[placeholder step - press N to continue]",
                    /*5*/"Step 6 of 11: Stir the solution\nStation: 2 MIXING\nLabware: stir rod, test tube #1\n\nUse the glass stir rod to mix the solution inside the test tube until the drops completely dissolve.\n\nPick up the stir rod, place it in test tube #1, and stir (hold the button for automatic stirring) for at least 5 seconds. Then place the rod on the counter.\n\n[placeholder step - press N to continue]",
                    /*6*/"Step 7 of 11: Record your observations\nStation: 2 MIXING\nLabware: lab notebook, pen, test tube #1\n\nIs this a physical change or a chemical change? Select the statement that matches what you see in test tube #1. If it is not right, think about whether a new substance was created and try again.\n\nThen pick up the pen and record your observations in the lab notebook.\n\n[placeholder step - press N to continue]",
                    /*7*/"Step 8 of 11: Add 3 M sulfuric acid to test tube #2\nStation: 2 MIXING\nLabware: 3 M sulfuric acid bottle + dropper, test tube #2, stir rod, notebook + pen\n\nOpen the reagent bottle that says 3 M sulfuric acid and use the dropper.\n\nPick up the bottle and its dropper, put the bottle down, and hold the dropper over test tube #2. Push the button to add drops; the system stops automatically after 4 drops.\n\nPick up the stir rod, place it in test tube #2, and stir (hold the button for automatic stirring) for at least 5 seconds. Put the rod on the counter.\n\nThen pick up the pen and record what you see in the lab notebook.\n\n[placeholder step - press N to continue]",
                    /*8*/"Step 9 of 11: Add 1 M sodium hydroxide to test tube #2\nStation: 2 MIXING\nLabware: 1 M sodium hydroxide bottle + dropper, test tube #2, stir rod, notebook + pen\n\nOpen the reagent bottle that says 1 M sodium hydroxide and use the dropper.\n\nPick up the bottle and its dropper, put the bottle down, and hold the dropper over test tube #2. Push the button to add drops; the system stops automatically after 4 drops.\n\nPick up the stir rod, place it in test tube #2, and stir (hold the button for automatic stirring) for at least 5 seconds. Put the rod on the counter.\n\nThen pick up the pen and record what you see in the lab notebook.\n\n[placeholder step - press N to continue]",
                    /*9*/"Step 10 of 11: Waste disposal\nStation: 3 HOOD, 4 SINK\nLabware: both test tubes, chemical waste container, glass waste bin\n\nWASTE REMINDER: dispose of waste in the designated waste containers. Metal ion waste should NEVER be poured down the drain. You are not yet qualified to distinguish when acids and bases may be poured down the drain, so always dispose of acid and base waste in the designated containers.\n\nMetal ion waste goes in the fume hood. The test tubes are thrown out in glass waste.\n\n[placeholder step - press N to continue]",
                    /*10*/"Step 11 of 11: Wrap-up: what happened\n\nMixing chromate ions (CrO4 2-) with sodium hydroxide keeps the chromium in its yellow chromate form. NaOH is a base: it adds hydroxide ions (OH-), which remove free hydrogen ions (H+). That prevents the solution from becoming acidic, which would turn yellow chromate into orange dichromate (Cr2O7 2-). The equilibrium shifts toward chromate. No precipitate forms; the solution stays a clear yellow liquid.\n\nSAFETY: sodium hydroxide is a strong alkali and causes severe skin burns. Chromate contains hexavalent chromium, which is toxic and harmful to the environment. Always wear gloves and eye protection.\n\n[placeholder step - press N to continue]",
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
