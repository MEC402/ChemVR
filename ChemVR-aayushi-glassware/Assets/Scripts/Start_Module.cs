using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Module : MonoBehaviour
{
    [SerializeField]
    public GameObject myGameObject;
    public void Show()
    {
        myGameObject.SetActive(true);
    }
    public void Hide()
    {
        myGameObject.SetActive(false);
    }
}
