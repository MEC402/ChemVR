using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitrationSpill_Particle : MonoBehaviour
{
    [SerializeField]
    private GameObject spillMarkerTop;
    [SerializeField]
    private GameObject upperBound;
    [SerializeField]
    private GameObject lowerBound;
    [SerializeField]
    private GameObject fill;

    private ParticleSystem particleLauncher;
    private bool isSpilling;
    private float upperBoundNum;
    private float lowerBoundNum;
    void Start()
    {
        upperBoundNum = upperBound.transform.position.y;
        lowerBoundNum = lowerBound.transform.position.y;
        particleLauncher = this.GetComponent<ParticleSystem>();
        isSpilling = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (spillMarkerTop.transform.position.y < upperBoundNum && spillMarkerTop.transform.position.y > lowerBoundNum)
        {
           
            if (!isSpilling)
            {
                isSpilling = true;
                StartCoroutine(Spill());
            }
        }
        else
        {
            StopAllCoroutines();
            isSpilling = false;
        }
    }

    private IEnumerator Spill()
    {
        while (particleLauncher.GetComponent<FillObject_Particle>().IsEmpty() == false)
        {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            particleLauncher.GetComponent<Renderer>().material = fill.GetComponent<Renderer>().material;
            emitParams.position = particleLauncher.transform.position;
            particleLauncher.Emit(emitParams, 1);
            particleLauncher.GetComponent<FillObject_Particle>().UnFillContainer();
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
