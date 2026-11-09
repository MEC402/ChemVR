using UnityEngine;

/// <summary>
/// Lets a VR hand toggle a sink valve on/off by touching it, instead of requiring the
/// player to grab and physically twist it. Snaps the valve to the same open/closed
/// rotations used by ToggleSink/ToggleSinkR (the WebGL click-to-toggle equivalent), so
/// RotatingValveController (which reads the valve's live rotation every frame) picks up
/// the change the same way it would from a physical twist.
/// </summary>
public class SinkTouchToggle : MonoBehaviour
{
    [Tooltip("Local Euler angles the valve snaps to when toggled on.")]
    [SerializeField] private Vector3 onRotation = new Vector3(0f, 270f, -90f);
    [Tooltip("Local Euler angles the valve snaps to when toggled off.")]
    [SerializeField] private Vector3 offRotation = new Vector3(0f, 0f, -90f);

    [Tooltip("Minimum time between toggles, so a hand lingering/jittering at the trigger's edge doesn't flip the sink on/off repeatedly.")]
    [SerializeField] private float toggleCooldown = 0.75f;

    public bool isSinkOn = false;

    private float lastToggleTime = -Mathf.Infinity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time - lastToggleTime >= toggleCooldown)
        {
            ToggleOpen();
        }
    }

    public void ToggleOpen()
    {
        isSinkOn = !isSinkOn;
        transform.localEulerAngles = isSinkOn ? onRotation : offRotation;
        lastToggleTime = Time.time;
    }
}
