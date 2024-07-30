using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HintController : MonoBehaviour
{
    public Camera camera;
    public Renderer arrowRenderer;
    public Renderer qMarkRenderer;
    public float flashDuration = 0.125f;
    private float displacement = .75f;
    private GameObject target = null;
    private Material blinkTarget = null;
    private bool pointOrBlink = true; // true is point, false is blink

    //Animators of both objects
    Animator arrowAnimator;
    Animator questionAnimator;

    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onSetHint += SetGoal;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onSetHint -= SetGoal;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set animators
        arrowAnimator = arrowRenderer.gameObject.GetComponent<Animator>();
        questionAnimator = qMarkRenderer.gameObject.GetComponent<Animator>();

        if (arrowRenderer == null)
        {
            Debug.LogError("No Renderer Attached to Hint Arrow");
        }
        if (qMarkRenderer == null)
        {
            Debug.LogError("No Renderer Attached to Question Mark");
        }
        if (arrowAnimator == null)
        {
            Debug.LogError("No Animator Attached to Hint Arrow");
        }
        if (questionAnimator == null)
        {
            Debug.LogError("No Animator Attached to Question Mark");
        }
    }
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Hint(target); 
        }

    }

    private void SetGoal(GameObject obj)
    {
        target = obj;
        pointOrBlink = true;
    }

    //This is called if there is no goal
    public void Hint()
    {
        Vector3 cameraPos = camera.transform.position;
        Quaternion cameraRot = camera.transform.rotation;

        //Maintain proportioned height
        Vector3 goalPos = new Vector3(cameraPos.x, cameraPos.y * .9f, cameraPos.z);

        //Don't rotate x and z values
        Quaternion goalRot = new Quaternion(0, cameraRot.y, 0, cameraRot.w);
        goalRot = Quaternion.Normalize(goalRot);

        transform.position = goalPos; // The question mark can be at camera height
        transform.rotation = goalRot;

        transform.Translate(Vector3.forward * displacement);
        StartFlashing(qMarkRenderer);
        //StartBouncing(qMarkRenderer, questionAnimator);
        //StartStretching(qMarkRenderer, questionAnimator);
    }
    public void Hint(GameObject pointAt)
    {
        if (pointAt == null)
        {
            Hint();
            return;
        }
        Vector3 cameraPos = camera.transform.position;
        Quaternion cameraRot = camera.transform.rotation;

        //Maintain proportioned height
        Vector3 goalPos = new Vector3(cameraPos.x, cameraPos.y * .75f, cameraPos.z);

        //Don't rotate x and z values
        Quaternion goalRot = new Quaternion(0, cameraRot.y, 0, cameraRot.w);
        goalRot = Quaternion.Normalize(goalRot);

        transform.position = goalPos; //move to camera
        transform.rotation = goalRot; //point forward from camera

        //move forward from camera
        transform.Translate(Vector3.forward * displacement);

        //point at goal ***after*** displacement
        transform.LookAt(pointAt.transform.position);

        StartFlashing(arrowRenderer);
        //StartBouncing(arrowRenderer, arrowAnimator);
        //StartStretching(arrowRenderer, arrowAnimator);
    }

    public void StartFlashing(Renderer myRenderer)
    {
        if (myRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashRoutine(myRenderer));
        }
    }

    private IEnumerator FlashRoutine(Renderer myRenderer)
    {
        myRenderer.enabled = true;
        yield return new WaitForSeconds(flashDuration);

        myRenderer.enabled = false;
        yield return new WaitForSeconds(flashDuration);

        myRenderer.enabled = true;
        yield return new WaitForSeconds(flashDuration);

        myRenderer.enabled = false;
    }

    public void StartBouncing(Renderer myRenderer, Animator myAnimator)
    {
        if (myRenderer != null)
        {
            //myAnimator.ResetTrigger("Bounce");
            //myAnimator.ResetTrigger("Stretch");
            //myAnimator.SetTrigger("Reset");
            //myAnimator.ResetTrigger("Reset");

            StopAllCoroutines();
            StartCoroutine(BounceRoutine(myRenderer, myAnimator));
        }
    }

    public void StartStretching(Renderer myRenderer, Animator myAnimator)
    {
        if (myRenderer != null)
        {
            //myAnimator.ResetTrigger("Bounce");
            //myAnimator.ResetTrigger("Stretch");
            //myAnimator.SetTrigger("Reset");
            //myAnimator.ResetTrigger("Reset");

            StopAllCoroutines();
            StartCoroutine(StretchRoutine(myRenderer, myAnimator));
        }
    }

    private IEnumerator BounceRoutine(Renderer myRenderer, Animator myAnimator)
    {
        myRenderer.enabled = true;
        myAnimator.SetTrigger("Bounce");
        yield return new WaitForSeconds(10 * flashDuration);
        myRenderer.enabled = false;
        myAnimator.ResetTrigger("Bounce");
        myAnimator.SetTrigger("Reset");
    }

    private IEnumerator StretchRoutine(Renderer myRenderer, Animator myAnimator)
    {
        myRenderer.enabled = true;
        myAnimator.SetTrigger("Stretch");
        yield return new WaitForSeconds(10 * flashDuration);
        myRenderer.enabled = false;
        myAnimator.ResetTrigger("Stretch");
        myAnimator.SetTrigger("Reset");
    }
}
