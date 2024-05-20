using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTasks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventsManager.instance.taskEvents.StartTask("Tutorial_Task");
        GameEventsManager.instance.taskEvents.StartTask("Chemical_Change_Task");
        GameEventsManager.instance.taskEvents.StartTask("Glassware_Use_Task");
        GameEventsManager.instance.taskEvents.StartTask("Glove_Hygiene_Task");
    }
}
