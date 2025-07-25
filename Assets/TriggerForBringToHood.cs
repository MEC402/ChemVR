using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForBringToHood : MonoBehaviour
{
    //  /*11*/"Bring the copper sulfate and DI water back to your hood, and then open the hood and lower it to a working height.\n\n\nSkip with A",
    // Start is called before the first frame update
    private int objsInHood = 0;
    private bool isBeakerinHood = false;
    private bool isGraduatedCylinderinHood = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower().Contains("beakerup250ml") && !isBeakerinHood)
        {
            objsInHood++;
            isBeakerinHood=true;
            Debug.Log("beaker in hood::" +  objsInHood);
        }
        if (other.name.ToLower().Contains("graudatedcylinder") && !isGraduatedCylinderinHood)
        {
            objsInHood++;
            isGraduatedCylinderinHood=true;
            Debug.Log("beaker in hood::" + objsInHood);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(objsInHood >= 2)
        {
            GameEventsManager.instance.miscEvents.ObjectsBroughtToHood();
            Debug.Log("Objs in hood");
        }
    }
}
