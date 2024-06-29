using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingValveController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject valve;
    private PourActivator pA;
    void Start()
    {
        pA = this.gameObject.GetComponent<PourActivator>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFlow(valve.transform.localEulerAngles.z);
    }

    private void CalculateFlow(float rotation)
    {
        float normRotation = rotation % 180;
        if(normRotation < 15 || normRotation > 165) // Valve is closed
        {
            pA.Deactivate();
        } else
        {
            pA.Activate();
            if(normRotation > 90)
            {
                normRotation = Mathf.Abs(normRotation - 180);
            }
            float flow = (normRotation - 15) / 75;
            pA.setFlow(flow);
        }
    }
}
