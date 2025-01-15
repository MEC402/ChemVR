using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private bool isSmoothMove;
    private bool isSnapTurn;
    private bool isSmoothTurn;

    public Toggle movementToggle;
    public Toggle snapToggle;
    public Toggle smoothTurnToggle;

    public LocoOptions locoOptions;

    private string[] MOVEMENT_TEXT = { "Smooth Walk Enabled", "Smooth Walk Disabled" };
    private string[] Smooth_Turn_Text = { "Smooth Turn Enabled", "Smooth Turn Disabled" };
    private string[] Snap_Turn_Text = { "Snap Turn Enabled", "Snap Turn Disabled" };

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
        if(snapToggle.isOn)
        {
            locoOptions.SetSnapTurn(isSnapTurn);
            isSnapTurn = true;
            locoOptions.SetTurnSpeed(0);
        }
        else if (!snapToggle.isOn)
        {
            locoOptions.SetSnapTurn(!isSnapTurn);
            isSnapTurn = false;
        }

        debugText.GetComponent<TextMeshProUGUI>().text = isSnapTurn ? Snap_Turn_Text[0] : Snap_Turn_Text[1];
    }

    //sets a turn speed value (float) when the toggle is on/off
    //on should be set to value of #?
    //off should be set to value of 0
    public void SetTurnSpeed()
    {
        if(smoothTurnToggle.isOn)
        {
            locoOptions.SetTurnSpeed(30);
            isSmoothTurn = true;
        }
        else if (!smoothTurnToggle.isOn)
        {
            locoOptions.SetTurnSpeed(0);
            isSmoothTurn = false;
        }

        debugText.GetComponent<TextMeshProUGUI>().text = isSmoothTurn ? Smooth_Turn_Text[0] : Smooth_Turn_Text[1];
    }
}
