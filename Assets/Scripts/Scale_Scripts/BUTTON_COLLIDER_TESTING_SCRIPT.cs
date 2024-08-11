using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUTTON_COLLIDER_TESTING_SCRIPT : MonoBehaviour
{
    public enum button { on, off, mode, tare };
    public button whichButton;
   
    public void ButtonPress()
    {
        Debug.Log(whichButton + " Button Pressed");
    }
}
