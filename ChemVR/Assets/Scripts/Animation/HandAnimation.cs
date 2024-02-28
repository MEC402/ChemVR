using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anime;
    private OVRGrabber grab;

    void Start()
    {
        anime = this.GetComponent<Animator>();
        grab = this.transform.parent.GetComponent<OVRGrabber>();
    }

    // Update is called once per frame
    void Update()
    {
        if(grab.grabbedObject != null)
        {
            anime.SetBool("IsGrabbing",true);
        }
        else
        {
            anime.SetBool("IsGrabbing",false);
        }
    }
}
