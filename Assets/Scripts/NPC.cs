using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    private GameObject person;
    private GameObject[] locations;
    private int nextLocation;

    void Start()
    {
        nextLocation = 0;
        locations = new GameObject[this.transform.childCount - 1];
        person = this.transform.GetChild(0).gameObject;

        for(int i = 0; i < locations.Length; i++)
        {
            locations[i] = this.transform.GetChild(i + 1).gameObject;
        }
        person.transform.position = locations[0].transform.position;
        StartCoroutine(Move());
    }
   private IEnumerator Move()
    {
        person.transform.LookAt(locations[nextLocation].transform.position);
        while(Vector3.Distance(locations[nextLocation].transform.position, person.transform.position) > .5)
        {
            person.transform.position += person.transform.TransformDirection(new Vector3(0,0,.025f));
            yield return null;
        }
        
        if(nextLocation + 1 == locations.Length)
        {
            nextLocation = 0;
        }
        else
        {
            nextLocation++;
        }
        StartCoroutine(Move());
    }
    void Update()
    {
 
    }
}
