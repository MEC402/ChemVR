using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glove_Hygiene_Overview : MonoBehaviour
{
    public TextMeshProUGUI ghText;
    int curStep;

    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = -1;
        ghText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            ghText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAbandonTask += gh_abandonMe;
        GameEventsManager.instance.taskEvents.onAdvanceTask += AdvanceGloTask;
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

    string[] text = {"Welcome!\nBefore we begin the lab, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind gloves, goggles, and a lab coat and put them on with (A).",
                    "For this lab, bring the following to a table in the red zone:\nBeaker\nErlenmeyer flask with white chemical\nErlenmeyer flask w/ red chemical.\n\n\nSkip with A",
                    "You're getting a call! It might be urgent.\n\nYour phone is in the office, pick it up and press (B) to answer it.\n\n\nSkip with A",
                    "Phew, that was close.\n\nNow return to the experiment in the red zone.\n\n\nSkip with A",
                    "You need to set up a burette.\n\nAttach a funnel and a burette to the holder at your table.\n\n\nSkip with A",
                    "Next, fill the beaker with 5 drop from the Erlenmeyer flask with the white chemical.\n\n\nSkip with A",
                    "It's time for a break.\n\nI think I saw some coffee in your office. Pick it up and press (B) to take a drink.\n\n\nSkip with A",
                    "Breaks over!\n\nFinish filling the beaker with 5 drop from the Erlenmeyer flask with the white chemical.\n\n\nSkip with A",
                    "Now fill the burette with 10 drops from the Erlenmeyer flask with the red chemical.\n\n\nSkip with A",
                    "Titrate until you see a change of color in the beaker, or until you run out of red chemical. When you're done, bring the beaker into the yellow zone.\n\n\nSkip with A",
                    "Uh Oh!\n\nThe printer is acting up again, and it's printing important data. Smack it once or twice with your hand to get it running right.\n\n\nSkip with A",
                    "Return to the red zone and titrate until you see a change of color in the beaker, or until you run out of red chemical. When you're done, bring the beaker into the yellow zone.\n\n\nSkip with A",
                    "Record your findings using the pencil and data sheet on the tables near the office. Touch the pencil to the paper to do so.\n\n\nSkip with A",
                    "The titration experiment is now complete. Please go to the trash to remove your gloves.\n\n\nSkip with A",
                    "This completes the glove hygiene module. We will now display all of the chemical spills created throughout the experience.\n(X) Hides Popup\n(Y) Opens Menu"
                    };
    void AdvanceGloTask(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
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
                curStep = -1;
            }
        }
    }
}
