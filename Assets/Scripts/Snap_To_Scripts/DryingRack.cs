using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryingRack : MonoBehaviour
{
    HashSet<GameObject> itemsOnRack = new HashSet<GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<MeshCollider>();
            child.gameObject.GetComponent<MeshCollider>().convex = true;
            child.gameObject.AddComponent<Rigidbody>();
            Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            child.gameObject.AddComponent<HangOnRack>();
            HangOnRack rack = child.GetComponent<HangOnRack>();
            rack.itemsOnRack = this.itemsOnRack;
        }
    }
}
