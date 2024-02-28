using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChem : MonoBehaviour
{
    public GameObject chemical;
    public GameObject top;
    public Material material;
    public bool isFacingUp;
    public bool notDropper;

    private GameObject chemHolder;
    void Start()
    {
        chemHolder = GameObject.FindGameObjectWithTag("ChemHolder");
    }
    void Update()
    {
        if (notDropper)
        {
            if (isFacingUp)
            {
                if (transform.up.y < -.5f)
                {
                    Vector3 position = top.transform.position;
                    GameObject chem = Instantiate(chemical, position, new Quaternion());
                    chem.transform.SetParent(chemHolder.transform);
                    chem.GetComponent<Renderer>().material = material;
                }
            }
            else
            {
                if (transform.up.y > .5f)
                {
                    Vector3 position = top.transform.position;
                    GameObject chem = Instantiate(chemical, position, new Quaternion());
                    chem.transform.SetParent(chemHolder.transform);
                    chem.GetComponent<Renderer>().material = material;
                }
            }
        }
        else
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && this.GetComponent<OVRGrabbable>().isGrabbed)
            {
                Vector3 position = top.transform.position;
                GameObject chem = Instantiate(chemical, position, new Quaternion());
                chem.transform.SetParent(chemHolder.transform);
                chem.GetComponent<Renderer>().material = material;
            }
        }
    }
}
