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

    private void OnValidate() {
        if (capsule == null) {
            capsule = GetComponent<CapsuleCollider>();
        }
        if (rb == null) {
            rb = GetComponent<Rigidbody>();
        }
        if (smr == null) {
            smr = GetComponent<SkinnedMeshRenderer>();
        }
        if (hygieneManager == null) {
            hygieneManager = FindFirstObjectByType<HygieneManager>();
        }
    }

    private void Awake() {
        if (capsule == null) {
            Debug.LogError($"No CapsuleCollider component assigned for {this.name}.");
        } else if (!capsule.isTrigger) {
            Debug.LogWarning($"{this.name}'s CapsuleCollider must be set to IsTrigger for hygiene touch to work.");
        }
        if (rb == null) {
            Debug.LogError($"No Rigidbody component assigned for {this.name}.");
        } else if (!rb.isKinematic) {
            Debug.LogWarning($"{this.name}'s Rigidbody must be set to IsKinematic for hygiene touch to work.");
        }
        if (gloveMaterial == null) {
            Debug.LogWarning($"No glove material set on object {this.name}. Please specify the glove material for proper glove on/off detection.");
        }
        if (hygieneManager == null) {
            Debug.LogError($"No HygieneManager assigned in HygieneTouchReporter on {this.name}.");
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (!smr.material.name.ToLower().Contains("glove")) {
            // Only track objects that are touched when gloves are on. Otherwise, just ignore the collision.
            return;
        }
        if (other.isTrigger) {
            // Also ignore other trigger colliders.
            return;
        }
        Vector3 touchPoint = other.ClosestPoint(this.transform.parent.position);
        touchPoint = other.transform.InverseTransformPoint(touchPoint);
        hygieneManager.AddPoint(touchPoint, other.gameObject);
        
    }
}
