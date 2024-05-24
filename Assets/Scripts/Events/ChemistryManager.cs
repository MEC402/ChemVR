using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChemistryManager : MonoBehaviour 
{
    public static ChemistryManager instance;

    public void Awake() {
        if (instance != null) {
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
    public Color GetColor(ChemType chemType) {
        switch (chemType) {
            case ChemType.WATER:
                return Color.cyan;
            case ChemType.HYDROCHLORIC_ACID:
                return Color.magenta;
            case ChemType.SODIUM_HYDROXIDE:
                return Color.yellow;
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
    public Color GetColor(ChemFluid chemFluid) {
        Color chemColor = new Color();
        foreach (Chem chem in chemFluid.GetChems()) {
            chemColor = chemColor + (GetColor(chem.type) * (chem.volume / chemFluid.totalVolume));
        }
        return chemColor;
    }


}
