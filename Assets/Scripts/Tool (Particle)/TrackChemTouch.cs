using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackChemTouch : MonoBehaviour
{
    [SerializeField]
    private GameObject chemical;
    private ShowSpills_Particle spillLocation;

    void Start() {
        spillLocation = GameObject.FindGameObjectWithTag("ParticleHolder").GetComponent<ParticleSystem>().GetComponent<ShowSpills_Particle>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 10)
        {
            spillLocation.AddPosition(this.transform.position);
        }
        else if (col.gameObject.layer == 12 || col.tag.Equals("Tools"))
        {
            for (int i = 0; i < col.transform.childCount; i++)
            {
                if (col.transform.GetChild(i).name.Equals("ChemMarkerHolder"))
                {
                    GameObject chem = Instantiate(chemical, this.transform.position, new Quaternion());
                    chem.transform.parent = col.transform.GetChild(i);
                }
            }
        }
    }
}
