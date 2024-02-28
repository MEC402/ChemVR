using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillObject : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pourLocations;
    [SerializeField]
    private GameObject chemical;
    [SerializeField]
    private GameObject fill;
    [SerializeField]
    private float maxHeight;
    [SerializeField]
    private float startHeight;
    [SerializeField]
    [Tooltip("Sets how many drops it should take to fill the container")]
    private int numDrops;
   
    private GameObject pourPosition;
    private bool isFull;
    private bool isEmpty;
    private float curHeight;
    [SerializeField]
    private float fillRate;

   

    void Start()
    {
        pourPosition = pourLocations[0];
        startHeight = fill.transform.localPosition.y;
        curHeight = startHeight;
        isFull = false;
        isEmpty = true;

        maxHeight = (maxHeight + startHeight)/2;

        fillRate = Mathf.Abs(maxHeight - startHeight) / numDrops;
        maxHeight = fillRate * numDrops + startHeight;

        
    }

    void Update()
    {
        curHeight = fill.transform.localPosition.y;

        // If container is empty, stop draining, hide fill, reset fill position.
        if (curHeight <= startHeight)
        {
            isEmpty = true;
            fill.GetComponent<MeshRenderer>().enabled = false;
            fill.transform.localPosition = new Vector3(0, startHeight, 0);
        }

        //If container is full, stop filling, spill rest.
        if (transform.up.y < -.2f && !isEmpty)
        {
            GameObject spill = Instantiate(chemical, pourPosition.transform.position, new Quaternion());
            spill.GetComponent<Renderer>().material = fill.GetComponent<Renderer>().material;
            fill.transform.localPosition -= new Vector3(0, fillRate, 0);
            fill.transform.localScale -= new Vector3(0, fillRate, 0);
        }

        if(curHeight < maxHeight)
        {
            isFull = false;
        }

        if (curHeight >= maxHeight)
        {
            isFull = true;
        }


        //Calculate pour location.
        foreach (GameObject location in pourLocations)
        {
            if (location.transform.position.y < pourPosition.transform.position.y)
            {
                pourPosition = location;
            }
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Chem"))
        {
            // Pour extra chemicals out.
            if (isFull)
            {
                GameObject spill = Instantiate(chemical, pourPosition.transform.position, new Quaternion());
                fill.GetComponent<Renderer>().material.color = Color.Lerp(fill.GetComponent<Renderer>().material.color, col.GetComponent<Renderer>().material.color, .02f);
                spill.GetComponent<Renderer>().material = fill.GetComponent<Renderer>().material;
            }

            // Destroy chemical, raise fill.
            else
            {
                isEmpty = false;
                if(fill.GetComponent<MeshRenderer>().enabled == false)
                {
                    fill.GetComponent<Renderer>().material.color = col.GetComponent<Renderer>().material.color;
                    fill.GetComponent<MeshRenderer>().enabled = true;
                }
                fill.transform.localPosition += new Vector3(0, fillRate, 0);
                fill.transform.localScale += new Vector3(0, fillRate, 0);
                fill.transform.position.Scale(new Vector3(0, fillRate, 0));
                fill.GetComponent<Renderer>().material.color = Color.Lerp(fill.GetComponent<Renderer>().material.color, col.GetComponent<Renderer>().material.color,  .02f);
            }

            Destroy(col.gameObject);
        }
    }
}



