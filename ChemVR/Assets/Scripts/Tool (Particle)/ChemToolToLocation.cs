using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemToolToLocation : MonoBehaviour
{

    private ShowSpills_Particle spillLocation;
    // Start is called before the first frame update
    void Start()
    {
        spillLocation = GameObject.FindGameObjectWithTag("ParticleHolder").GetComponent<ParticleSystem>().GetComponent<ShowSpills_Particle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            spillLocation.AddPosition(this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
