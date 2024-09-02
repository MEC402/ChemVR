using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HygieneTouchReporter : MonoBehaviour
{
    [SerializeField]
    CapsuleCollider capsule;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    HygieneManager hygieneManager;
    [SerializeField]
    Material gloveMaterial;

    SkinnedMeshRenderer smr;

    //Pasted in
   /*  bool wearingRightGlove;
    bool wearingLeftGlove;
    private void OnEnable()
    {

        GameEventsManager.instance.taskEvents.TaskStartApproved("Glove_Hygiene");
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("glovebox"));

        wearingLeftGlove = GameObject.Find("left hand model").GetComponent<SkinnedMeshRenderer>().material.name.ToLower().Contains("blue");
        wearingRightGlove = GameObject.Find("right hand model").GetComponent<SkinnedMeshRenderer>().material.name.ToLower().Contains("blue");
        GameEventsManager.instance.miscEvents.onPutOnRightGlove += RightGloveOn;
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove += LeftGloveOn;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove += RightGloveOff;
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove += LeftGloveOff;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPutOnRightGlove -= RightGloveOn;
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove -= LeftGloveOn;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove -= RightGloveOff;
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove -= LeftGloveOff;
    }*/
    //Pasted in

    private void OnValidate()
    {
        if (capsule == null)
        {
            capsule = GetComponent<CapsuleCollider>();
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (smr == null)
        {
            smr = GetComponent<SkinnedMeshRenderer>();
        }
        if (hygieneManager == null)
        {
            hygieneManager = FindFirstObjectByType<HygieneManager>();
        }
    }

    private void Awake()
    {
        if (capsule == null)
        {
            Debug.LogError($"No CapsuleCollider component assigned for {this.name}.");
        }
        else if (!capsule.isTrigger)
        {
            Debug.LogWarning($"{this.name}'s CapsuleCollider must be set to IsTrigger for hygiene touch to work.");
        }
        if (rb == null)
        {
            Debug.LogError($"No Rigidbody component assigned for {this.name}.");
        }
        else if (!rb.isKinematic)
        {
            Debug.LogWarning($"{this.name}'s Rigidbody must be set to IsKinematic for hygiene touch to work.");
        }
        if (gloveMaterial == null)
        {
            Debug.LogWarning($"No glove material set on object {this.name}. Please specify the glove material for proper glove on/off detection.");
        }
        if (hygieneManager == null)
        {
            Debug.LogError($"No HygieneManager assigned in HygieneTouchReporter on {this.name}.");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!smr.material.name.ToLower().Contains("blue"))
        {
            // Only track objects that are touched when gloves are on. Otherwise, just ignore the collision.
            return;
        }
        if (other.isTrigger)
        {
            // Also ignore other trigger colliders.
            return;
        }
        Vector3 touchPoint = other.ClosestPoint(this.transform.parent.position);

        hygieneManager.AddPoint(touchPoint, other.gameObject);
    }
}
