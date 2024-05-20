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
        curStep = 0;
        ghText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            ghText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task0Complete;
        GameEventsManager.instance.taskEvents.onAbandonTask += gh_abandonMe;
    }

    void OnDisable()
    {
        //Remove any listeners
        if (curStep == 1)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task1Complete;
        }
    }

    public void gh_abandonMe(string context)
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameObject ghPop = GameObject.Find("Glove Hygiene PopUp");
        ghText.text = "Don't look at me I'm inactive.";
        if (ghPop != null)
        {
            ghPop.SetActive(false);
            if (curStep == 1)
            {
                GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task1Complete;
            }
            curStep = 0;
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task0Complete;
        }
    }

    void gh_task0Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            ghText.text = "This is the Chemical Change PopUp";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task1Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task0Complete;
        }
    }

    void gh_task1Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Good Job!\n\nNow approach the table with beakers on it.\n\nNotice that it is inside the blue square on the floor.";
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task1Complete;
        }
    }
}
