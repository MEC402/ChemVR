using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleText : MonoBehaviour
{
    private GameObject text;

    void Start()
    {
        text = this.gameObject.transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch))
        {
            if (text.activeInHierarchy)
            {
                text.SetActive(false);
            }
            else
            {
                text.SetActive(true);
            }
        }
        
    }
}
