using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Chem
{
    [HideInInspector, SerializeField]
    public string name;
    [SerializeField]
    public ChemType type;
    [SerializeField]
    public float volume;

    public Chem(ChemType type, float volume)
    {
        this.type = type;
        this.volume = volume;
        this.name = type.ToString();
    }
}
