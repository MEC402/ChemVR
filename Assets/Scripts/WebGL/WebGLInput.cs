using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WebGLInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset masterControls;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference look;
    [SerializeField] private InputActionReference use;
    [SerializeField] private InputActionReference rotate;
    [SerializeField] private InputActionReference rotation;
    [SerializeField] private InputActionReference pause;
    [SerializeField] private InputActionReference hideUI;

    public Vector2 movementInput;
    public Vector2 lookInput;
    public Vector2 rotationInput;
    public bool isInteracting = false;
    public bool isUsing = false;
    public bool isRotating = false;
    public bool isPaused = false;
    public bool isHidingUI = false;

    public event Action OnInteractPressed;
    public event Action OnInteractReleased;

    [SerializeField] GameObject helpMenu;
    [SerializeField] GameObject instructionsText;

    private void Awake()
    {
        move.action.performed += ctx => MoveHandler(ctx);
        move.action.canceled += ctx => MoveCanceledHandler(ctx);

        look.action.performed += ctx => LookHandler(ctx);
        look.action.canceled += ctx => LookCanceledHandler(ctx);

        interact.action.performed += ctx => InteractHandler(ctx);
        interact.action.canceled += ctx => InteractCanceledHandler(ctx);

        use.action.performed += ctx => UseHandler(ctx);
        use.action.performed += ctx => GameEventsManager.instance.inputEvents.AButtonPressed(ctx);
        use.action.canceled += ctx => UseCanceledHandler(ctx);

        rotate.action.performed += ctx => RotateHandler(ctx);
        rotate.action.canceled += ctx => RotateCanceledHandler(ctx);

        rotation.action.performed += ctx => RotationHandler(ctx);
        rotation.action.canceled += ctx => RotationCanceledHandler(ctx);

        pause.action.performed += ctx => PauseHandler(ctx);

        hideUI.action.performed += ctx => HideUIHandler(ctx);

        MasterControlsEnable(); // Enable the player controls by default
    }

    #region Input Handlers
    private void MoveHandler(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    private void MoveCanceledHandler(InputAction.CallbackContext ctx)
    {
        movementInput = Vector2.zero;
    }

    private void LookHandler(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void LookCanceledHandler(InputAction.CallbackContext ctx)
    {
        lookInput = Vector2.zero;
    }

    private void InteractHandler(InputAction.CallbackContext ctx)
    {
        isInteracting = true;
        OnInteractPressed?.Invoke();
    }

    private void InteractCanceledHandler(InputAction.CallbackContext ctx)
    {
        isInteracting = false;
        OnInteractReleased?.Invoke();
    }

    public void Interacting()
    {
        isInteracting = true;   // Can pass this variable to another script that handels interaction that needs to know if the interact key is currently being passed
    }

    private void UseHandler(InputAction.CallbackContext ctx)
    {
        isUsing = true;
    }

    private void UseCanceledHandler(InputAction.CallbackContext ctx)
    {
        isUsing = false;
    }

    private void RotateHandler(InputAction.CallbackContext ctx)
    {
        isRotating = true;
    }

    private void RotateCanceledHandler(InputAction.CallbackContext ctx)
    {
        isRotating = false;
    }

    private void RotationHandler(InputAction.CallbackContext ctx)
    {
        rotationInput = ctx.ReadValue<Vector2>();
    }

    private void RotationCanceledHandler(InputAction.CallbackContext ctx)
    {
        rotationInput = Vector2.zero;
    }

    private void PauseHandler(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused; // Toggle the pause state
        HandlePauseState();
    }

    private void HideUIHandler(InputAction.CallbackContext ctx)
    {
        isHidingUI = !isHidingUI; // Toggle the UI visibility state
        instructionsText.SetActive(!isHidingUI); // Set the UI active based on the state
    }

    #region Custom Methods
    private void HandlePauseState()
    {
        if (helpMenu != null)
        {
            helpMenu.SetActive(isPaused);

            // Pause or resume the game
            Time.timeScale = isPaused ? 0f : 1f;

            // Unlock or lock the mouse cursor
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;
        }
    }
    #endregion

    #endregion
    #region Enable & Disable Actions
    public void MasterControlsEnable()
    {
        masterControls.Enable();
    }
    public void MasterControlsDisable()
    {
        masterControls.Disable();
    }

    public void MoveEnable()
    {
        move.action.Enable();
    }

    public void MoveDisable()
    {
        move.action.Disable();
    }

    public void LookEnable()
    {
        look.action.Enable();
    }

    public void LookDisable()
    {
        look.action.Disable();
    }

    public void InteractEnable()
    {
        interact.action.Enable();
    }

    public void InteractDisable()
    {
        interact.action.Disable();
    }

    public void UseEnable()
    {
        use.action.Enable();
    }

    public void UseDisable()
    {
        use.action.Disable();
    }
    public void RotateEnable()
    {
        rotate.action.Enable();
    }

    public void RotateDisable()
    {
        rotate.action.Disable();
    }

    public void RotationEnable()
    {
        rotation.action.Enable();
    }

    public void RotationDisable()
    {
        rotation.action.Disable();
    }
    public void PauseEnable()
    {
        pause.action.Enable();
    }

    public void PauseDisable()
    {
        pause.action.Disable();
    }

    public void HideUIEnable()
    {
        hideUI.action.Enable();
    }

    public void HideUIDisable()
    {
        hideUI.action.Disable();
    }
    #endregion

}
