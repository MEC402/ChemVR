using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierBox : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.ToLower().Contains("cover") || collision.gameObject.name.ToLower().Contains("base") 
            || collision.gameObject.name.ToLower().Contains("floor") || collision.gameObject.name.ToLower().Contains("titration_holder") 
            || collision.gameObject.name.ToLower().Contains("doors") || collision.gameObject.name.ToLower().Contains("modular_kitchen_table")
            || collision.gameObject.name.ToLower().Contains("drying_rack") || collision.gameObject.name.ToLower().Contains("trash"))
        {
            return;
        }
        else
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }
}
