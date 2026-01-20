using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirAccepter : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool isStirTarget;
    [SerializeField] private string stirName;

    #endregion
    #region Unity Methods

    public bool TryStirTarget(string stirName)
    {
        if(isStirTarget)
        {
           if(stirName == this.stirName)
            {
                return true;
            } 
            else
                return false;
        }
        return false;
    }

    #endregion
}
