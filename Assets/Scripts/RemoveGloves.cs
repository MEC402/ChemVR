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

    void Start()
    {
        // Get and store the original material
        leftIsTouching = false;
        rightIsTouching = false;

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

        isWebGL = IsRunningOnWebGL();
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
            if (webGLIsTouching)
            WebTakeOffGloves();
        }
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
        else if (isWebGL && other.name.Contains("FP Player"))
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
        else if (isWebGL && other.name.Contains("FP Player"))
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
