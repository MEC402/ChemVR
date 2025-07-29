using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForBringToHood : MonoBehaviour
{
    //  /*11*/"Bring the copper sulfate and DI water back to your hood, and then open the hood and lower it to a working height.\n\n\nSkip with A",
    // Start is called before the first frame update
    private bool BeakerinHood = false;
    private bool GraduatedCylinderinHood = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower().Contains("beakerup250ml"))
        {
            BeakerinHood = true;
        }
        if (other.name.ToLower().Contains("graudatedcylinder"))
        {
            GraduatedCylinderinHood = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GraduatedCylinderinHood && BeakerinHood)
        {
            GameEventsManager.instance.miscEvents.ObjectsBroughtToHood();
            Debug.Log("Objs in hood");
        }
    }
}
