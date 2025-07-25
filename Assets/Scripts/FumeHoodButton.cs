using UnityEngine;

public class FumeHoodButton : MonoBehaviour
{
    [SerializeField] DoorOpen doorOpenScript;
    [SerializeField] bool isVR = true;

    [SerializeField, TextArea]
    private string info = "For functionality in VR, set the toggle isVR = true & " +
        "make sure the 'left hand model' --> Left Controller > Left Hand > Hand > hands:hands_geom > left hand model " +
        "& 'right hand model' right Controller > right Hand > Hand > hands:hands_geom > right hand model " +
        "have the Player tag";

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

    public void ToggleFumeHood()
    {
        // GameEventsManager.instance.miscEvents.StirBeaker();
        GameEventsManager.instance.miscEvents.HoodSashHeightSet();
        doorOpenScript.ToggleOpen();
    }

    public void ToggleFumeHoodWebGL(GameObject obj)
    {
        if (obj == gameObject)
            ToggleFumeHood();
    }
}
