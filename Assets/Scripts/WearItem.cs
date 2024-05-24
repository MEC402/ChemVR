using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class WearItem : MonoBehaviour
{

    public GameObject itemToWear;
    private GameObject leftHand;
    private GameObject rightHand;
    static private bool wearing; 
    // Start is called before the first frame update
    private void OnXPress(InputAction.CallbackContext obj)
    {
        if (Physics.OverlapBox(leftHand.transform.position, leftHand.transform.localScale / 2, Quaternion.identity).Length > 0 &&
            Physics.OverlapBox(itemToWear.transform.position, itemToWear.transform.localScale / 2, Quaternion.identity).Length > 0)
        {
            PutOn();
        }
    }

    private void OnAPress(InputAction.CallbackContext obj)
    {
        if (Physics.OverlapBox(rightHand.transform.position, rightHand.transform.localScale / 2, Quaternion.identity).Length > 0 &&
            Physics.OverlapBox(itemToWear.transform.position, itemToWear.transform.localScale / 2, Quaternion.identity).Length > 0)
        {
            PutOn();
        }
    }

    private void PutOn()
    {
        wearing = true;
        itemToWear.SetActive(false);
    }

    static public bool IsWearing()
    {
        return wearing;
    }
}

