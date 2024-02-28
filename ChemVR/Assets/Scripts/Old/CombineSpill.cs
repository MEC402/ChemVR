using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSpill : MonoBehaviour
{

    private int previousChildCount;

    void Start()
    {
        previousChildCount = 0;
    }
    void Update()
    {
        if (transform.childCount != previousChildCount)
        {

        }

        previousChildCount = transform.childCount;
    }
}

