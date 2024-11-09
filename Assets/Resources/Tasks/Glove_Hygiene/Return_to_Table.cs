using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Return_to_Table : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    GameObject mainCamera;
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
        if (mainCamera == null)
        {
            Debug.LogError("No camera registered");
        }
    }

    private void OnEnable()
    {
        //point at the beaker
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Glove Hygiene Goals/4"));
    }

    void Update()
    {
        float upperX = 3.93f;
        float lowerX = -3.34f;
        float upperZ = 21.08f;
        float lowerZ = 13.88f;
        float xPos = mainCamera.transform.position.x;
        float zPos = mainCamera.transform.position.z;

        // Log the positions and bounds
        //Debug.Log($"xPos: {xPos}, zPos: {zPos}");

        if (((xPos > lowerX) && (xPos < upperX)) && ((zPos > lowerZ) && (zPos < upperZ)))
        {
            FinishTaskStep();
        }
    }
}
