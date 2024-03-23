using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillObject_Particle : MonoBehaviour
{
    
   
    [SerializeField]
    private GameObject[] pourLocations;
    [SerializeField]
    private GameObject chemical;
    [SerializeField]
    private GameObject fill;
    [SerializeField]
    private float maxHeight;
    [SerializeField]
    private float startHeight;
    [SerializeField]
    private int numDrops;

    private List<ParticleCollisionEvent> collisionEvents;    
    private ParticleSystem particleLauncher;
    private ParticleSystem spillLocation;
    private GameObject pourPosition;
    private float curHeight;
    private float fillRate;
    private bool isEmpty;
    private bool isFull;
    private MeshRenderer fillMeshRenderer;
    private Renderer fillRenderer;
    private Renderer psRenderer;
    private void Start()
    {
        isFull = false;
        isEmpty = true;
        curHeight = startHeight;
        pourPosition = pourLocations[0];
        particleLauncher = this.GetComponent<ParticleSystem>();
        startHeight = fill.transform.localPosition.y;
        collisionEvents = new List<ParticleCollisionEvent>();
        spillLocation = GameObject.FindGameObjectWithTag("ParticleHolder").GetComponent<ParticleSystem>();
        fillMeshRenderer = fill.GetComponent<MeshRenderer>();
        fillRenderer = fill.GetComponent<Renderer>();


        maxHeight = (maxHeight + startHeight) / 2;
        fillRate = Mathf.Abs(maxHeight - startHeight) / numDrops;
        maxHeight = fillRate * numDrops + startHeight;
    }

    private void Update()
    {
        curHeight = fill.transform.localPosition.y;

        // If container is empty, stop draining, hide fill, reset fill position.
        if (curHeight <= startHeight)
        {
            isEmpty = true;
            fillMeshRenderer.enabled = false;
            fill.transform.localPosition = new Vector3(0, startHeight, 0);
        }

        //If container is full, stop filling, spill rest.
        if (this.transform.parent.transform.up.y < -.2f && !isEmpty)
        {
            this.SpillChem();
            fill.transform.localPosition -= new Vector3(0, fillRate, 0);
            fill.transform.localScale -= new Vector3(0, fillRate, 0);
        }

        if (curHeight < maxHeight)
        {
            isFull = false;
        }

        if (curHeight >= maxHeight)
        {
            isFull = true;
        }

        //Calculate pour location.
        foreach (GameObject location in pourLocations)
        {
            if (location.transform.position.y < pourPosition.transform.position.y)
            {
                pourPosition = location;
            }
        }
    }

    public void FillContainer(ParticleSystem ps)
    {
        psRenderer = ps.gameObject.GetComponent<Renderer>();
        if (isFull)
        {
            fillRenderer.material.color = Color.Lerp(fillRenderer.material.color, psRenderer.material.color, .025f);
            this.SpillChem();
        }

        // Destroy chemical, raise fill.
        else
        {
            if(fillMeshRenderer.enabled == false)
            {
                fillRenderer.material.color = psRenderer.material.color;
                fillMeshRenderer.enabled = true;
            }
            isEmpty = false;
            fillMeshRenderer.enabled = true;
            fill.transform.localPosition += new Vector3(0, fillRate, 0);
            fill.transform.localScale += new Vector3(0, fillRate, 0);
            fill.transform.position.Scale(new Vector3(0, fillRate, 0));
            fillRenderer.material.color = Color.Lerp(fillRenderer.material.color, psRenderer.material.color, .025f);
        }
    }

    public void UnFillContainer()
    {
        fill.transform.localPosition -= new Vector3(0, fillRate, 0);
        fill.transform.localScale -= new Vector3(0, fillRate, 0);
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            if (other.tag.Equals("Tool"))
            {
                for(int j = 0; j < other.transform.childCount; j++)
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

    private void SpillChem()
    {
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        particleLauncher.GetComponent<Renderer>().material = fill.GetComponent<Renderer>().material;
        emitParams.position = pourPosition.transform.position;
        particleLauncher.Emit(emitParams, 1);
    }

}
