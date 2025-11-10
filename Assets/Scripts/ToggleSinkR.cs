using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSinkR : MonoBehaviour
{
    public bool isSinkOn = false;

   public void ToggleOpen()
    {
        if (!isSinkOn)
        {
            isSinkOn = true;
            transform.localEulerAngles = new Vector3(0f, 270f, 90f);
        }
        else if (isSinkOn) {
            isSinkOn = false;
            transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        }
    }
}
