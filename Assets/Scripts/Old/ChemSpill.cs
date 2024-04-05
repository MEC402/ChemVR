using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemSpill : MonoBehaviour
{
    public GameObject chemSpill;
    private GameObject spillHolder;

    void Start()
    {
        spillHolder = GameObject.FindGameObjectWithTag("SpillHolder");
    }
    void OnCollisionEnter(Collision col)
    {
        if (!(col.gameObject.tag.Equals("Chem")|| col.gameObject.tag.Equals("Tool") || col.gameObject.tag.Equals("Spill") || col.gameObject.tag.Equals("Fill")))
        {
            GameObject spill = Instantiate(chemSpill, this.transform.position, new Quaternion());
            spill.transform.SetParent(spillHolder.transform);
            spill.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        // Used to make spills pool together and increase in size.
        if (col.gameObject.tag.Equals("Spill"))
        {
            float offSet = Vector3.Distance(this.transform.position, col.transform.position);
            if (offSet < .03f)
            {
                Vector3 newPosition = new Vector3((col.transform.position.x + this.transform.position.x) / 2, col.transform.position.y, (col.transform.position.z + this.transform.position.z) / 2);
                Vector3 newScale = new Vector3(col.gameObject.transform.position.x + this.transform.position.x, 1, col.gameObject.transform.position.z + this.transform.position.z);
                Destroy(col.gameObject);

                if (newScale.x < .4f && newScale.y < 1f && newScale.z < .4f)
                {
                    GameObject spill = Instantiate(chemSpill, newPosition, new Quaternion());
                    spill.transform.localScale = newScale;
                    spill.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
                }
            }
            Destroy(this.gameObject);

        }

        if (col.gameObject.tag.Equals("Player"))
        {
            GameObject spill = Instantiate(chemSpill, this.transform.position, new Quaternion());
            spill.transform.SetParent(col.gameObject.transform);
            spill.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
            Destroy(this.gameObject);
        }
    }
}
