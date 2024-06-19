using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Task_3 : TaskStep
{
    GameObject mainCamera;
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
    }


    void Update()
    {
        float upperX = 4.02f;
        float lowerX = -3.37f;
        float upperZ = 21.07f;
        float lowerZ = 13.85f;
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
