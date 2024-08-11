using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Task_2 : TaskStep
{
    GameObject mainCamera;
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
    }


    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Tutorial Goals/2"));
    }

    void Update()
    {
        float upperX = 3.96f;
        float lowerX = -3.24f;
        float upperZ = 11.19f;
        float lowerZ = 3.79f;
        float xPos = mainCamera.transform.position.x;
        float zPos = mainCamera.transform.position.z;

        // Log the positions and bounds
        //Debug.Log($"xPos: {xPos}, zPos: {zPos}");

        if (((xPos > lowerX) && (xPos < upperX)) && ((zPos > lowerZ) && (zPos < upperZ)))
        {
           FinishTaskStep();
        }
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
