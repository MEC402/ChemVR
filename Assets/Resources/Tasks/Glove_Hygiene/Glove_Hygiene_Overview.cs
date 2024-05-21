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
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        disableAll();
    }

    void disableAll()
    {
        if (curStep == 0)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task0Complete;
        }
        else if (curStep == 1)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task1Complete;
        }
        else if (curStep == 2)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task2Complete;
        }
        else if (curStep == 3)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task3Complete;
        }
        else if (curStep == 4)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task4Complete;
        }
        else if (curStep == 5)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task5Complete;
        }
        else if (curStep == 6)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task6Complete;
        }
        else if (curStep == 7)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task7Complete;
        }
        else if (curStep == 8)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task8Complete;
        }
        else if (curStep == 9)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task9Complete;
        }
        else if (curStep == 10)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task10Complete;
        }
        else if (curStep == 11)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task11Complete;
        }
        else if (curStep == 12)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task12Complete;
        }
        else if (curStep == 13)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task13Complete;
        }
        else if (curStep == 14)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task14Complete;
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
            disableAll();
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
            ghText.text = "Welcome!\nBefore we begin the lab, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind gloves, goggles, and a lab coat and put them on with (B).";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task1Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task0Complete;
        }
    }

    void gh_task1Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "For this lab, bring the following to a table in the red zone:\nBeaker\nErlenmeyer flask with white chemical\nErlenmeyer flask w/ red chemical.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task2Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task1Complete;
        }
    }

    void gh_task2Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "You're getting a call! It might be urgent.\n\nYour phone is in the office, pick it up and press (B) to answer it.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task3Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task2Complete;
        }
    }

    void gh_task3Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Phew, that was close.\n\nNow return to the experiment in the red zone.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task4Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task3Complete;
        }
    }

    void gh_task4Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "You need to set up a burette.\n\nAttach a funnel and a burette to the holder at your table.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task5Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task4Complete;
        }
    }

    void gh_task5Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Next, fill the beaker with 5 drop from the Erlenmeyer flask with the white chemical.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task6Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task5Complete;
        }
    }

    void gh_task6Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "It's time for a break.\n\nI think I saw some coffee in your office. Pick it up and press (B) to take a drink.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task7Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task6Complete;
        }
    }

    void gh_task7Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Breaks over!\n\nFinish filling the beaker with 5 drop from the Erlenmeyer flask with the white chemical.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task8Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task7Complete;
        }
    }

    void gh_task8Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Now fill the burette with 10 drops from the Erlenmeyer flask with the red chemical.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task9Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task8Complete;
        }
    }

    void gh_task9Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Titrate until you see a change of color in the beaker, or until you run out of red chemical. When you're done, bring the beaker into the yellow zone.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task10Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task9Complete;
        }
    }

    void gh_task10Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Uh Oh!\n\nThe printer is acting up again, and it's printing important data. Smack it once or twice with your hand to get it running right.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task11Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task10Complete;
        }
    }

    void gh_task11Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Return to the red zone and titrate until you see a change of color in the beaker, or until you run out of red chemical. When you're done, bring the beaker into the yellow zone.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task12Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task11Complete;
        }
    }

    void gh_task12Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "Record your findings using the pencil and data sheet on the tables near the office. Touch the pencil to the paper to do so.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task13Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task12Complete;
        }
    }

    void gh_task13Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "The titration experiment is now complete. Please go to the trash to remove your gloves.";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gh_task14Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task13Complete;
        }
    }

    void gh_task14Complete(string context)
    {
        if (context.Contains("Glove_Hygiene_Task"))
        {
            curStep += 1;
            ghText.text = "This completes the glove hygiene module. We will now display all of the chemical spills created throughout the experience.\n(X) Hides Popup\n(Y) Exits Module";
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gh_task14Complete;
        }
    }
}
