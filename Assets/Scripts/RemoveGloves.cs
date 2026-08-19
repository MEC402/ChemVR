using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RemoveGloves : MonoBehaviour
{
    public Material gloves;
    public GameObject leftHand;
    public GameObject rightHand;
    private Material original;
    private bool leftIsTouching;
    private bool rightIsTouching;

    private bool isWebGL = false;
    private bool webGLIsTouching = false;

    [SerializeField] AudioSource trashSound;

    [Header("Desktop / WebGL Reach")]
    [SerializeField, Tooltip("How close the player can stand and still bin their gloves with the " +
        "use key. The bin's trigger volume is barely wider than the bin itself, so without this " +
        "you have to walk into the bin to get the key to do anything.")]
    private float webGLUseRange = 2f;
    [SerializeField, Range(10f, 180f), Tooltip("How far off-centre the bin can sit in your view " +
        "and still count as being aimed at. 180 accepts any direction.")]
    private float webGLUseAngle = 75f;

    private Collider reachAnchor;

    void Start()
    {
        // Get and store the original material
        leftIsTouching = false;
        rightIsTouching = false;

        reachAnchor = GetComponent<Collider>();

        if (rightHand != null)
            original = rightHand.GetComponent<SkinnedMeshRenderer>().material;
    }

    public static bool IsRunningOnWebGL()
     {
         return Application.platform == RuntimePlatform.WebGLPlayer;
     }

    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onRTriggerPressed += OnAPress;
        GameEventsManager.instance.inputEvents.onLTriggerPressed += OnXPress;

        // Also treat "no headset running" as desktop, so the WebGL path can be tested in the
        // editor instead of only in a published build.
        isWebGL = IsRunningOnWebGL() || !UnityEngine.XR.XRSettings.isDeviceActive;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onRTriggerPressed -= OnAPress;
        GameEventsManager.instance.inputEvents.onLTriggerPressed -= OnXPress;
    }

    void OnAPress(InputAction.CallbackContext context)
    {
        if (!isWebGL)
            if (rightIsTouching && (rightHand.GetComponent<SkinnedMeshRenderer>().material.name.Contains(gloves.name)))
            {
                TakeOffRightGlove();
            }
        if (isWebGL)
        {
            if (webGLIsTouching || PlayerIsAtTheBin())
            WebTakeOffGloves();
        }
    }

    /// <summary>
    /// Whether the player is stood in front of the bin looking at it. Standing inside the bin's
    /// trigger volume still counts on its own - this only widens where the key works.
    /// </summary>
    private bool PlayerIsAtTheBin()
    {
        Vector3 point = (reachAnchor != null) ? reachAnchor.bounds.center : transform.position;
        return PlayerReach.IsWithinReach(point, webGLUseRange, webGLUseAngle);
    }
    void OnXPress(InputAction.CallbackContext context)
    {
        if (leftIsTouching && (leftHand.GetComponent<SkinnedMeshRenderer>().material.name.Contains(gloves.name)))
        {
            TakeOffLeftGlove();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("right") && other.name.Contains("hand"))
        {
            rightIsTouching = true;
        }
        else if (other.name.Contains("left") && other.name.Contains("hand"))
        {
            leftIsTouching = true;
        }
        else if (isWebGL && (other.CompareTag("Player") || other.name.Contains("FP Player")))
        {
            webGLIsTouching = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("right") && other.name.Contains("hand"))
        {
            rightIsTouching = false;
        }
        else if (other.name.Contains("left") && other.name.Contains("hand"))
        {
            leftIsTouching = false;
        }
        else if (isWebGL && (other.CompareTag("Player") || other.name.Contains("FP Player")))
        {
            webGLIsTouching = false;
        }
    }
    void TakeOffLeftGlove()
    {
        leftHand.GetComponent<SkinnedMeshRenderer>().material = original;
        PlayTrashAudio();
        GameEventsManager.instance.miscEvents.TakeOffLeftGlove();
        //Debug.Log("Left glove taken off!");
    }
    void TakeOffRightGlove()
    {
        rightHand.GetComponent<SkinnedMeshRenderer>().material = original;
        PlayTrashAudio();
        GameEventsManager.instance.miscEvents.TakeOffRightGlove();
        //Debug.Log("Right glove taken off!");
    }

    public void WebTakeOffGloves()
    {
        GameEventsManager.instance.miscEvents.TakeOffLeftGlove();
        GameEventsManager.instance.miscEvents.TakeOffRightGlove();

        PlayTrashAudio();
    }

    private void PlayTrashAudio()
    {
        if (trashSound == null) return;

        trashSound.pitch = UnityEngine.Random.Range(0.9f, 1.1f);

        trashSound.Play();
    }
}
