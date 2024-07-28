using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System;

public class Spawn_Paper_Towels : XRBaseInteractable
{
    private XRGrabInteractable grabInteractable;
    public GameObject individualPaperTowels;

    private void OnEnable()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();

        //Initialize grab interactable for listening
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }

        grabInteractable.selectEntered.AddListener(Spawn_A_Towel);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.AddListener(Spawn_A_Towel);
    }

    public void Spawn_A_Towel(SelectEnterEventArgs arg0)
    {
        Vector3 handLocation = arg0.interactorObject.transform.position;
        Vector3 towelLocation = arg0.interactableObject.transform.position;
        Vector3 location = new Vector3(handLocation.x, towelLocation.y, handLocation.z);
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        GameObject paperTowel = Instantiate(individualPaperTowels, location, rotation);

        
        // Get grab interactable from prefab
        XRGrabInteractable objectInteractable = paperTowel.GetComponent<XRGrabInteractable>();

        // Select object into same interactor
        interactionManager.SelectEnter(arg0.interactorObject, objectInteractable);
        
    }
}