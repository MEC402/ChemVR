using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial_Overview : MonoBehaviour
{
    public TextMeshProUGUI tutText;

    void OnEnable()
    {
        tutText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            tutText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAdvanceTask += task0Complete;
    }

    void OnDisable()
    {
        GameEventsManager.instance.taskEvents.onAdvanceTask -= task2Complete;
    }

    void task0Complete(string context)
    {
        tutText.text = "Welcome to the tutorial!\n\nYou can hide this popup with the primary button on your left controller, (X).\n\nTry hiding this popup and re-opening it by pressing (X) twice!";
        GameEventsManager.instance.taskEvents.onAdvanceTask += task1Complete;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= task0Complete;

    }
    void task1Complete(string context)
    {
        //Debug.LogWarning("task 1 complete in overview");
        tutText.text = "Good Job!\n\nNow approach the table with beakers on it.\n\nNotice that it is inside the blue square on the floor.";
        GameEventsManager.instance.taskEvents.onAdvanceTask += task2Complete;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= task1Complete;


    }
    void task2Complete(string context)
    {
        tutText.text = "Great!\n\nYou can pick up objects using the main triggers on either hand.\n\nNow that you're here, give it a try.";
        //GameEventsManager.instance.taskEvents.onAdvanceTask += task3Complete;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= task2Complete;
    }
}
