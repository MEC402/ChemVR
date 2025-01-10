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
    }

    public void SetVignette(bool value)
    {
        vignette = !vignette;
        NotifyValuesChanged();
    }

    public void SetSmoothMove(bool value)
    {
        if(smoothMove != value)
        {
            smoothMove = value;
            NotifyValuesChanged();
        }
    }

    public void SetTurnSpeed(float value)
    {
        turnSpeed = value;
        NotifyValuesChanged();
    }

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
