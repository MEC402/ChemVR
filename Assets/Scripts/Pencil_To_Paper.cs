using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil_To_Paper : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.ToLower().Contains("pencil"))
        {
            GameEventsManager.instance.miscEvents.PencilOnPaper();
        }
    }
}
