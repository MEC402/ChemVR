using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrubWithBrushObj : MonoBehaviour
{
    private bool flask45scrubbed = false;
    private bool flask55scrubbed = false;
    private bool flask65scrubbed = false;
    private bool graduatedCylinderscrubbed = false;
    private bool beakerscrubbed = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower().Contains("beaker"))
        {
            beakerscrubbed = true;
        }
        if (other.name.ToLower().Contains("graduated"))
        {
            graduatedCylinderscrubbed = true;
        }
        if (other.name.ToLower().Contains("volumetricflaskwraplabel45ml"))
        {
            flask45scrubbed = true;
        }
        if (other.name.ToLower().Contains("volumetricflaskwraplabel55ml"))
        {
            flask55scrubbed = true;
        }
        if (other.name.ToLower().Contains("volumetricflaskwraplabel65ml"))
        {
            flask65scrubbed = true;
        }
    }
    private void Update()
    {
        if (flask45scrubbed && flask55scrubbed && flask65scrubbed && graduatedCylinderscrubbed && beakerscrubbed)
        {
            GameEventsManager.instance.miscEvents.ScrubWithBrush();
        }
    }
}
