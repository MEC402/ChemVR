using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForPutAway : MonoBehaviour
{
    //  /*11*/"Bring the copper sulfate and DI water back to your hood, and then open the hood and lower it to a working height.\n\n\nSkip with A",
    // Start is called before the first frame update
    private bool BeakerinCabinet = false;
    private bool GraduatedCylinderinCabinet = false;
    private bool flask45inCabinet = false;
    private bool flask55inCabinet = false;
    private bool flask65inCabinet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower().Contains("beakerup250ml"))
        {
            BeakerinCabinet = true;
        }
        if (other.name.ToLower().Contains("graduatedcylinder"))
        {
            GraduatedCylinderinCabinet = true;
        }
        if (other.name.ToLower().Contains("volumetricflaskwraplabel45ml"))
        {
            flask45inCabinet = true;
        }
        if (other.name.ToLower().Contains("volumetricflaskwraplabel55ml"))
        {
            flask55inCabinet = true;
        }
        if (other.name.ToLower().Contains("volumetricflaskwraplabel65ml"))
        {
            flask65inCabinet = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BeakerinCabinet && GraduatedCylinderinCabinet && flask45inCabinet && flask55inCabinet && flask65inCabinet )
        {
            GameEventsManager.instance.miscEvents.ObjsPutAway();
        }
    }
}
