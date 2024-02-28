using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRightGlove : MonoBehaviour
{
    public Material gloves;
    public GameObject rightHand;

    private ParticleSystem spillLocation;
    private Material original;
    private bool wearingGloves;
    void Start()
    {
        spillLocation = GameObject.FindGameObjectWithTag("ParticleHolder").GetComponent<ParticleSystem>();
        original = rightHand.GetComponent<Renderer>().material;
        wearingGloves = false;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Glove Box"))
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                wearingGloves = true;
                rightHand.GetComponent<Renderer>().material = gloves;
            }
        }
    }

    public bool HasGloves()
    {
        return wearingGloves;
    }
}

