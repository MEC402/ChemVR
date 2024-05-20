using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glassware_Use_Overview : MonoBehaviour
{
    public TextMeshProUGUI guText;
    int curStep;

    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = 0;
        guText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            guText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task0Complete;
        GameEventsManager.instance.taskEvents.onAbandonTask += gu_abandonMe;
    }

    void OnDisable()
    {
        //Remove any listeners
        if (curStep == 1)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task1Complete;
        }
    }

    public void gu_abandonMe(string context)
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameObject guPop = GameObject.Find("Glassware Use PopUp");
        guText.text = "Don't look at me I'm inactive.";
        if (guPop != null)
        {
            guPop.SetActive(false);
            if (curStep == 1)
            {
                GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task1Complete;
            }
            curStep = 0;
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task0Complete;
        }
    }

    void gu_task0Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            guText.text = "This is the Glassware Use PopUp";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task1Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task0Complete;
        }
    }

    void gu_task1Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Good Job!\n\nNow approach the table with beakers on it.\n\nNotice that it is inside the blue square on the floor.";
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task1Complete;
        }
    }
}
