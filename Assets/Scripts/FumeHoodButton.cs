using UnityEngine;

public class FumeHoodButton : MonoBehaviour
{
    [SerializeField] DoorOpen doorOpenScript;
    [SerializeField] bool isVR = true;

    private void Start()
    {
        if (!isVR)
        {
            if (TryGetComponent<BoxCollider>(out var collider))
                collider.isTrigger = false;
        }
    }

    private void OnEnable()
    {
        if (!isVR)
            GameEventsManager.instance.webGLEvents.OnInteractPressed += ToggleFumeHoodWebGL;
    }

    private void OnDisable()
    {
        if (!isVR)
            GameEventsManager.instance.webGLEvents.OnInteractPressed -= ToggleFumeHoodWebGL;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isVR)
                ToggleFumeHood();
        }
    }

    public void ToggleFumeHood() => doorOpenScript.ToggleOpen();

    public void ToggleFumeHoodWebGL(GameObject obj)
    {
        if (obj == gameObject)
            ToggleFumeHood();
    }
}
