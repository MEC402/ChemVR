using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;



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
        GameEventsManager.instance.inputEvents.onXButtonPressed += TextToggle;
    }

    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onXButtonPressed -= TextToggle;
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
