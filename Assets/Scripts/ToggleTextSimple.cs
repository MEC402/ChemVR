using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using TMPro;



public class ToggleTextSimple : MonoBehaviour
{
    GameObject text;

    public void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            TextToggle();
        }
    }

    void Start()
    {
        text = this.gameObject.transform.GetChild(0).gameObject;
    }
    void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onLTriggerPressed += TextToggle;
        GameEventsManager.instance.taskEvents.onAdvanceTask += popUp;
    }

    //This code makes sure that if you complete a task the next popup appears without the need to toggle
    private void popUp(string obj)
    {
        text.SetActive(true);
        GameEventsManager.instance.miscEvents.TextPopUp();
    }

    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onLTriggerPressed -= TextToggle;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= popUp;
    }

    void TextToggle(InputAction.CallbackContext context)
    {
            text.SetActive(!text.activeInHierarchy);
    }
    void TextToggle()
    {
        text.SetActive(!text.activeInHierarchy);
    }
}
