using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidContainer : MonoBehaviour
{
    public float maxVolume;
    public float currentVolume;
    public float pourRate;

    public ResizeFluid internalFluid;
    public GameObject pourPoint;
    public GameObject hitMarker;

    public GameObject recipient;

    // Start is called before the first frame update
    void Start()
    {
        if (internalFluid == null)
        {
            internalFluid = this.GetComponentInChildren<ResizeFluid>();
            Debug.Log(this.gameObject.name + "'s FluidContainer was not given a ResizeFluid. Setting to first ResizeFluid found in object's children.");
        }
        internalFluid.currentVolume = currentVolume;
        internalFluid.maxCapacity = maxVolume;
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
        RaycastHit hit;
        if (angleBetween >= 90 && Physics.Raycast(pourPoint.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(pourPoint.transform.position, Vector3.down * hit.distance, Color.yellow);
            
            hitMarker.transform.position = hit.point;
            hitMarker.SetActive(true);

            recipient = hit.collider.gameObject;

            Debug.Log("Pouring into " + recipient.name);

            FluidContainer recipientFluidContainer = recipient.GetComponent<FluidContainer>();
            if (recipientFluidContainer != null)
            {
                //The object is able to receive fluid, and a pour is initiated
                float amountPoured = pourRate * Time.fixedDeltaTime;
                currentVolume = Mathf.Clamp(currentVolume - amountPoured, 0f, maxVolume);
                recipientFluidContainer.currentVolume = Mathf.Clamp(recipientFluidContainer.currentVolume + amountPoured, 0f, recipientFluidContainer.maxVolume);

            } else
            { 
                //The object is not able to receive fluid, and a spill is created

            }

        } else {
            hitMarker.SetActive(false);
            recipient = null;
        }
    }
}
