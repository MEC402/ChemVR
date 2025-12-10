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
    }

    void OnDisable()
    {
        //Remove any listeners
        diagramController.hideAllDiagrams();
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= AdvanceTutTask;
        GameEventsManager.instance.taskEvents.onAbandonTask -= tu_abandonMe;
    }

    private void Start()
    {
        //Start the Tutorial on opening this scene
        taskPreper.Show();
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
        GameEventsManager.instance.taskEvents.StartTask("Tutorial_Task");
        tutText.text = text[curStep];
        handleDiagrams();
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

    string[] text = {"Welcome to the Tutorial.\n\nYou will be guided through experiments with these pop ups.\n\nPop ups can be hidden and opened with the button (Y).\n\nPractice hiding this pop up by pressing (Y) twice.",
                    "Look around the room by turning your head.\n\nTeleport by pushing either joystick forward and releasing it when the circle is where you want to go.\n\nLook around until you see a highlighted area, and teleport into it.",
                    "The lower buttons on either controller, (A) and (X), are used to interact with items.\n\nYou should see a glove box on a table near you. Reach inside with each hand and use (A) and (X) to put on gloves.",
                    "Great!\n\nNear the gloves, you should see a folded lab coat and a box of goggles.\n\nUsing the same method, put these on.",
                    "You can use grips on the back of your controllers can be used to grab items.\n\nYou should see a few flasks on one of the tables near you.\n\nTeleport to the table and pick up one of the flasks.",
                    "Well done!\n\nYou can also pour solutions between containers.\n\nTilt a flask over the beaker and pour into it.",
                    "Nice!\n\nThe hamburger button opens and closes the help menu.\nPress it at any time to see the button configurations, or for access to the main menu.\n\nTry opening and closing it by pressing the button twice.",
                    "This completes the tutorial.\n\nFeel free to explore the room and get familiar with the lab.\nWhen you are done, you can either:\n\nPress and hold (A) or (X) to move on to the first module\n\nUse the hamburger button to navigate to the main menu.",
                    "Good luck!"
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
            diagramController.showAllDiagrams();
            diagramController.showAButton();
            diagramController.showXButton();
            
        } else if (curStep == 4)
        {
            diagramController.showLeftController();
            diagramController.showRightController();
            diagramController.showLeftGrip();
            diagramController.showRightGrip();
        }
        else if (curStep == 6) 
        {
            diagramController.showLeftController();
            diagramController.showHamburgerButton();
        }
    }
}
