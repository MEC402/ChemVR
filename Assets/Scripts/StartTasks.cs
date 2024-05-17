using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTasks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventsManager.instance.taskEvents.StartTask("Tutorial_Task");
    }
}
