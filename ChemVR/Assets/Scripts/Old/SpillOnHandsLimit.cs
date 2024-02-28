using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillOnHandsLimit : MonoBehaviour
{     void Update()
    {
        if(this.transform.childCount > 20)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }
}
