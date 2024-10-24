using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glove_Hygiene_Overview : MonoBehaviour
{
    public HygieneManager touchTracker;
    public TextMeshProUGUI ghText;
    int curStep;

    // Controllers for starting tasks
    [Header("Start Task Controllers")]
    public Start_Module taskPreper;

    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = 0;
        ghText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            ghText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAbandonTask += gh_abandonMe;
        GameEventsManager.instance.taskEvents.onAdvanceTask += AdvanceGloTask;

        //Start the Module on opening this scene
        taskPreper.Show();
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
        GameEventsManager.instance.taskEvents.StartTask("Glove_Hygiene_Task");
        ghText.text = text[curStep];
    }

    void OnDisable()
    {
        //Remove any listeners
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= AdvanceGloTask;
        GameEventsManager.instance.taskEvents.onAbandonTask -= gh_abandonMe;
    }
    public void restart()
    {
        gh_abandonMe("Glove_Hygiene_Overview");
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
    }

    string[] text = {"Before beginning the lab, find gloves, goggles, and a lab coat and put them on with (A/X).\n\nRemove gloves anytime by holding your hands over the trashcan near the office door and pressing (A/X).\n\nRemember to take them off when you aren't working on the expirement!",
                    //"For this lab, bring the following to a table in the red zone / C zone:\n\nBeaker\nErlenmeyer flask with pink chemical\nErlenmeyer flask with blue chemical", //disabled task step 2 (add to TaskInfoSO block and uncomment to undo)
                    //"You will need these items as well:\n\nBurret\nFunnel", //disabled task step 2_5 (add to TaskInfoSO block and uncomment to undo)
                    "You're getting a call! It might be urgent.\n\nYour phone is in the office on your right, pick it up and press (A/X) to answer it.",
                    "Phew, that was close.\n\nNow return to the experiment in the red zone.",
                    "You need to set up a burette.\n\nAttach a funnel and a burette to the holder at your table.",
                    "Next, fill the beaker one quarter way with the Erlenmeyer flask with the pink chemical.", //After 2 drops, interrupt with next one!
                    "It's time for a break.\n\nI think I saw some coffee in your office.\n\nPick it up and press (A/X) to take a drink.",
                    "Breaks over!\n\nFinish filling the beaker the beaker one quarter way with the Erlenmeyer flask with the pink chemical.",
                    "Now fill the burette halfway from the Erlenmeyer flask with the blue chemical.\n\nTip: make sure the burette turner is closed first!",
                    "Titrate from the burette into the beaker until you see a change of color in the beaker, or until you run out of blue chemical.\n\nWhen you're done, bring the beaker the the table with the notepad.",
                    "Uh Oh!\n\nThe printer is acting up again, and it's printing important data.\n\nSmack it once or twice with your hand to get it running right.", //Interrupt once you touch the turner
                    "Return to the red zone and titrate until you see a change of color in the beaker, or until you run out of blue chemical.\n\nWhen you're done, bring the beaker the the table with the notepad.",
                    "Record your findings using the pencil and data sheet on the tables near the office. Touch the pencil to the paper to do so.",
                    "The titration experiment is now complete. Please go to the trash to remove your gloves.\n\nHold your hands over or in the trash and press A/X",
                    "This completes the glove hygiene module. We will now display all of the chemical spills created throughout the experience.\n\n(Y Button) Hides Popup\n(B Button) Opens Menu"
                    };
    void AdvanceGloTask(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            if (curStep == 0)
            {
                touchTracker.Restart();
            } else if (curStep == text.Length - 1)
            {
                touchTracker.ShowPoints(true);
            }

            ghText.text = text[curStep];
        }
    }

    public void gh_abandonMe(string context)
    {
        if (context.Contains("Glove"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
            GameObject ghPop = GameObject.Find("Glove Hygiene PopUp");
            ghText.text = "Don't look at me I'm inactive.";
            if (ghPop != null)
            {
                ghPop.SetActive(false);
            }
            curStep = 0;
        }
    }
}
