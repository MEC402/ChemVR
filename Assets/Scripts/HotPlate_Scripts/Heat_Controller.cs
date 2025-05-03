using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Heat_Controller : MonoBehaviour, IDial
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField, Range(0, 100)] private float tintLevel;
    Color redTint = new Color(203f / 255f, 67f / 255f, 53f / 255f);
    private ChemContainer chemContainerScript;

    float tintPrecentage;
    float BoilRate = 0.02f;
    public void DialChanged(float dialvalue)
    {
        //Debug.Log("Dial value: " + dialvalue);
        //tintPrecentage = dialvalue;
        tintPrecentage = (dialvalue / 360f) * 100;
        // Debug.Log("tintPrecentage: " + tintPrecentage);
        tintLevel = tintPrecentage;
    }

    private void Update()
    {
        //Color tintedColor = Color.Lerp(Color.white, Color.red, tintLevel / 100f); //calculate the red tint
        Color tintedColor = Color.Lerp(Color.white, redTint, tintLevel / 100f); //calculate the red tint
        targetRenderer.material.color = tintedColor; //apply tint to material
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with: " + other.gameObject.name + " | Tag: " + other.tag);
        //Debug.Log("OnCollisionEnter");
        if (other.gameObject.CompareTag("CanBoil"))
        {
            chemContainerScript = other.gameObject.GetComponent<ChemContainer>();
            Debug.Log("ChemContainerSet. " + chemContainerScript);
        }
        else
        {
            chemContainerScript = null;
            Debug.Log("the other object cannot boil. script set to null");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnCollisionExit");
        chemContainerScript = null;
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("OnCollisionStay");
        float startingVolume;
        float lossPerSecond = (tintPrecentage * BoilRate);

        if (chemContainerScript != null)
        {
            startingVolume = chemContainerScript.currentVolume;

            startingVolume -= lossPerSecond * Time.deltaTime;
            chemContainerScript.currentVolume = startingVolume;

            Debug.Log("Current volume: " + startingVolume);
            //Debug.Log("tintPrecentage: " + tintPrecentage);
            Debug.Log("boil rate after multiply: " + lossPerSecond);

        }
        else
        {
           // Debug.Log("ChemContainerScript is Null.");
        }
    }
}
