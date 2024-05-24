using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spill : MonoBehaviour
{
    public const float minScale = 0.01f;    
    public float currentVolume;

    public int ticks; // number of physics engine ticks elapsed
    public int maxTicksToFill = 25; // max ticks to reach minVolume before object is destroyed
    public const float minVolume = Mathf.PI;

    // The square root of (pi * 10^4), used in computing spill radius given chemFluid volume in milliliters
    private const float SQRT_10K_PI = 0.00564189583548f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        transform.localScale = new Vector3(minScale, 0.01f, minScale);
        ticks = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentVolume >= minVolume) {
            GetComponent<MeshRenderer>().enabled = true;
            float radius = Mathf.Sqrt(currentVolume) * SQRT_10K_PI;
            transform.localScale = new Vector3(2 * radius, 0.01f, 2 * radius);
        } else {
            ticks++;
            if (ticks >= maxTicksToFill)
            {
                Destroy(this);
            }
        }        
    }

    public void PourIn(float amount)
    {
        currentVolume += amount;
    }
}
