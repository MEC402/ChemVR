using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSpill : MonoBehaviour
{
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
           
            
          this.GetComponent<MeshRenderer>().enabled = true;
            
        }
        else
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }

    }
}
