using UnityEngine;

/// <summary>
/// World-space station label (number + name). Faces the player's camera (yaw only) when shown.
/// Labels are an authoring aid: they stay visible in the Editor but are hidden when the game runs,
/// unless visibleInPlay is ticked.
/// </summary>
public class StationLabelBillboard : MonoBehaviour
{
    [SerializeField] bool visibleInPlay = false;

    void Awake()
    {
        if (!visibleInPlay) gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        var cam = Camera.main;
        if (cam == null) return;
        Vector3 away = transform.position - cam.transform.position;
        away.y = 0f;
        if (away.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(away);
    }
}
