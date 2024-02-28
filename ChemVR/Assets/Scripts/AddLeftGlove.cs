using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLeftGlove : MonoBehaviour
{
    public Material gloves;
    public GameObject leftHand;
    private Material original;
    private bool wearingGloves;
    void Start()
    {
        original = leftHand.GetComponent<Renderer>().material;
        wearingGloves = false;
    }
   
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag.Equals("Glove Box"))
        {
            if (OVRInput.GetDown(OVRInput.Button.Four))
            {
                wearingGloves = true;
                leftHand.GetComponent<Renderer>().material = gloves;
            }
        }
    }

    public bool HasGloves()
    {
        return wearingGloves;
    }

}
