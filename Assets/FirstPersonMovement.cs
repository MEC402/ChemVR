using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class FirstPersonMovement : MonoBehaviour
{
    public WebGLInput webGLInput;
    [SerializeField] public CinemachineVirtualCamera playerCamera; // Reference to the first-person camera
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float stoppingTime = 0.2f;
    public float mouseSensitivity = 1f;
    [SerializeField] private float cameraSmoothing = 5f; // Smoothing factor for camera rotation

    private Vector2 rotation = Vector2.zero;
    private Vector2 rotationSmooth = Vector2.zero; // Smoothed rotation values

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor at start
        Cursor.visible = false; // Hide cursor at start
    }

    private void FixedUpdate()
    {
        Vector2 movementInput = webGLInput.movementInput;
        Vector2 mouseInput = webGLInput.lookInput;

        // Rotate the player based on mouse input
        rotation += mouseInput * mouseSensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -90f, 90f); // Limit vertical rotation

        // Smooth the rotation
        rotationSmooth = Vector2.Lerp(rotationSmooth, rotation, 1f / cameraSmoothing);

        playerCamera.transform.localRotation = Quaternion.Euler(-rotationSmooth.y, 0, 0); // Rotate camera vertically
        transform.rotation = Quaternion.Euler(0, rotationSmooth.x, 0); // Rotate player horizontally

        // Get the forward direction of the camera without vertical component
        Vector3 cameraForward = Vector3.Scale(playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // Calculate movement direction relative to the camera
        Vector3 moveDirection = (cameraForward * movementInput.y + playerCamera.transform.right * movementInput.x).normalized;

        // Calculate movement
        Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;

        // Move the player using Rigidbody velocity
        rb.MovePosition(rb.position + movement);

        // Limit the velocity to the maximum speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // If there is no input, ease into stop
        if (movementInput == Vector2.zero)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, stoppingTime);
        }
    }
}
