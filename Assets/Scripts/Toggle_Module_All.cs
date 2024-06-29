using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle_Module_All : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject gloveHygiene;
    public GameObject glasswareUse;
    public GameObject chemicalChange;
    public GameObject current;

    public void TutorialPrep()
    {
        current = tutorial;
    }
    public void GloveHygienePrep()
    {
        current = gloveHygiene;
    }
    public void GlasswareUsePrep()
    {
        current = glasswareUse;
    }
    public void ChemicalChangePrep()
    {
        current = chemicalChange;
    }

    public void UnPrep()
    {
        current = null;
    }
    public void Show()
    {
        if (current != null) current.GetComponent<Start_Module>().Show();
    }
}
