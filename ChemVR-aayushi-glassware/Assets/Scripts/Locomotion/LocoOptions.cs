using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocoOptions", menuName = "ScriptableObjects/LocoOptions")]
public class LocoOptions : ScriptableObject
{
    public bool snapTurn; //false = smooth turn
    public bool vignette;
    public bool smoothMove;
    public float turnSpeed;
    public float snapTurnAmount;
    public float moveSpeed;

    public event System.Action OnVlauesChanged;

    private void NotifyValuesChanged()
    {
       OnVlauesChanged?.Invoke();
    }

    public void SetSnapTurn(bool value)
    {
        snapTurn = !snapTurn;
        NotifyValuesChanged();
        //left controller should have smooth turn enabled by default, right have snap by default
    }

   /* public void SetVignette(bool value)
    {
        vignette = !vignette;
        NotifyValuesChanged();
    }*/

    public void SetSmoothMove(bool value)
    {
        /* if(smoothMove != value)
         {
             smoothMove = value;
             NotifyValuesChanged();
         }*/

        smoothMove = !smoothMove;
        NotifyValuesChanged();
    }

    public void SetTurnSpeed(float value)
    {
        turnSpeed = value;
        NotifyValuesChanged();
        //this sets the value for smooth turn
        //20 is standard, 0 should be used for when snap turn is enabled
    }

    //this is the value that should be changed to enable/disable snap turn
    //45 is standard value
    public void SetSnapTurnAmount(float value)
    {
        snapTurnAmount = value;
        NotifyValuesChanged();
    }

    
    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
        NotifyValuesChanged();
    }
}
