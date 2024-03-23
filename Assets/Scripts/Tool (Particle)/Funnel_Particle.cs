using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Funnel_Particle : MonoBehaviour
{
    private ParticleSystem spillLocation;
    private List<ParticleCollisionEvent> collisionEvents;
    private ParticleSystem particleLauncher;
    private void Start()
    {
        spillLocation = GameObject.FindGameObjectWithTag("ParticleHolder").GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        particleLauncher = this.GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            if (other.tag.Equals("Tool"))
            {
                for (int j = 0; j < other.transform.childCount; j++)
                {
                    if (other.transform.GetChild(j).name.Equals("Particle System"))
                    {
                        other.transform.GetChild(j).GetComponent<FillObject_Particle>().FillContainer(particleLauncher);
                    }
                }
            }
            else if (other.tag.Equals("Player"))
            {
                for (int j = 0; j < other.transform.childCount; j++)
                {
                    if (other.transform.GetChild(j).name.Equals("ParticleDetection"))
                    {
                        other.transform.GetChild(j).GetComponent<HandDetection_Particle>().addChemContact(collisionEvents[i].intersection);
                    }
                }
            }
            else
            {
                ParticleSystem.EmitParams emitParam = new ParticleSystem.EmitParams();
                emitParam.position = collisionEvents[i].intersection;
                spillLocation.GetComponent<ShowSpills_Particle>().AddPosition(emitParam);
            }
        }
    }

    public void EmitParticle(Material mat)
    {
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        particleLauncher.GetComponent<Renderer>().material = mat;
        emitParams.position = particleLauncher.transform.position;
        particleLauncher.Emit(emitParams, 1);
    }
}
