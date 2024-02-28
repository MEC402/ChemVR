using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Wander : MonoBehaviour
{
    private float xForce;
    private float zForce;
    void Start()
    {
        xForce = Random.Range(-.25f, .25f);
        zForce = Random.Range(-.25f, .25f);
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, 0, zForce));
    }

    void OnCollisionEnter()
    {
        xForce = Random.Range(-.25f, .25f);
        zForce = Random.Range(-.25f, .25f);
    }

    void OnCollisionStay()
    {
        xForce = xForce * -1;
        zForce = zForce * -1;
    }
}
