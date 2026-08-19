using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Watches for the player washing their hands at the sink and reports it to the wash-hands step.
///
/// In VR the hand colliders enter the basin volume. On desktop there are no hands, and the volume
/// sits inside the basin where a body capsule cannot reach, so standing at the sink and looking
/// into it counts instead.
/// </summary>
public class WashHandsTrigger : MonoBehaviour
{
    [Header("Desktop / WebGL Reach")]
    [SerializeField, Tooltip("Desktop has no hand colliders, so standing at the basin and looking " +
        "into it counts as holding your hands under the water. Turn this off in VR-only scenes.")]
    private bool playerBodyCountsAsHands = true;
    [SerializeField, Tooltip("How close the player has to stand to the basin for that to count.")]
    private float playerRange = 1.25f;
    [SerializeField, Range(10f, 180f), Tooltip("How far off-centre the basin can sit in your view " +
        "and still count as looking into it. 180 accepts any direction.")]
    private float playerAngle = 75f;

    private bool leftHandWashed = false;
    private bool rightHandWashed = false;
    private bool eventTaskEnabled = false;
    private Collider basin;

    private void Awake()
    {
        basin = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other) => RecordWash(other);

    // Hands already in the water when the step begins have to count too. Listening only for the
    // moment of entry left the step stuck for anyone who walked to the sink before it started.
    private void OnTriggerStay(Collider other) => RecordWash(other);

    private void RecordWash(Collider other)
    {
        if (other == null) return;

        string otherName = other.name.ToLower();
        bool isPlayerBody = other.CompareTag("Player");

        if (otherName.Contains("left") || isPlayerBody)
        {
            leftHandWashed = true;
        }

        if (otherName.Contains("right") || isPlayerBody)
        {
            rightHandWashed = true;
        }
    }

    private void Update()
    {
        if (!eventTaskEnabled) return;

        // Reported every frame the condition holds, not once: the wash-hands step ignores the
        // report until both gloves are off, so it has to be able to hear it again afterwards.
        if ((leftHandWashed && rightHandWashed) || PlayerIsAtTheBasin())
        {
            GameEventsManager.instance.miscEvents.HandsinWater();
        }
    }

    private bool PlayerIsAtTheBasin()
    {
        if (!playerBodyCountsAsHands) return false;

        Vector3 point = (basin != null) ? basin.bounds.center : transform.position;
        return PlayerReach.IsWithinReach(point, playerRange, playerAngle);
    }

    public void ActivateWashHands()
    {
        eventTaskEnabled = true;

        // Start the step from a clean slate. The player has been at this sink all through the
        // clean-up steps, and stale touches would otherwise finish the step the moment it opens.
        leftHandWashed = false;
        rightHandWashed = false;
    }
}
