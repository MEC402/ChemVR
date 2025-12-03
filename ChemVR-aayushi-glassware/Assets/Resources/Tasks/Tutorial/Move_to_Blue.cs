using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_to_Blue : TaskStep
{
    GameObject mainCamera;
    public GameObject areaHighlighter;
    private GameObject createdObject;

    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
        if(areaHighlighter != null)
        {
            Vector3 position = new Vector3(0.130679995f, 0.221000001f, 7.5323329f); 
            Vector3 rotationEuler = new Vector3(270f, 0, 0);
            Quaternion rotation = Quaternion.Euler(rotationEuler);
            createdObject = Instantiate(areaHighlighter, position, rotation);
        }
    }
    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Tutorial Goals/2"));
    }
    void Update()
    {
        float upperX = 1.374f;
        float lowerX = -1.012f;
        float upperZ = 8.577f;
        float lowerZ = 6.395f;
        float xPos = mainCamera.transform.position.x;
        float zPos = mainCamera.transform.position.z;

        // Log the positions and bounds
        //Debug.Log($"xPos: {xPos}, zPos: {zPos}");

        if (((xPos > lowerX) && (xPos < upperX)) && ((zPos > lowerZ) && (zPos < upperZ)))
        {
           FinishTaskStep();
        }
    }

    private new void OnDestroy()
    {
        if (createdObject != null)
        {
            Destroy(createdObject);
        }
        base.OnDestroy();
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
