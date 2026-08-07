
using UnityEngine;
using System;
[Serializable]
public class ObjectState
{
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public bool isChemContainer;
    public ChemFluid currentFluid;
}
