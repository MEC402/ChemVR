using System;
using UnityEngine;

public class TrackedObject : MonoBehaviour
{
    private Vector3 defaultRotation;
    private bool isRotated;





    private void Start()
    {
        defaultRotation = this.gameObject.transform.localRotation.eulerAngles;
        ResetTaskManager.instance.onResetCalled += ResetRotationBools;
    }
    private void OnDisable()
    {
        ResetTaskManager.instance.onResetCalled -= ResetRotationBools;
    }
    public void ToggleRotation()
    {
        isRotated = !isRotated;
    }
    public Vector3 GetDefaultRotation()
    {
        return defaultRotation;
    }
    public bool GetIsRotated()
    {
        return isRotated;
    }

    private void ResetRotationBools(object sender, EventArgs e)
    {
        isRotated = false;
    }



}
