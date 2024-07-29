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

    // Start is called before the first frame update
    void Start()
    {
        if (arrowRenderer == null)
        {
            Debug.LogError("No Renderer Attached to Hint Arrow");
        }
        if (qMarkRenderer == null)
        {
            Debug.LogError("No Renderer Attached to Question Mark");
        }
    }
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Hint(GameObject.Find("Drying_rack"));
        }
    }

    //This is called if there is no goal
    public void Hint()
    {
        Vector3 cameraPos = camera.transform.position;
        Quaternion cameraRot = camera.transform.rotation;

        //Don't rotate x and z values
        Quaternion goalRot = new Quaternion(0, cameraRot.y, 0, cameraRot.w);
        goalRot = Quaternion.Normalize(goalRot);

        transform.position = cameraPos; // The question mark can be at camera height
        transform.rotation = goalRot;

        transform.Translate(Vector3.forward * displacement);
        StartFlashing(qMarkRenderer);
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

        //Maintain constant height
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
    }

    public void StartFlashing(Renderer myRenderer)
    {
        if (myRenderer != null)
        {
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
}
