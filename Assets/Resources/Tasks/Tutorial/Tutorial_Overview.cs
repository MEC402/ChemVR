using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial_Overview : MonoBehaviour
{
    public GameObject task1;
    public GameObject task2;

    void Update()
    {
        
    }

    void OnEnable()
    {
        GameEventsManager.instance.taskEvents.onAdvanceTask += task0Complete;
    }

    void OnDisable()
    {
        GameEventsManager.instance.taskEvents.onAdvanceTask -= task2Complete;
    }

    void task0Complete(string context)
    {
        Debug.LogWarning("task 0 complete in overview");
        task1.SetActive(true);
        GameEventsManager.instance.taskEvents.onAdvanceTask += task1Complete;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= task0Complete;

    }
    void task1Complete(string context)
    {
        task1.SetActive(false);
        task2.SetActive(true);
        GameEventsManager.instance.taskEvents.onAdvanceTask += task2Complete;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= task1Complete;


    }
    void task2Complete(string context)
    {
        
    }
}
