using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table_Contents : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        GameObject thingOnTable = collision.gameObject;
        GameEventsManager.instance.miscEvents.ItemOnTable(thingOnTable, this.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        GameEventsManager.instance.miscEvents.ItemOffTable(collision.gameObject, this.gameObject);
    }
}
