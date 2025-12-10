using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupToggle : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    [SerializeField] private Rigidbody myRigidbody;
    private bool rotationLocked = true;
    


    // Start is called before the first frame update
    void Start()
    {
        if (myCollider == null)
        {
            Debug.Log(this.gameObject.name + " does not have the toggle collider set. Please assign.");
        }
    }


    public void ToggleCollider()
    {
        if (myCollider != null)
        {
            if (!myCollider.enabled)
            {
                myCollider.enabled = true;
            }
            else if (myCollider.enabled)
            {
                myCollider.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("Cannot toggle collider because it has not been assigned to script.");
        }
    }

    public void ToggleZRotation()
    {
        if(myRigidbody != null)
        {
            if(rotationLocked == true)
            {
                myRigidbody.constraints = RigidbodyConstraints.None;
                rotationLocked = false;
            }
            else if(rotationLocked == false)
            {
                myRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
                rotationLocked = true;
            }
        }
        else
        {
            Debug.LogWarning("Cannot toggle rigidbody constraints because rigidbody has not been assigned to script.");
        }
    }
}
