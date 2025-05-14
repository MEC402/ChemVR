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
    public ParticleSystem steamEffect;
    public bool canSteam;
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
            if (chemContainerScript != null)
            {
                startingVolume = chemContainerScript.currentVolume;

                startingVolume -= lossPerSecond * Time.deltaTime;
                chemContainerScript.currentVolume = startingVolume;


                if (chemContainerScript.currentVolume <= 0)
                {
                    chemContainerScript.currentVolume = 0;
                    stopParticleEffect();
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
