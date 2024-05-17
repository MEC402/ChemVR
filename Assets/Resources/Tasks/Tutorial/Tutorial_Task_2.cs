using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Task_2 : MonoBehaviour
{
    [SerializeField]
    public GameObject currText; // Set this to the popup text for this step.
    void Awake()
    {
        currText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
