using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingValveController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject valve;
    public GameObject optionalSecondValve;
    private PourActivator pA;
    private bool useTwoValves;
    void Start()
    {
        useTwoValves = false;
        pA = this.gameObject.GetComponent<PourActivator>();
        if (optionalSecondValve != null)
        {
            useTwoValves = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (useTwoValves)
        {
            CalculateFlow(valve.transform.eulerAngles.x, optionalSecondValve.transform.eulerAngles.x); //sink valves rotate on y
        }
        else
        {
            CalculateFlow(valve.transform.localEulerAngles.z); //burette valve rotates on z
        }
    }

    public void CalculateFlow(float rotation)
    {
        float normRotation = rotation % 180;
        if (normRotation < 15 || normRotation > 165) // Valve is closed
        {
            pA.Deactivate();
        }
        else
        {
            pA.Activate();
            if (normRotation > 90)
            {
                normRotation = Mathf.Abs(normRotation - 180);
            }
            float flow = (normRotation - 15) / 75;
            pA.setFlow(flow);
        }
    }

    private void CalculateFlow(float rotation1, float rotation2)
    {
        float normRotation1 = (rotation1 + 90) % 360;
        float normRotation2 = (rotation2 + 90) % 360;

        int startLimit = 15;
        if ((normRotation1 < startLimit) && (normRotation2 < startLimit)) // Both valves are closed
        {
            pA.Deactivate();
        }
        else
        {
            pA.Activate();
            float normRotation = 0;
            if (normRotation1 > startLimit && normRotation2 > startLimit)
            {
                normRotation = ((normRotation1 - startLimit) + (normRotation2 - startLimit)) / 2;
            }
            else if (normRotation1 > startLimit)
            {
                normRotation = (normRotation1 - startLimit) / 2;
            }
            else if (normRotation2 > startLimit)
            {
                normRotation = (normRotation2 - startLimit) / 2;
            }
            float flow = (normRotation) / (180 - startLimit);
            pA.setFlow(flow);
        }
    }
}
