using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial_Overview : MonoBehaviour
{
    public TextMeshProUGUI tutText;
    int curStep;

    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = -1;
        tutText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            tutText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAdvanceTask += AdvanceTutTask;
        GameEventsManager.instance.taskEvents.onAbandonTask += tu_abandonMe;
    }

    void OnDisable()
    {
        //Remove any listeners
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= AdvanceTutTask;
        GameEventsManager.instance.taskEvents.onAbandonTask -= tu_abandonMe;
    }

    public void restart()
    {
        tu_abandonMe("Tutorial_Overview");
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
    }
    public void tu_abandonMe(string context)
    {
        if (context.Contains("Tutorial"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
            GameObject tutPop = GameObject.Find("Tutorial PopUp");
            tutText.text = "Don't look at me I'm inactive.";
            if (tutPop != null)
            {
                tutPop.SetActive(false);
                curStep = -1;
            }
        }
    }
    string[] text = {"Welcome to the tutorial!\n\nYou can hide this popup with the primary button on your left controller, (X).\n\nTry hiding this popup and re-opening it by pressing (X) twice!",
                    "Good Job!\n\nYou can use the analog sticks to look and move around.\n\nApproach the table with beakers on it, it is inside the blue square on the floor.",
                    "Tutorial complete!\n\nOne more thing!\nPress the secondary button (Y) on your left controller anytime to exit the module and return to the menu.\n\nGive it a try!"
                    };
    void AdvanceTutTask(string context)
    {
        if (context.Contains("Tutorial_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            tutText.text = text[curStep];
        }
    }
}
