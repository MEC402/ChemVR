using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChemTrail : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            Destroy(this.gameObject);
            Destroy(this);
        }
    }
}
