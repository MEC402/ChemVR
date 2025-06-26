using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirAccepter : MonoBehaviour
{
    #region Variables
    private GameObject insertedObject = null;
    private Rigidbody insertedRigidbody = null;

    [SerializeField] private Transform lockPoint;

    #endregion
    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StirTool") && insertedObject == null)
        {
            Debug.Log("Object inserted into the stir accepter.");
            insertedObject = other.gameObject.transform.parent != null ? other.transform.parent.gameObject : other.gameObject;
            insertedRigidbody = insertedObject.GetComponentInParent<Rigidbody>();
            if (insertedRigidbody != null)
            {
                insertedRigidbody.isKinematic = true;
                insertedRigidbody.MovePosition(transform.position);
                insertedRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StirTool") && insertedObject != null)
        {
            Debug.Log("Object removed from the stir accepter.");
            if (insertedRigidbody != null)
            {
                insertedRigidbody.isKinematic = false;
                insertedRigidbody.constraints = RigidbodyConstraints.None;

            }
            insertedObject = null;
            insertedRigidbody = null;
        }
    }

    #endregion
}
