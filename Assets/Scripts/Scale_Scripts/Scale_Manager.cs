using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scale_Manager : MonoBehaviour
{
    public TextMeshProUGUI screenText;
    public TextMeshProUGUI unitText;
    public Scale_Plate plate;

    private enum unit { grams, carats, ounces, pounds };
    private unit currentUnit = unit.grams;
    private float currentMassInGrams = 0;
    private float taredMassInGrams = 0;

    private bool power = false;

    // Update is called once per frame
    void Update()
    {
        if(power)
        {
            SetMass();
            SetUnit();
        } else
        {
            screenText.text = "";
            unitText.text = "";
        }
    }
    private void SetMass()
    {
        currentMassInGrams = plate.measuredWeight;
        if (currentUnit == unit.grams)
        {
            screenText.text = (currentMassInGrams - taredMassInGrams).ToString("F2");
        }
        else if (currentUnit == unit.carats)
        {
            screenText.text = ((currentMassInGrams - taredMassInGrams) * 5f).ToString("F2");
        }
        else if (currentUnit == unit.ounces)
        {
            screenText.text = ((currentMassInGrams - taredMassInGrams) * 0.035274f).ToString("F2");
        }
        else if (currentUnit == unit.pounds)
        {
            screenText.text = ((currentMassInGrams - taredMassInGrams) * 0.00220462).ToString("F2");
        }
    }
    private void SetUnit()
    {
        if (currentUnit == unit.grams)
        {
            unitText.text = "g";
        }
        else if (currentUnit == unit.carats)
        {
            unitText.text = "ct";
        }
        else if(currentUnit == unit.ounces)
        {
            unitText.text = "oz";
        }
        else if(currentUnit == unit.pounds)
        {
            unitText.text = "lb";
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
        if(power)
        {
            if (currentUnit == unit.grams)
            {
                currentUnit = unit.carats;
            }
            else if (currentUnit == unit.carats)
            {
                currentUnit = unit.ounces;
            }
            else if (currentUnit == unit.ounces)
            {
                currentUnit = unit.pounds;
            }
            else if (currentUnit == unit.pounds)
            {
                currentUnit = unit.grams;
            }
        }
    }
    public void pressTare()
    {
        taredMassInGrams = currentMassInGrams;
    }
}
