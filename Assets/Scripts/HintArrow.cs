using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HintArrow : MonoBehaviour
{

    public Camera camera;
    private Renderer myRenderer;
    public float flashDuration = 0.125f;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = this.GetComponent<MeshRenderer>();

        if (myRenderer == null)
        {
            Debug.LogError("No Renderer Attached to Hint Arrow");
        }
    }
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Hint();
        }
    }
    public void Hint()//GameObject pointAt)
    {
        Vector3 cameraPos = camera.transform.position;
        Quaternion cameraRot = camera.transform.rotation;

        Vector3 goalPos = new Vector3(cameraPos.x, 1, cameraPos.z);








       
        // If you need to keep the x, z, and w from the original and just zero the y rotation:
        Quaternion intendedRot = new Quaternion(0, cameraRot.y, 0, cameraRot.w);
        intendedRot = Quaternion.Normalize(intendedRot);









        Quaternion goalRot = Quaternion.Euler(new Vector3(cameraRot.x, 0, cameraRot.z));
        transform.position = goalPos;
        transform.rotation = intendedRot;

        int displacement = 1;
        transform.Translate(Vector3.forward * displacement);
        StartFlashing();
    }

    public void StartFlashing()
    {
        if (myRenderer != null)
        {
            StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
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
