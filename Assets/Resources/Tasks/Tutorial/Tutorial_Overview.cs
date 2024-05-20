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
        curStep = 0;
        tutText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            tutText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAdvanceTask += tu_task0Complete;
        GameEventsManager.instance.taskEvents.onAbandonTask += tu_abandonMe;
    }

    void OnDisable()
    {
        //Remove any listeners
        if (curStep == 0)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= tu_task0Complete;
        }
        else if (curStep == 1)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= tu_task1Complete;
        }
        else if (curStep == 2)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= tu_task2Complete;
        }
    }

    public void tu_abandonMe(string context)
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameObject tutPop = GameObject.Find("Tutorial PopUp");
        tutText.text = "Don't look at me I'm inactive.";
        if (tutPop != null)
        {
            tutPop.SetActive(false);
            if (curStep == 1)
            {
                GameEventsManager.instance.taskEvents.onAdvanceTask -= tu_task1Complete;
            }
            else if (curStep == 2)
            {
                GameEventsManager.instance.taskEvents.onAdvanceTask -= tu_task2Complete;
            }
            curStep = 0;
            GameEventsManager.instance.taskEvents.onAdvanceTask += tu_task0Complete;
        }
    }

    void tu_task0Complete(string context)
    {
        if (context.Contains("Tutorial_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            tutText.text = "Welcome to the tutorial!\n\nYou can hide this popup with the primary button on your left controller, (X).\n\nTry hiding this popup and re-opening it by pressing (X) twice!";
            GameEventsManager.instance.taskEvents.onAdvanceTask += tu_task1Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= tu_task0Complete;
        }
    }
    void tu_task1Complete(string context)
    {
        if (context.Contains("Tutorial_Task"))
        {
            curStep += 1;
            tutText.text = "Good Job!\n\nNow approach the table with beakers on it.\n\nNotice that it is inside the blue square on the floor.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += tu_task2Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= tu_task1Complete;
        }
    }
    void tu_task2Complete(string context)
    {
        if (context.Contains("Tutorial_Task"))
        {
            curStep += 1;
            tutText.text = "Tutorial complete!\n\nOne more thing!\nPress the secondary button (Y) on your left controller anytime to exit the module and return to the menu.\n\nGive it a try!";
            //tutText.text = "Great!\n\nYou can pick up objects using the main triggers on either hand.\n\nNow that you're here, give it a try.";
            //GameEventsManager.instance.taskEvents.onAdvanceTask += task3Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= tu_task2Complete;
        }
    }
}
