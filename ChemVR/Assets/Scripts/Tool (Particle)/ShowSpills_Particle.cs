using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpills_Particle : MonoBehaviour
{
    private List<ParticleSystem.EmitParams> emitLocations;
    private ParticleSystem particleLauncher;
    private int counter;
    void Start()
    {
        particleLauncher = this.GetComponent<ParticleSystem>();
        emitLocations = new List<ParticleSystem.EmitParams>();
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            StartCoroutine(showChems());
        }
    }

    private IEnumerator showChems()
    {
        while (counter < emitLocations.Count)
        {
            particleLauncher.Emit(emitLocations[counter], 1);
            counter++;
            yield return new WaitForSeconds(.0125f);
        }
    }
    public void AddPosition(ParticleSystem.EmitParams pos)
    {
        emitLocations.Add(pos);
    }

    public void AddPosition(Vector3 pos)
    {
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = pos;
        emitLocations.Add(emitParams);
    }

}
