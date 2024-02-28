using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipette_Particle : MonoBehaviour
{
    
    private ShowSpills_Particle spillLocation;
    private List<ParticleCollisionEvent> collisionEvents;
    private ParticleSystem particleLauncher;
    private void Start()
    {
        spillLocation = GameObject.FindGameObjectWithTag("ParticleHolder").GetComponent<ParticleSystem>().GetComponent<ShowSpills_Particle>();
        collisionEvents = new List<ParticleCollisionEvent>();
        particleLauncher = this.GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);
        for(int i = 0; i < collisionEvents.Count; i++)
        {
            
            if (other.tag.Equals("Tool"))
            {
                if (other.transform.childCount != 0)
                {
                    for (int j = 0; j < other.transform.childCount; j++)
                    {
                        if (other.transform.GetChild(j).name.Equals("Particle System"))
                        {
                            other.transform.GetChild(j).GetComponent<FillObject_Particle>().FillContainer(particleLauncher);
                        }
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
            else if (other.tag.Equals("Funnel"))
            {
                for (int j = 0; j < other.transform.childCount; j++)
                {
                    if (other.transform.GetChild(j).name.Equals("Particle System"))
                    {
                        other.transform.GetChild(j).GetComponent<Funnel_Particle>().EmitParticle(this.GetComponent<Renderer>().material);
                    }
                }
            }
            else
            {
                ParticleSystem.EmitParams emitParam = new ParticleSystem.EmitParams();
                emitParam.position = collisionEvents[i].intersection;
                spillLocation.AddPosition(emitParam);
            }
        }
    }

    private void Update()
    {
        OVRInput.Update();
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && this.transform.parent.GetComponent<OVRGrabbable>().isGrabbed)
        {
            particleLauncher.Emit(1);
        }
    }
}
