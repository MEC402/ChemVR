using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChemistryManager : MonoBehaviour
{
    public static ChemistryManager instance;

    [SerializeField]
    public float transparency = .5f;
    public bool isASolid;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one ChemistryManager was found in the scene.");
        }
        instance = this;
    }

    // This script will manage chemical reactions when different Chems are mixed within a ChemContainer
    // Example: Hydrochloric Acid and Sodium Hydroxide combine to form Salt (NaCl) and Water. Actual amount
    // depends on the concentration of the acid and base solutions.
    // This script doesn't actually do that yet, but it will. Later.

    /// <summary>
    /// Gets the color corresponding to a single ChemType
    /// </summary>
    /// <param name="chemType">ChemType to get the color of</param>
    /// <returns>Color for the given ChemType</returns>
    public Color GetColor(ChemType chemType)
    {
        switch (chemType)
        {
            case ChemType.HYDROGEN_PEROXIDE:
                return Color.clear;
                //return new Color32(0, 128, 128, 5);
            case ChemType.WATER:
                return Color.cyan;
            case ChemType.HYDROCHLORIC_ACID:
                return Color.grey;
            case ChemType.SODIUM_HYDROXIDE:
                return Color.yellow;
            case ChemType.COPPER_SULFATE:
                return Color.blue;
            case ChemType.GLUCOSE:
                return Color.white;
            case ChemType.SOLID_SUGAR:
                isASolid = true;
                return Color.white;
            case ChemType.SOLID_CHROMIUMIIICHLORIDE:
                isASolid = true;
                return Color.green;
            case ChemType.CHROMIUM_III_CHLORIDE:
                return Color.black;
               // return new Color32(0, 15, 2, 0);
            default:
                return Color.black;
        }
    }

    /// <summary>
    /// Produces a combined Color based on the corresponding Colors and amounts for all chems in the ChemFluid.
    /// </summary>
    /// <remarks>
    /// Each ChemType contributes a percentage of its pure color value according to what percentage that type 
    /// contributes to the overall volume of the ChemFluid.
    /// </remarks>
    /// <param name="chemFluid"></param>
    /// <returns></returns>
    public Color GetColor(ChemFluid chemFluid)
    {
        Color chemColor = new Color();
        foreach (Chem chem in chemFluid.GetChems())
        {
            chemColor = chemColor + (GetColor(chem.type) * (chem.volume / chemFluid.totalVolume));
        }
        if (isASolid)
        {
            transparency = 1f;
        }
        else
        {
            transparency = 0.5f;
        }
        chemColor.a = chemColor.a * transparency;
        return chemColor;
    }


}
