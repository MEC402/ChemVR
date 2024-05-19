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


    void Update()
    {
        float upperX = 3.15f;
        float lowerX = 1.10f;
        float upperZ = 4.21f;
        float lowerZ = 3.38f;
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
