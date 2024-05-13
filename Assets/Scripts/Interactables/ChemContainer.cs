using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemContainer : MonoBehaviour
{
    public float maxVolume;
    public float currentVolume;
    public float pourRate;
        
    public ResizeFluid internalFluid;
    public GameObject pourPoint;
    public GameObject hitMarker;
    public GameObject opening;

    [Tooltip("If true, pouring is activated by interacting with another object such as a faucet or knob. " + 
                "If false, pouring is activated when object is tilted past 90 degrees.")]
    public Boolean pouringUsesActivator;
    public GameObject pourActivator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (pouringUsesActivator) {
            if (pourActivator == null)
            {
                Debug.LogError("This ChemContainer is set to pour with an activator but no activator is specified.");
            }
        }
        if (internalFluid != null)
        {
            internalFluid.currentVolume = currentVolume;
            internalFluid.maxCapacity = maxVolume;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        internalFluid.currentVolume = currentVolume;
        internalFluid.maxCapacity = maxVolume;
    }

    private void FixedUpdate()
    {
        float angleBetween = Vector3.Angle(Vector3.up, transform.up);
        LayerMask mask = LayerMask.GetMask("Chem");
        RaycastHit hit;
        if (angleBetween >= 90 && Physics.Raycast(pourPoint.transform.position, Vector3.down, out hit, Mathf.Infinity, mask))
        {
            Debug.DrawRay(pourPoint.transform.position, Vector3.down * hit.distance, Color.yellow);            
            hitMarker.transform.position = hit.point;
            hitMarker.SetActive(true);
            ChemContainer recipientChemContainer = hit.collider.gameObject.GetComponentInParent<ChemContainer>();
            float amountPoured = pourRate * Time.fixedDeltaTime;

            if (amountPoured > currentVolume) { 
                amountPoured = currentVolume; 
            }
            float newVolume = recipientChemContainer.currentVolume + amountPoured;
            if (newVolume > recipientChemContainer.maxVolume) {
                amountPoured -= (newVolume - recipientChemContainer.maxVolume);
                // There is no overflow logic at this time. Instead, the pour just doesn't happen.
            }

            currentVolume -= amountPoured;
            recipientChemContainer.currentVolume += amountPoured;



        } else {
            hitMarker.SetActive(false);
        }

        //TODO: Add support for pouring via activators such as turning the burette's stopcock.
    }
}
