using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flask_Particle : MonoBehaviour
{
    private ParticleSystem spillLocation;
    private List<ParticleCollisionEvent> collisionEvents;
    private ParticleSystem particleLauncher;
    private bool isPouring;
    private void Start()
    {
        spillLocation = GameObject.FindGameObjectWithTag("ParticleHolder").GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        particleLauncher = this.GetComponent<ParticleSystem>();
        isPouring = false;
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
            else if (other.tag.Equals("Funnel"))
            {
                Debug.Log("Funnel");
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
                spillLocation.GetComponent<ShowSpills_Particle>().AddPosition(emitParam);
            }
        }
    }

    private void Update()
    {
        if (this.transform.parent.transform.up.y < -.2f)
        {
            if (!isPouring)
            {
                isPouring = true;
                StartCoroutine(Spill());
            }
            
        }
        else
        {
            StopAllCoroutines();
            isPouring = false;
        }
    }

    private IEnumerator Spill()
    {
        particleLauncher.Emit(1);
        yield return new WaitForSecondsRealtime(.5f);
        StartCoroutine(Spill());
    }
}
