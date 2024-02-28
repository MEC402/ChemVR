using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemtrail : MonoBehaviour
{
    public GameObject chemTrail;
    private ArrayList chemLocations;
    void Start()
    {
        chemLocations = new ArrayList();
        chemTrail.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            foreach(Vector3 location in chemLocations)
            {
                GameObject trail = Instantiate(chemTrail, location, new Quaternion());
                trail.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
            }
            chemLocations = new ArrayList();
            Destroy(this.gameObject);
            Destroy(this);
        }

        if(this.transform.position.y < 1f)
        {
            Destroy(this.gameObject);
            Destroy(this);
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (!(col.gameObject.tag.Equals("Chem") || col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("Tool") || col.gameObject.tag.Equals("Spill")))
        {
            chemLocations.Add(this.transform.position);
        }
    }
}
