using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitChemInWorld : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        if (this.transform.childCount > 200)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }
}
