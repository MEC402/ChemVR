using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraySnap : MonoBehaviour
{
    [SerializeField] private GameObject[] ObjectPoints = new GameObject[6];

    private SnapToTray[] snappedObjects = new SnapToTray[6];

    public void AddMe(SnapToTray obj)
    {
        //Find the nearest available snap point to where the object was released
        int nearestIndex = -1;
        float nearestDistance = float.MaxValue;
        for (int i = 0; i < ObjectPoints.Length; i++)
        {
            if (snappedObjects[i] == null)
            {
                float distance = Vector3.Distance(obj.transform.position, ObjectPoints[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestIndex = i;
                }
            }
        }

        if (nearestIndex >= 0)
        {
            obj.SetObject(ObjectPoints[nearestIndex], nearestIndex);
            snappedObjects[nearestIndex] = obj;
        }
        else
        {
            obj.LetGo();
        }
    }

    public void RemoveMe(int pointIndex)
    {
        snappedObjects[pointIndex] = null;
    }
}
