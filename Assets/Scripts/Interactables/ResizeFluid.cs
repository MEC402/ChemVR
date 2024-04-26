using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeFluid : MonoBehaviour
{
    public float maxCapacity;
    public float currentVolume;
    private Vector3 scaleAtFull;
    private Vector3 defaultPosition;    

    // Start is called before the first frame update
    void Start()
    {
        scaleAtFull = transform.localScale;
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float percentFilled = Mathf.Clamp(currentVolume / maxCapacity, 0, 1);
        if (percentFilled == 0)
        {
            currentVolume = 0;
        } else if (percentFilled == 1)
        {
            currentVolume = maxCapacity;
        }
        float delta = scaleAtFull.y * (1  - percentFilled);
        Vector3 scaleChange = Vector3.up * delta;
        Vector3 positionChange = Vector3.up * delta;
        transform.localScale = scaleAtFull - scaleChange;
        transform.localPosition = defaultPosition - positionChange; 
        
    }
}
