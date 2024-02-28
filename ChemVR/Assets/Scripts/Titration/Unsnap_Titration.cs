using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unsnap_Titration : MonoBehaviour
{
    [SerializeField]
    private GameObject titrationHolder;
    [SerializeField]
    private GameObject titrationDropper;

    private OVRGrabbable grab;
    public Vector3 pos;
    private static float xOffset = .046f;
    private static float yOffset = -.0372f;
    void Start()
    {
        pos = this.transform.parent.gameObject.transform.position;
        pos.x -= xOffset;
        pos.y -= yOffset;
        grab = this.GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if(grab.isGrabbed == true)
        {
            OVRGrabber hand = grab.grabbedBy;
            hand.ForceRelease(grab);

            GameObject holder = Instantiate(titrationHolder);
            holder.transform.position = pos;

            GameObject dropper = Instantiate(titrationDropper);
            dropper.transform.position = this.transform.position;
            dropper.GetComponent<OVRGrabbable>().GrabBegin(hand,dropper.GetComponent<Collider>());
            //hand.ForceGrab(dropper.GetComponent<OVRGrabbable>());

            Destroy(this.transform.parent.gameObject);
        }
    }
}
