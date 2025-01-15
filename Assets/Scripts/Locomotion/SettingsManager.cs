using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private bool isSmoothMove;
    public Toggle movementToggle;
    public LocoOptions locoOptions;
    private string[] MOVEMENT_TEXT = { "Smooth Walk Enabled", "Smooth Walk Disabled" };
    public GameObject debugText;


    //script should translate toggles in scene to change settings on the player.
    //references the scriptable obj LocoOptions
    //LocoOptions changes the settings on the player w/ references to LocoManager script

    //TOGGLES THE MOVEMENT TYPE
    public void SetMovementType()
    {
        if(movementToggle.isOn)
        {
            locoOptions.SetSmoothMove(isSmoothMove);
            isSmoothMove = true;
        }
        else if(!movementToggle.isOn)
        {
            locoOptions.SetSmoothMove(!isSmoothMove);
            isSmoothMove = false;
        }
        //isSmoothMove = !isSmoothMove;

        debugText.GetComponent<TextMeshProUGUI>().text = isSmoothMove ? MOVEMENT_TEXT[0] : MOVEMENT_TEXT[1];
    }

    //toggles SnapTurn on and off
    public void SetSnapTurn()
    {

    }

    //sets a turn speed value (float) when the toggle is on/off
    //on should be set to value of #?
    //off should be set to value of 0
    public void SetTurnSpeed()
    {

    }
}
