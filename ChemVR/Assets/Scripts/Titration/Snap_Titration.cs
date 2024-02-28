using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap_Titration : MonoBehaviour
{
    [SerializeField]
    private GameObject titrationRig;
    private Vector3 pos;
    private Vector3 scale;
    private static float xOffset = .046f;
    private static float yOffset = -.0372f;

    private void Start()
    {
        
        pos = this.transform.position;
        pos.x += xOffset;
        pos.y += yOffset;
        scale = new Vector3(0.05f,0.05f,0.05f);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Dropper"))
        {
            if(other.GetComponent<OVRGrabbable>().isGrabbed == false)
            {
                GameObject rig = Instantiate(titrationRig);
                rig.transform.position = pos;
                rig.transform.localScale = scale;
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
