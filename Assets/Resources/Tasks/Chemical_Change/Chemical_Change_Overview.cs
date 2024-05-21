using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chemical_Change_Overview : MonoBehaviour
{
    public TextMeshProUGUI ccText;
    int curStep;

    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = 0;
        ccText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            ccText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAdvanceTask += cc_task0Complete;
        GameEventsManager.instance.taskEvents.onAbandonTask += cc_abandonMe;
    }

    void OnDisable()
    {
        //Remove any listeners
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        disableAll();
    }

    void disableAll()
    {
        if (curStep == 0)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= cc_task0Complete;
        }
        else if(curStep == 1)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= cc_task1Complete;
        }
    }

    public void cc_abandonMe(string context)
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameObject ccPop = GameObject.Find("Chemical Change PopUp");
        ccText.text = "Don't look at me I'm inactive.";
        if (ccPop != null)
        {
            ccPop.SetActive(false);
            disableAll();
            curStep = 0;
            GameEventsManager.instance.taskEvents.onAdvanceTask += cc_task0Complete;
        }
    }

    void cc_task0Complete(string context)
    {
        if (context.Contains("Chemical_Change_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            ccText.text = "Welcome!\nBefore we begin the lab, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind gloves, goggles, and a lab coat and put them on with (B).";
            GameEventsManager.instance.taskEvents.onAdvanceTask += cc_task1Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= cc_task0Complete;
        }
    }

    void cc_task1Complete(string context)
    {
        if (context.Contains("Chemical_Change_Task"))
        {
            curStep += 1;
            ccText.text = "Good Job!\n\nNow approach the table with beakers on it.\n\nNotice that it is inside the blue square on the floor.";
            GameEventsManager.instance.taskEvents.onAdvanceTask -= cc_task1Complete;
        }
    }
}
