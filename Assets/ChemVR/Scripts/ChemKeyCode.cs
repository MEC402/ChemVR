using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChemKeyCode : MonoBehaviour
{
    private Transform taskManager;

    // Start is called before the first frame update
    void Start()
    {
         taskManager = GameObject.Find("The Managers").transform;
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.N)) {        
            //EditorUtility.DisplayDialog("Test Display","Bad Idea", "ok");
            //AdvanceGloTask("Glove_Hygiene_Task");
        //Continue();
        //string taskName = (taskPreper.current.name).Replace(" ", "_");
        GameEventsManager.instance.taskEvents.AdvanceTask("Glove_Hygiene_Task");
        
        Destroy(taskManager.GetChild(0).gameObject);

 
         }
    }
}
