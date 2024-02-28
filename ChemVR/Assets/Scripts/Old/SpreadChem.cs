using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadChem : MonoBehaviour
{
    [SerializeField]
    private GameObject chemMarker;

    void OnTriggerEnter(Collider col)
    {
        if (!(col.gameObject.tag.Equals("Fill") || col.gameObject.tag.Equals("Tool") || col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("Spill")))
        {
            GameObject spill = Instantiate(chemMarker.gameObject, this.transform.position, new Quaternion());
            spill.transform.SetParent(col.gameObject.transform);
            spill.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
        }
    }
}
