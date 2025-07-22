using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    #region Variables
    [SerializeField] private Collider[] collidersToDisable;
    [SerializeField] private GameObject gripHandle;

    private bool isInserted = false;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stirrable") && !isInserted)
        {
            Debug.Log("Object inserted into the tip trigger.");
            isInserted = true;
            gripHandle.SetActive(true);
            foreach (Collider collider in collidersToDisable)
            {
                collider.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stirrable") && isInserted)
        {
            Debug.Log("Object removed from the tip trigger.");
            isInserted = false;
            gripHandle.SetActive(false);
            foreach (Collider collider in collidersToDisable)
            {
                collider.enabled = true;
            }
        }
    }
    

    public bool IsInserted()
    {
        return isInserted;
    }

    #endregion

}
