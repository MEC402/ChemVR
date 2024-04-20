using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float speed = 4.0f; // Adjust this value to change movement speed
    public float cameraHeight = 2.11f;
    private float rotation = 0.0f;


    void Update()
    {
        Vector2 moveDirection = Vector2.zero;
        float rotationDiff = 0.0f;

        // Read WASD or arrow keys input from Input System
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            moveDirection.y = 1.0f;
        }
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            moveDirection.y = -1.0f;
        }
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            rotationDiff = -0.35f;
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            rotationDiff += 0.35f;
        }
        
        //Slower if backwards
        if (moveDirection.y < 0)
        {
            rotationDiff /= 2;
            moveDirection.y /= 2;
        }

        rotation += rotationDiff; 

        // Move the camera based on input (only in XY plane)
        Vector3 movement = new Vector3(moveDirection.x, 0.0f, moveDirection.y) * speed * Time.deltaTime;
        transform.position += transform.TransformDirection(movement);

        // Set the camera's height to the specified value
        Vector3 newPosition = transform.position;
        newPosition.y = cameraHeight;
        transform.position = newPosition;

        // Rotate the camera based on the position of the left right movement
        float angle = rotation;
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}
