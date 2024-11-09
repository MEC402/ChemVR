using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial_Overview : MonoBehaviour
{
    public TextMeshProUGUI tutText;
    public GameObject controllerDiagrams;
    Controller_Diagram diagramController;
    int curStep;

    // Controllers for starting tasks
    [Header("Start Task Controller")]
    public Start_Module taskPreper;

    void OnEnable()
    {
        diagramController = controllerDiagrams.GetComponent<Controller_Diagram>();
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = 0;
        tutText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            tutText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAdvanceTask += AdvanceTutTask;
        GameEventsManager.instance.taskEvents.onAbandonTask += tu_abandonMe;

        //Start the Tutorial on opening this scene
        taskPreper.Show();
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
        GameEventsManager.instance.taskEvents.StartTask("Tutorial_Task");
        tutText.text = text[curStep];
        handleDiagrams();
    }

    void OnDisable()
    {
        //Remove any listeners
        diagramController.hideAllDiagrams();
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
            diagramController.hideAllDiagrams();
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
            GameObject tutPop = GameObject.Find("Tutorial PopUp");
            tutText.text = "Don't look at me I'm inactive.";
            if (tutPop != null)
            {
                tutPop.SetActive(false);
            }
            curStep = 0;
        }
    }

    string[] text = {"Welcome to the tutorial!\n\nYou can hide this popup with the button Y.\n\nIt is the top button on the left controller.\n\nTry hiding this popup and re-opening it by pressing the button twice!",
                    "Good Job!\n\nTurn and look around by turning your head.You can also teleport with either joystick by pushing it forward and releasing on the desired location.Teleport into the section of the room notated by the blue square on the floor, with the tables labelled 'B'.",
                    "The grips on the back of your controller can be used to grab and interact with items.\n\nFor now, skip with A",
                    "The primary buttons, (A) and (X) are your main way of interacting with objects!\n\nPress one to show you know where they are.",
                    "The hamburger button opens the help menu!\nPress it anytime to see the help menu, or for access to the main menu.\n\nGive it a try, as the Tutorial is complete!"
                    };
    void AdvanceTutTask(string context)
    {
        if (context.Contains("Tutorial_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            tutText.text = text[curStep];
            handleDiagrams();
        }
    }

    void handleDiagrams()
    {
        diagramController.hideAllDiagrams();
        if (curStep == 0)
        {
            diagramController.showLeftController();
            diagramController.showYButton();
        } else if (curStep == 1)
        {
            diagramController.showLeftController();
            diagramController.showLeftToggle();
        } else if (curStep == 2)
        {
            diagramController.showLeftController();
            diagramController.showRightController();
            diagramController.showLeftGrip();
            diagramController.showRightGrip();
        } else if (curStep == 3)
        {
            diagramController.showAllDiagrams();
            diagramController.showAButton();
            diagramController.showXButton();
        }
        else if (curStep == 4) 
        {
            diagramController.showAllDiagrams();
            diagramController.showBButton();
            diagramController.showYButton();
        }
    }
}
