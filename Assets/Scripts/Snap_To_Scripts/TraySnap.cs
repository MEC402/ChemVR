using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraySnap : MonoBehaviour
{
    [SerializeField] private GameObject[] ObjectPoints = new GameObject[6];

    private SnapToTray[] snappedObjects = new SnapToTray[6];

    public void AddMe(SnapToTray obj)
    {
        bool pointTaken = false;
        //Find the first available snap point
        for (int i = 0; i < ObjectPoints.Length; i++)
        {
            if (snappedObjects[i] == null)
            {
                obj.SetObject(ObjectPoints[i], i);
                snappedObjects[i] = obj;
                pointTaken = true;
                break;
            }
        }
        if (!pointTaken)
        {
            obj.LetGo();
        }
    }

    public void RemoveMe(int pointIndex)
    {
        snappedObjects[pointIndex] = null;
    }
}
