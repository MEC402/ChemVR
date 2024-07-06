using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil_To_Paper : MonoBehaviour
{
    private void OnTriggerEnter(Collider obj)
    {
        Debug.Log(obj.name);
        if (obj.name.ToLower().Contains("pencil"))
        {
            Debug.Log("Pencil to paper");
            GameEventsManager.instance.miscEvents.PencilOnPaper();
        }
    }
}
