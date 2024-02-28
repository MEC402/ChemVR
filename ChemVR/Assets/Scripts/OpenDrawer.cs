using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDrawer : MonoBehaviour
{
    [SerializeField]
    GameObject leftHand;
    [SerializeField]
    GameObject rightHand;

    private Vector3 force;
    private Vector3 cross;
    private bool holdingHandle;
    private float angle;
    private const float forceMultiplier = 150f;

    void Update()
    {
        if (holdingHandle)
        {
            GetComponentInParent<Rigidbody>().angularVelocity = cross * angle * forceMultiplier;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)){
                holdingHandle = true;

                Vector3 doorPivotToHand = leftHand.transform.position - transform.parent.position;

                doorPivotToHand.y = 0;

                force = leftHand.transform.position - transform.position;

                cross = Vector3.Cross(doorPivotToHand, force);
                angle = Vector3.Angle(doorPivotToHand, force);
            }
            else if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                holdingHandle = true;

                Vector3 doorPivotToHand = rightHand.transform.position - transform.parent.position;

                doorPivotToHand.y = 0;

                force = rightHand.transform.position - transform.position;

                cross = Vector3.Cross(doorPivotToHand, force);
                angle = Vector3.Angle(doorPivotToHand, force);
            }
            else
            {
                GetComponentInParent<Rigidbody>().angularVelocity = Vector3.zero;
                holdingHandle = false;
            }
        }
    }
}
