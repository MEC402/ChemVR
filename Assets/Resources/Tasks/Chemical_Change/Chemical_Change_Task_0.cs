using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Chemical_Change_Task_0 : TaskStep
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
            if (child.name.Contains("che") && !child.name.Contains("0"))
            {
                Destroy(child);
            }
            else if (!child.name.Contains("che"))
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
        GameObject destroy = GameObject.Find("Chemical_Change_Task(Clone)");
        Destroy(destroy);
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

}
