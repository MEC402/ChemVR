using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat_Controller : MonoBehaviour, IDial
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField, Range(0, 100)] private float tintLevel;
    Color redTint = new Color(203f / 255f, 67f / 255f, 53f / 255f);

    float tintPrecentage;
    public void DialChanged(float dialvalue)
    {
        Debug.Log("Dial value: " + dialvalue);
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
}
