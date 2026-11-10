using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Instruction popup for the Physical & Chemical Changes module (Lab 3, Reaction #1:
// the chromate-dichromate equilibrium). Popup text follows the module document
// (docs/VR Chemistry Module 3 - Physical and Chemical Changes UPDATED.docx), see docs/chemmodule_tasks.md.
// Each popup is ONE task, one per required action in the document (see docs/chemmodule_tasks.md).
// Every task is currently a click-through placeholder (Module3_ClickThroughStep);
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
                    /*0*/"Task 1 of 33: Background: reaction #1\nStation: 1 PPE\n\nIn this experiment, 9 reactions at equilibrium will be perturbed, and the resulting chemical and physical changes will be observed. A physical change occurs when matter changes form. A chemical change occurs when new products are formed. For this simulation, you will do reaction #1 from Lab 3: The Law of Mass Action: Le Chatelier's Principle.\n\n[placeholder task - press A to continue]",
                    /*1*/"Task 2 of 33: Background: Le Chatelier's Principle\nStation: 1 PPE\n\nLe Chatelier's Principle allows us to predict the effect of change upon a chemical system in equilibrium. It says that if stress is imposed on a system at equilibrium, the equilibrium concentrations of reactants and products will shift in such a manner as to relieve the stress.\n\n[placeholder task - press A to continue]",
                    /*2*/"Task 3 of 33: Safety precaution\nStation: 1 PPE\n\nSAFETY PRECAUTION: Several of the reactions in this experiment use acids and bases at concentrations of 1 M or more. Be extremely careful when using high concentrations of corrosive chemicals such as acids and bases. All spills or drips should be cleaned up immediately as directed by your TA. Be particularly careful not to leave drops of acid or base on the sides of the bottles.\n\n[placeholder task - press A to continue]",
                    /*3*/"Task 4 of 33: Put on goggles and lab coat\nStation: 1 PPE\n\nBefore starting any chemistry experiment, you must wear proper Personal Protective Equipment (PPE). At the PPE station, press the goggles button and then the lab coat button. Each item equips automatically.\n\n[placeholder task - press A to continue]",
                    /*4*/"Task 5 of 33: Put on gloves\nStation: 1 PPE\n\nPut on your gloves. Your hands turn blue to show that you are wearing gloves.\n\n[placeholder task - press A to continue]",
                    /*5*/"Task 6 of 33: Before you start: lab notes\nStation: 1 PPE\n\nGoggles protect your eyes from splashes; lab coats protect your skin and clothing. In this lab you will generate metal ion waste, disposed of in the fume hood, and glass waste (the test tubes). This is a qualitative experiment: measurements do not have to be exact, detailed observations of color, precipitation and temperature changes are what matter most.\n\n[placeholder task - press A to continue]",
                    /*6*/"Task 7 of 33: Set up the test tubes\nStation: 5 TUBE\n\nGather your equipment. At the TUBE station, put the test tube rack in your work area, place two test tubes inside it, and pick up the glass stir rod.\n\n[placeholder task - press A to continue]",
                    /*7*/"Task 8 of 33: Estimate 1 mL\nStation: 2 MIXING\n\nThis lab uses approximately 1 mL volumes, which do not have to be exact. Take the squirt bottle of water and a disposable pipette from the bench and measure about 1 mL of water into one test tube to learn the amount. Also place the 10 mL graduated cylinder in your work area.\n\n[placeholder task - press A to continue]",
                    /*8*/"Task 9 of 33: Label the test tubes\nStation: 2 MIXING\n\nLabel one empty test tube #1 and the other #2 with the black Sharpie.\n\n[placeholder task - press A to continue]",
                    /*9*/"Task 10 of 33: Collect the reagents\nStation: 3 HOOD\n\nGo to the fume hood. The reagents are stored in two trays labeled acids and bases, and each bottle has a disposable pipette taped to it. Bring the potassium chromate (K2CrO4), sodium hydroxide (NaOH) and sulfuric acid (H2SO4) bottles to your work area.\n\n[placeholder task - press A to continue]",
                    /*10*/"Task 11 of 33: Measure 1 mL of chromate\nStation: 2 MIXING\n\nPick up the graduated cylinder and hold it under the potassium chromate reagent bottle. Pour: the yellow solution stops flowing automatically at exactly 1 mL.\n\n[placeholder task - press A to continue]",
                    /*11*/"Task 12 of 33: Pour into test tube #1\nStation: 2 MIXING\n\nPour the yellow potassium chromate from the cylinder into test tube #1.\n\n[placeholder task - press A to continue]",
                    /*12*/"Task 13 of 33: Repeat for test tube #2\nStation: 2 MIXING\n\nMeasure another 1 mL of potassium chromate in the cylinder and pour it into test tube #2.\n\n[placeholder task - press A to continue]",
                    /*13*/"Task 14 of 33: NaOH #1: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.\n\n[placeholder task - press A to continue]",
                    /*14*/"Task 15 of 33: NaOH #1: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #1 and push the button to add drops, drop by drop. The system stops automatically after four drops.\n\n[placeholder task - press A to continue]",
                    /*15*/"Task 16 of 33: NaOH #1: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #1 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.\n\n[placeholder task - press A to continue]",
                    /*16*/"Task 17 of 33: NaOH #1: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.\n\n[placeholder task - press A to continue]",
                    /*17*/"Task 18 of 33: NaOH #1: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #1, then pick up the pen and write it in the lab notebook.\n\n[placeholder task - press A to continue]",
                    /*18*/"Task 19 of 33: H2SO4 #2: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 3 M sulfuric acid: pick it up in one hand and its dropper in the other, then put the bottle down.\n\n[placeholder task - press A to continue]",
                    /*19*/"Task 20 of 33: H2SO4 #2: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.\n\n[placeholder task - press A to continue]",
                    /*20*/"Task 21 of 33: H2SO4 #2: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.\n\n[placeholder task - press A to continue]",
                    /*21*/"Task 22 of 33: H2SO4 #2: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.\n\n[placeholder task - press A to continue]",
                    /*22*/"Task 23 of 33: H2SO4 #2: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.\n\n[placeholder task - press A to continue]",
                    /*23*/"Task 24 of 33: NaOH #2: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.\n\n[placeholder task - press A to continue]",
                    /*24*/"Task 25 of 33: NaOH #2: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.\n\n[placeholder task - press A to continue]",
                    /*25*/"Task 26 of 33: NaOH #2: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.\n\n[placeholder task - press A to continue]",
                    /*26*/"Task 27 of 33: NaOH #2: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.\n\n[placeholder task - press A to continue]",
                    /*27*/"Task 28 of 33: NaOH #2: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.\n\n[placeholder task - press A to continue]",
                    /*28*/"Task 29 of 33: Waste reminder\nStation: 3 HOOD\n\nWASTE REMINDER: Dispose of waste in the designated waste containers. Metal ion waste should NEVER be poured down the drain. You are not yet qualified to distinguish when acids and bases may be poured down the drain, so you should always dispose of acid and base waste in the designated containers.\n\n[placeholder task - press A to continue]",
                    /*29*/"Task 30 of 33: Empty the test tubes\nStation: 3 HOOD\n\nTake test tubes #1 and #2 to the fume hood and empty both into the chemical waste container.\n\n[placeholder task - press A to continue]",
                    /*30*/"Task 31 of 33: Test tubes to glass waste\nStation: 4 SINK\n\nPut both empty test tubes in the glass waste.\n\n[placeholder task - press A to continue]",
                    /*31*/"Task 32 of 33: Answer: what happened\nStation: 2 MIXING\n\nMixing chromate ions (CrO4 2-) with sodium hydroxide (NaOH) keeps the chromium in its yellow chromate form. Because NaOH is a base, it prevents the solution from becoming acidic, which stops yellow chromate from turning into orange dichromate (Cr2O7 2-). NaOH adds hydroxide ions (OH-), which remove free hydrogen ions (H+), so the equilibrium shifts toward chromate. No precipitate forms; the solution stays a clear yellow liquid.\n\n[placeholder task - press A to continue]",
                    /*32*/"Task 33 of 33: Safety and handling\nStation: 2 MIXING\n\nSodium hydroxide is a strong alkali and causes severe skin burns. Chromate compounds contain hexavalent chromium, which is toxic and harmful to the environment. Always wear gloves and eye protection.\n\n[placeholder task - press A to continue]",
                    /*33*/"This concludes the Physical and Chemical Changes module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
            };

    // WebGL wording (keyboard / mouse)
    string[] webGLText = {
                    /*0*/"Task 1 of 33: Background: reaction #1\nStation: 1 PPE\n\nIn this experiment, 9 reactions at equilibrium will be perturbed, and the resulting chemical and physical changes will be observed. A physical change occurs when matter changes form. A chemical change occurs when new products are formed. For this simulation, you will do reaction #1 from Lab 3: The Law of Mass Action: Le Chatelier's Principle.\n\n[placeholder task - press N to continue]",
                    /*1*/"Task 2 of 33: Background: Le Chatelier's Principle\nStation: 1 PPE\n\nLe Chatelier's Principle allows us to predict the effect of change upon a chemical system in equilibrium. It says that if stress is imposed on a system at equilibrium, the equilibrium concentrations of reactants and products will shift in such a manner as to relieve the stress.\n\n[placeholder task - press N to continue]",
                    /*2*/"Task 3 of 33: Safety precaution\nStation: 1 PPE\n\nSAFETY PRECAUTION: Several of the reactions in this experiment use acids and bases at concentrations of 1 M or more. Be extremely careful when using high concentrations of corrosive chemicals such as acids and bases. All spills or drips should be cleaned up immediately as directed by your TA. Be particularly careful not to leave drops of acid or base on the sides of the bottles.\n\n[placeholder task - press N to continue]",
                    /*3*/"Task 4 of 33: Put on goggles and lab coat\nStation: 1 PPE\n\nBefore starting any chemistry experiment, you must wear proper Personal Protective Equipment (PPE). At the PPE station, press the goggles button and then the lab coat button. Each item equips automatically.\n\n[placeholder task - press N to continue]",
                    /*4*/"Task 5 of 33: Put on gloves\nStation: 1 PPE\n\nPut on your gloves. Your hands turn blue to show that you are wearing gloves.\n\n[placeholder task - press N to continue]",
                    /*5*/"Task 6 of 33: Before you start: lab notes\nStation: 1 PPE\n\nGoggles protect your eyes from splashes; lab coats protect your skin and clothing. In this lab you will generate metal ion waste, disposed of in the fume hood, and glass waste (the test tubes). This is a qualitative experiment: measurements do not have to be exact, detailed observations of color, precipitation and temperature changes are what matter most.\n\n[placeholder task - press N to continue]",
                    /*6*/"Task 7 of 33: Set up the test tubes\nStation: 5 TUBE\n\nGather your equipment. At the TUBE station, put the test tube rack in your work area, place two test tubes inside it, and pick up the glass stir rod.\n\n[placeholder task - press N to continue]",
                    /*7*/"Task 8 of 33: Estimate 1 mL\nStation: 2 MIXING\n\nThis lab uses approximately 1 mL volumes, which do not have to be exact. Take the squirt bottle of water and a disposable pipette from the bench and measure about 1 mL of water into one test tube to learn the amount. Also place the 10 mL graduated cylinder in your work area.\n\n[placeholder task - press N to continue]",
                    /*8*/"Task 9 of 33: Label the test tubes\nStation: 2 MIXING\n\nLabel one empty test tube #1 and the other #2 with the black Sharpie.\n\n[placeholder task - press N to continue]",
                    /*9*/"Task 10 of 33: Collect the reagents\nStation: 3 HOOD\n\nGo to the fume hood. The reagents are stored in two trays labeled acids and bases, and each bottle has a disposable pipette taped to it. Bring the potassium chromate (K2CrO4), sodium hydroxide (NaOH) and sulfuric acid (H2SO4) bottles to your work area.\n\n[placeholder task - press N to continue]",
                    /*10*/"Task 11 of 33: Measure 1 mL of chromate\nStation: 2 MIXING\n\nPick up the graduated cylinder and hold it under the potassium chromate reagent bottle. Pour: the yellow solution stops flowing automatically at exactly 1 mL.\n\n[placeholder task - press N to continue]",
                    /*11*/"Task 12 of 33: Pour into test tube #1\nStation: 2 MIXING\n\nPour the yellow potassium chromate from the cylinder into test tube #1.\n\n[placeholder task - press N to continue]",
                    /*12*/"Task 13 of 33: Repeat for test tube #2\nStation: 2 MIXING\n\nMeasure another 1 mL of potassium chromate in the cylinder and pour it into test tube #2.\n\n[placeholder task - press N to continue]",
                    /*13*/"Task 14 of 33: NaOH #1: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.\n\n[placeholder task - press N to continue]",
                    /*14*/"Task 15 of 33: NaOH #1: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #1 and push the button to add drops, drop by drop. The system stops automatically after four drops.\n\n[placeholder task - press N to continue]",
                    /*15*/"Task 16 of 33: NaOH #1: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #1 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.\n\n[placeholder task - press N to continue]",
                    /*16*/"Task 17 of 33: NaOH #1: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.\n\n[placeholder task - press N to continue]",
                    /*17*/"Task 18 of 33: NaOH #1: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #1, then pick up the pen and write it in the lab notebook.\n\n[placeholder task - press N to continue]",
                    /*18*/"Task 19 of 33: H2SO4 #2: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 3 M sulfuric acid: pick it up in one hand and its dropper in the other, then put the bottle down.\n\n[placeholder task - press N to continue]",
                    /*19*/"Task 20 of 33: H2SO4 #2: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.\n\n[placeholder task - press N to continue]",
                    /*20*/"Task 21 of 33: H2SO4 #2: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.\n\n[placeholder task - press N to continue]",
                    /*21*/"Task 22 of 33: H2SO4 #2: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.\n\n[placeholder task - press N to continue]",
                    /*22*/"Task 23 of 33: H2SO4 #2: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.\n\n[placeholder task - press N to continue]",
                    /*23*/"Task 24 of 33: NaOH #2: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.\n\n[placeholder task - press N to continue]",
                    /*24*/"Task 25 of 33: NaOH #2: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.\n\n[placeholder task - press N to continue]",
                    /*25*/"Task 26 of 33: NaOH #2: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.\n\n[placeholder task - press N to continue]",
                    /*26*/"Task 27 of 33: NaOH #2: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.\n\n[placeholder task - press N to continue]",
                    /*27*/"Task 28 of 33: NaOH #2: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.\n\n[placeholder task - press N to continue]",
                    /*28*/"Task 29 of 33: Waste reminder\nStation: 3 HOOD\n\nWASTE REMINDER: Dispose of waste in the designated waste containers. Metal ion waste should NEVER be poured down the drain. You are not yet qualified to distinguish when acids and bases may be poured down the drain, so you should always dispose of acid and base waste in the designated containers.\n\n[placeholder task - press N to continue]",
                    /*29*/"Task 30 of 33: Empty the test tubes\nStation: 3 HOOD\n\nTake test tubes #1 and #2 to the fume hood and empty both into the chemical waste container.\n\n[placeholder task - press N to continue]",
                    /*30*/"Task 31 of 33: Test tubes to glass waste\nStation: 4 SINK\n\nPut both empty test tubes in the glass waste.\n\n[placeholder task - press N to continue]",
                    /*31*/"Task 32 of 33: Answer: what happened\nStation: 2 MIXING\n\nMixing chromate ions (CrO4 2-) with sodium hydroxide (NaOH) keeps the chromium in its yellow chromate form. Because NaOH is a base, it prevents the solution from becoming acidic, which stops yellow chromate from turning into orange dichromate (Cr2O7 2-). NaOH adds hydroxide ions (OH-), which remove free hydrogen ions (H+), so the equilibrium shifts toward chromate. No precipitate forms; the solution stays a clear yellow liquid.\n\n[placeholder task - press N to continue]",
                    /*32*/"Task 33 of 33: Safety and handling\nStation: 2 MIXING\n\nSodium hydroxide is a strong alkali and causes severe skin burns. Chromate compounds contain hexavalent chromium, which is toxic and harmful to the environment. Always wear gloves and eye protection.\n\n[placeholder task - press N to continue]",
                    /*33*/"This concludes the Physical and Chemical Changes module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
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
