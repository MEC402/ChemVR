using UnityEngine;

public class EnableFlaskTrigger : MonoBehaviour
{
    [SerializeField] BoxCollider flaskTrigger;

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnEnableFlaskTrigger += EnableTrigger;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnEnableFlaskTrigger -= EnableTrigger;
    }

    private void EnableTrigger(bool enable) => flaskTrigger.enabled = enable;
}
