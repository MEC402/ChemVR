using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scale_Plate : MonoBehaviour
{
    //NOTE TO SELF: Consider changing collider to a very tall collider from the plate and up! so that anything stacked would count towards the weight as well.
    public float measuredWeight = 0;
    private HashSet<GameObject> ObjectsOnScale = new HashSet<GameObject>();

    private void Update()
    {
        measuredWeight = 0;
        foreach (GameObject item in ObjectsOnScale)
        {
            Rigidbody weightRB = item.GetComponent<Rigidbody>();
            if (weightRB != null)
            {
                Debug.Log("Before: " + measuredWeight);
                measuredWeight += (weightRB.mass + getFluidMass(item));
                Debug.Log("After: " + measuredWeight);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!ObjectsOnScale.Contains(other.gameObject))
        {
            ObjectsOnScale.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (ObjectsOnScale.Contains(other.gameObject))
        {
            ObjectsOnScale.Remove(other.gameObject);
        }
    }

    private float getFluidMass(GameObject weight)
    {
        ChemContainer fluid = weight.GetComponent<ChemContainer>();
        float totalFluidInGrams = 0;
        if (fluid != null)
        {
            //Debug.Log(fluid.getContents());
            string contents = fluid.getContents();
            string[] sepContents = contents.Split('\n');
            foreach (string s in sepContents)
            {
                string[] type_to_Amount = s.Split(new[] { ": " }, StringSplitOptions.None);
                float unConvertedContents;
                if (type_to_Amount.Length > 1 && float.TryParse(type_to_Amount[1], out unConvertedContents))
                {
                    if (type_to_Amount[0].Contains("WATER")) //Water has a 1 g / mL density at room temperature
                    {
                        totalFluidInGrams += unConvertedContents;
                    }
                    else if (type_to_Amount[0].Contains("HYDROCHLORIC_ACID")) //Hydrochloric acid has a 1.2 g / mL density at room temperature 
                    {
                        totalFluidInGrams += (1.2f * unConvertedContents);
                    }
                    else if (type_to_Amount[0].Contains("SODIUM_HYDROXIDE")) //Sodium Hydroxide has a 1.36 g / mL density at room temperature 
                    {
                        totalFluidInGrams += (1.36f * unConvertedContents);
                    }
                }
                
            }

        }
        return totalFluidInGrams;
    }
}
