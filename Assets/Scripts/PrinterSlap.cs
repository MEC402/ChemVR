using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterSlap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("hand"))
        {
            GameEventsManager.instance.miscEvents.PrinterSlap();
        }
    }



}
