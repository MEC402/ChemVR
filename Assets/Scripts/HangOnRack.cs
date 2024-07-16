using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HangOnRack : MonoBehaviour
{
    public HashSet<GameObject> itemsOnRack;
    private Vector3 lastPos = Vector3.zero;

    /*private void Start()
    {
        GameEventsManager.instance.inputEvents.onLGripReleased += waitForLetGo;
        GameEventsManager.instance.inputEvents.onRGripReleased += waitForLetGo;
    }

    private void waitForLetGo(InputAction.CallbackContext obj)
    {
        Debug.Log("Released Hand");
    } */

    private void OnCollisionEnter(Collision collision)
    {
        GameObject item = collision.collider.gameObject;
        Rigidbody itemRB = item.GetComponent<Rigidbody>();
        if (itemRB != null && !itemsOnRack.Contains(item) && transform.childCount < 1)//has rigidbody, not already on rack, there isn't already something on this spot
        {
            if(itemRB.isKinematic == false)
            {
                // Freeze the object to hang
                itemRB.isKinematic = true;

                // Set object as a child of the hanger
                item.transform.SetParent(transform);

                // Snap location to parent
                string name = item.name.ToLower();
                if (name.Contains("burette"))
                {
                    item.transform.localPosition = new Vector3(0, 0, .15f);
                }
                else if (name.Contains("flask"))
                {
                    item.transform.localPosition = new Vector3(0, 0, .175f);
                }
                else //usually this is the beaker
                {
                    item.transform.localPosition = new Vector3(0, 0, .075f);
                }

                //Keep track of motion
                lastPos = item.transform.localPosition;
                
                item.transform.localEulerAngles = new Vector3(-90, 0, 0);
                itemRB.velocity = Vector3.zero;

                itemsOnRack.Add(item);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject item = collision.collider.gameObject;
        Rigidbody itemRB = item.GetComponent<Rigidbody>();
        if (itemRB != null && itemsOnRack.Contains(item) && transform.childCount > 0)
        {
            if (itemRB.isKinematic == true)
            {
                //Check for motion
                if(!lastPos.Equals(item.transform.localPosition))
                {
                    // Unfreeze the object when it exits the collider
                    itemRB.isKinematic = false;

                    //remove parent
                    item.transform.SetParent(null);

                    itemsOnRack.Remove(item);
                }
            }
        }
    }
}
