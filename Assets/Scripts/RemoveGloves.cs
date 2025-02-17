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

    [SerializeField] AudioSource trashSound;

    void Start()
    {
        // Get and store the original material
        leftIsTouching = false;
        rightIsTouching = false;

        if (rightHand != null)
            original = rightHand.GetComponent<SkinnedMeshRenderer>().material;
    }
    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed += OnAPress;
        GameEventsManager.instance.inputEvents.onXButtonPressed += OnXPress;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= OnAPress;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= OnXPress;
    }

    void OnAPress(InputAction.CallbackContext context)
    {
        if (rightIsTouching && (rightHand.GetComponent<SkinnedMeshRenderer>().material.name.Contains(gloves.name)))
        {
            TakeOffRightGlove();
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
