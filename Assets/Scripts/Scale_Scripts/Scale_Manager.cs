using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scale_Manager : MonoBehaviour
{
    public TextMeshProUGUI screenText;
    public TextMeshProUGUI unitText;
    public Scale_Plate plate;

    private enum unit { milligrams, grams, carats, ounces, pounds };
    private unit currentUnit = unit.milligrams;
    private float currentMassInGrams = 0;
    private float taredMassInGrams = 0;

    private bool power = false;

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOn += pressOn;
        GameEventsManager.instance.miscEvents.OnScalePowerOff += pressOff;
        GameEventsManager.instance.miscEvents.OnScaleMode += pressMode;
        GameEventsManager.instance.miscEvents.OnScaleTare += pressTare;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScalePowerOn -= pressOn;
        GameEventsManager.instance.miscEvents.OnScalePowerOff -= pressOff;
        GameEventsManager.instance.miscEvents.OnScaleMode -= pressMode;
        GameEventsManager.instance.miscEvents.OnScaleTare -= pressTare;
    }

    // Update is called once per frame
    void Update()
    {
        if (power)
        {
            SetMass();
            SetUnit();
        }
        else
        {
            screenText.text = "";
            unitText.text = "";
        }
    }

    private void SetMass()
    {
        currentMassInGrams = plate.measuredWeight;
        // if (currentUnit == unit.milligrams)
        // {
        //     screenText.text = ((currentMassInGrams - taredMassInGrams) * 1000).ToString("F2");
        // }
        // else if (currentUnit == unit.grams)
        // {
        //     screenText.text = (currentMassInGrams - taredMassInGrams).ToString("F2");
        // }
        // else if (currentUnit == unit.carats)
        // {
        //     screenText.text = ((currentMassInGrams - taredMassInGrams) * 5f).ToString("F2");
        // }
        // else if (currentUnit == unit.ounces)
        // {
        //     screenText.text = ((currentMassInGrams - taredMassInGrams) * 0.035274f).ToString("F2");
        // }
        // else if (currentUnit == unit.pounds)
        // {
        //     screenText.text = ((currentMassInGrams - taredMassInGrams) * 0.00220462).ToString("F2");
        // }

        switch (currentUnit)
        {
            case unit.milligrams:
                screenText.text = ((currentMassInGrams - taredMassInGrams) * 1000).ToString("F2");
                break;
            case unit.grams:
                screenText.text = (currentMassInGrams - taredMassInGrams).ToString("F2");
                break;
            case unit.carats:
                screenText.text = ((currentMassInGrams - taredMassInGrams) * 5f).ToString("F2");
                break;
            case unit.ounces:
                screenText.text = ((currentMassInGrams - taredMassInGrams) * 0.035274f).ToString("F2");
                break;
            case unit.pounds:
                screenText.text = ((currentMassInGrams - taredMassInGrams) * 0.00220462).ToString("F2");
                break;
            default:
                break;
        }
    }

    private void SetUnit()
    {
        // if (currentUnit == unit.milligrams)
        // {
        //     unitText.text = "mg";
        // }
        // else if (currentUnit == unit.grams)
        // {
        //     unitText.text = "g";
        // }
        // else if (currentUnit == unit.carats)
        // {
        //     unitText.text = "ct";
        // }
        // else if (currentUnit == unit.ounces)
        // {
        //     unitText.text = "oz";
        // }
        // else if (currentUnit == unit.pounds)
        // {
        //     unitText.text = "lb";
        // }

        switch (currentUnit)
        {
            case unit.milligrams:
                unitText.text = "mg";
                break;
            case unit.grams:
                unitText.text = "g";
                break;
            case unit.carats:
                unitText.text = "ct";
                break;
            case unit.ounces:
                unitText.text = "oz";
                break;
            case unit.pounds:
                unitText.text = "lb";
                break;
            default:
                break;
        }
    }

    public void pressOn()
    {
        power = true;
        // pressTare(); // a lot of scales start at zero despite weight
    }

    public void pressOff()
    {
        power = false;
        currentUnit = unit.grams;
        currentMassInGrams = 0;
    }

    public void pressMode()
    {
        // if (power)
        // {
        //     if (currentUnit == unit.milligrams)
        //         currentUnit = unit.grams;
        //     else if (currentUnit == unit.grams)
        //         currentUnit = unit.carats;
        //     else if (currentUnit == unit.carats)
        //         currentUnit = unit.ounces;
        //     else if (currentUnit == unit.ounces)
        //         currentUnit = unit.pounds;
        //     else if (currentUnit == unit.pounds)
        //         currentUnit = unit.milligrams;


        // }

        if (!power) return;

        switch (currentUnit)
        {
            case unit.milligrams:
                currentUnit = unit.grams;
                break;
            case unit.grams:
                currentUnit = unit.carats;
                break;
            case unit.carats:
                currentUnit = unit.ounces;
                break;
            case unit.ounces:
                currentUnit = unit.pounds;
                break;
            case unit.pounds:
                currentUnit = unit.milligrams;
                break;
            default:
                break;
        }
    }

    public void pressTare()
    {
        taredMassInGrams = currentMassInGrams;
    }
}
