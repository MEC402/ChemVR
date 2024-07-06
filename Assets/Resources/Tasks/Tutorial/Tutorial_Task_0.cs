using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tutorial_Task_0 : TaskStep
{
    public void Start()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed += Complete;
        GameEventsManager.instance.inputEvents.onBButtonPressed += Abandon;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= Complete;
        GameEventsManager.instance.inputEvents.onBButtonPressed -= Abandon;
    }
    public void Complete(InputAction.CallbackContext obj)
    {
        Complete();
    }
    public void Complete()
    {
        Debug.Log("Task 0 completed");
        Restart();
        FinishTaskStep();
    }

    public void Restart()
    {
        GameObject taskManager = GameObject.Find("The Managers");
        int childCount = taskManager.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = taskManager.transform.GetChild(i).gameObject;
            if (child.name.Contains("tut") && !child.name.Contains("_0"))
            {
                Destroy(child);
            }
            else if (!child.name.Contains("tut"))
            {
                Destroy(child);
            }
        }
    }
    public void Abandon(InputAction.CallbackContext obj)
    {
        Abandon();
    }
    public void Abandon()
    {
        GameObject destroy = GameObject.Find("Tutorial_Task_0(Clone)");
        Destroy(destroy);
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
