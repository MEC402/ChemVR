using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTasks : MonoBehaviour
{
    GameObject taskManager;
    

    private void OnEnable()
    {
        taskManager = GameObject.Find("The Managers");
    }
    public void StartTutorial()
    {
        RemoveUnstarted();
        GameEventsManager.instance.taskEvents.StartTask("Tutorial_Task");
    }

    public void StartChemicalChange()
    {
        RemoveUnstarted();
        GameEventsManager.instance.taskEvents.StartTask("Chemical_Change_Task");
    }

    public void StartGlasswareUse()
    {
        RemoveUnstarted();
        GameEventsManager.instance.taskEvents.StartTask("Glassware_Use_Task");
    }

    public void StartGloveHygiene()
    {
        RemoveUnstarted();
        GameEventsManager.instance.taskEvents.StartTask("Glove_Hygiene_Task");
    }


    private void RemoveUnstarted()
    {
        int childCount = taskManager.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = taskManager.transform.GetChild(i).gameObject;
            if (child.name.Contains("_0"))
            {
                Destroy(child);
            }
        }
    }
}
