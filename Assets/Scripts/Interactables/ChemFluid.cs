using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[System.Serializable]
public class ChemFluid 
{
    [SerializeField]
    public float totalVolume;
    [SerializeField]
    private Chem[] chems;
    
 
    public ChemFluid()
    {
        int numUniqueChemTypes = new HashSet<int>(Enum.GetValues(typeof(ChemType)).Cast<int>()).Count;
        this.chems = new Chem[numUniqueChemTypes];
        for (int i = 0; i < chems.Length; i++)
        {
            chems[i] = new Chem((ChemType)i, 0);
        }
        totalVolume = 0;        
    }

    public ChemFluid(Chem[] initialVolumes) : this()
    {
        for (int i = 0; i < initialVolumes.Length; i++)
        {
            chems[(int)initialVolumes[i].type].volume += initialVolumes[i].volume;
        }
        UpdateVolume();
    }

    /// <summary>
    /// Returns a read-only copy of the ChemFluid's chems array. Does not modify internal state.
    /// </summary>
    /// <returns>read-only copy of contents</returns>
    public ReadOnlyCollection<Chem> GetChems () {
        return Array.AsReadOnly<Chem>(chems);
    }

    /// <summary>
    /// Updates the total volume by summing all values in chems
    /// </summary>
    private void UpdateVolume()
    {
        this.totalVolume = 0;
        for (int i = 0; i < chems.Length; ++i)
        {
            this.totalVolume += chems[i].volume;
        }
        //this.totalVolume = chems.Sum();
    }

    /// <summary>
    /// Add an amount of one specific type of chem to the ChemFluid
    /// </summary>
    /// <param name="chemType">the type of chem to add</param>
    /// <param name="amount">the amount (in mL) to add</param>
    public void Add(ChemType chemType, float amount)
    {
        this.chems[(int)chemType].volume += amount;
        this.UpdateVolume();
    }

    /// <summary>
    /// Add an amount of one specific type of chem to the ChemFluid
    /// </summary>
    /// <param name="chem"></param>
    public void Add(Chem chem)
    {
        Add(chem.type, chem.volume);
    }

    /// <summary>
    /// Add multiple chems to the current ChemFluid by passing in another ChemFluid
    /// </summary>
    /// <param name="chemFluid">the ChemFluid to add to the current one</param>
    public void Add(ChemFluid chemFluid)
    {
        for (int i = 0; i < chems.Length; i++)
        {
            this.chems[i].volume += chemFluid.chems[i].volume;
        }
        this.UpdateVolume();
    }

    /// <summary>
    /// Remove an amount of one specific type of chem from the ChemFluid
    /// </summary>
    /// <param name="chemType"></param>
    /// <param name="amount"></param>
    public void Remove(ChemType chemType, float amount)
    {
        this.chems[(int)chemType].volume -= amount;
        this.UpdateVolume();
    }

    /// <summary>
    /// Remove multiple chems from the current ChemFluid
    /// </summary>
    /// <param name="chemFluid">the ChemFluid whose amounts to subtract from the current one</param>
    public void Remove(ChemFluid chemFluid)
    {
        for (int i = 0; i < chems.Length; i++)
        {
            this.chems[i].volume -= chemFluid.chems[i].volume;
        }
        this.UpdateVolume();
    }

    /// <summary>
    /// Sets all volumes to 0.
    /// </summary>
    public void SetToEmpty()
    {
        totalVolume = 0;
        for (int i = 0; i < chems.Length; ++i)
        {
            chems[i].volume = 0;
        }
    }

    /// <summary>
    /// Creates a ChemFluid from the current one based on a given volume (in mL). The new ChemFluid will 
    /// retain the same proportions of chems, scaled to match the new volume
    /// </summary>
    /// <param name="amount">between 0 and ChemFluid.totalVolume inclusive</param>
    /// <returns>a new, scaled ChemFluid</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the given amount is less than 0 or greater than total volume of ChemFluid</exception>
    public ChemFluid PortionFromVolume(float amount)
    {
        if (amount < 0 || amount > totalVolume)
        {
            throw new ArgumentOutOfRangeException();
        }
        return PortionFromPercent(amount / totalVolume);
    }

    /// <summary>
    /// Creates a ChemFluid from the current one based on a given percentage amount. The new ChemFluid will
    /// retain the same proportions fo chems, scaled by the percentage.
    /// </summary>
    /// <param name="percentage">between 0 and 1 inclusive</param>
    /// <returns>a new, scaled ChemFluid</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the given percentage is less than 0 or greater than 1</exception>
    public ChemFluid PortionFromPercent(float percentage)
    {
        if (percentage < 0 || percentage > 1)
        {
            throw new ArgumentOutOfRangeException();
        }
        ChemFluid portion = new ChemFluid();
        for (int i = 0; i < chems.Length; i++)
        {
            portion.chems[i].volume = this.chems[i].volume * percentage;
        }
        portion.UpdateVolume();
        return portion;
    }

    /// <summary>
    /// Produces a formatted string representing the ChemFluid's contents.
    /// Format is "{ChemType}: {amount}" on separate lines, with the total volume at the end.
    /// </summary>
    /// <returns>formatted string listing ChemFluid contents</returns>
    public string ContentsToString()
    {
        string message = "";
        for (int i = 0; i < chems.Length; i++)
        {
            message += $"{((ChemType)i).ToString()}: {chems[i].volume}\n";
        }
        message += $"TOTAL: {totalVolume}";
        return message; 
    }

}
