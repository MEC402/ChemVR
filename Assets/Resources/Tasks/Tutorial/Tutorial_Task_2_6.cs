using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Task_2_6 : TaskStep
{
    GameObject mainCamera;
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    float currentY;
    private void Start()
    {
        currentY = mainCamera.transform.eulerAngles.y;
    }
    void FixedUpdate()
    {
        float nextY = mainCamera.transform.eulerAngles.y;
        //Debug.Log("Current Y: " + currentY + ", Next Y: " + nextY);
        //Debug.Log("Difference: " + (currentY - nextY));
        if (Mathf.RoundToInt(Mathf.Abs((nextY - currentY))).Equals(180))
        {
            FinishTaskStep();
        }
        currentY = nextY;
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
