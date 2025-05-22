using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class FirstPersonMovement : MonoBehaviour
{
    public WebGLInput webGLInput;
    public CinemachineVirtualCamera playerCamera; // Reference to the first-person camera
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 15f;
    public float mouseSensitivity = 0.05f;
    [SerializeField] private float cameraSmoothing = 2f; // Smoothing factor for camera rotation

    private Vector2 rotation = Vector2.zero;
    private Vector2 rotationSmooth = Vector2.zero; // Smoothed rotation values

    private Vector2 movementInput;
    private Vector2 mouseInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor at start
        Cursor.visible = false; // Hide cursor at start
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        movementInput = webGLInput.movementInput;
        mouseInput = webGLInput.lookInput;
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward = Vector3.Scale(playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = (cameraForward * movementInput.y + playerCamera.transform.right * movementInput.x).normalized;
        Vector3 movement = moveSpeed * Time.fixedDeltaTime * moveDirection;

        rb.MovePosition(rb.position + movement);
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 0f) return;

        rotation += mouseInput * mouseSensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);
        rotationSmooth = Vector2.Lerp(rotationSmooth, rotation, 1f / cameraSmoothing);

        playerCamera.transform.localRotation = Quaternion.Euler(-rotationSmooth.y, 0, 0);
        transform.rotation = Quaternion.Euler(0, rotationSmooth.x, 0);
    }
}
