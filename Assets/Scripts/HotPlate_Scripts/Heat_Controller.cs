using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Heat_Controller : MonoBehaviour, IDial
{
    //please note that any object you want to be affected by the hot plate must have the tag "CanBoil" otherwise this script will not work.
    //also note that when you add the hotplate to the scene you need to assign the particle system for the steam in the inspector otherwise that will not work
    /*there is a known bug where after you boil the liquid out of the beaker and go to pour more into it it automaticlly returns to a volume equal
    to what was the beaker (or flask) before you boiled the liquid on the hot plate. I think this bug is on the chemContainer script but idk how to fix*/

    [SerializeField] private Renderer targetRenderer;
    [SerializeField, Range(0, 100)] private float tintLevel;
    Color redTint = new Color(203f / 255f, 67f / 255f, 53f / 255f);
    private ChemContainer chemContainerScript;
    public ParticleSystem steamEffect;
    public bool canSteam; //t/f is assignedd in the DialRotator script 
    public bool isSteamOn;

    float startingVolume;
    float tintPrecentage;
    float BoilRate = .1f;
    public void DialChanged(float dialvalue)
    {
        //tintPrecentage = dialvalue;
        tintPrecentage = (dialvalue / 360f) * 100;
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
        if (other.gameObject.CompareTag("CanBoil"))
        {
            Transform liquidOpeningTransform;
            chemContainerScript = other.gameObject.GetComponent<ChemContainer>();
            //get the transform location of the opening component of the glassware
            liquidOpeningTransform = other.transform.Find("Opening");
            //set the particle system for the steam location equal to where the opening is so it looks like the liquid is steaming
            steamEffect.transform.position = liquidOpeningTransform.position;

        }

        if(other.name.Contains("Beaker") && other.name.Contains("250"))
        {
            GameEventsManager.instance.miscEvents.BeakerOnHotPlate();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CanBoil"))
        {
            chemContainerScript = null;
            stopParticleEffect();
            //isSteamOn = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
        float lossPerSecond = (tintPrecentage * BoilRate);
        if (other.gameObject.CompareTag("CanBoil"))
        {
            if (canSteam)
            {
                playParticleEffect();
            }

            if (!canSteam)
            {
                stopParticleEffect();
            }
            if (chemContainerScript != null)
            {
                startingVolume = chemContainerScript.currentVolume;

                startingVolume -= lossPerSecond * Time.deltaTime;
                chemContainerScript.currentVolume = startingVolume;


                if (chemContainerScript.currentVolume <= 0)
                {
                    chemContainerScript.currentVolume = 0;
                    stopParticleEffect();
                    //chemContainerScript.chemFluid.totalVolume = 0;?
                }

            }
        }

    }

    private void playParticleEffect()
    {
        if (!isSteamOn)
        {
            steamEffect.Play();
            isSteamOn = true;
        }
    }

    private void stopParticleEffect()
    {
        if (isSteamOn)
        {
            steamEffect.Stop();
            isSteamOn = false;
        }
    }
}
