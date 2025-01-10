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
    [SerializeField] private InputActionReference equip;

    public Vector2 movementInput;
    public Vector2 lookInput;
    public bool isInteracting = false;
    public bool isEquipping = false;

    private void Awake()
    {
        move.action.performed += ctx => MoveHandler(ctx);
        move.action.canceled += ctx => MoveCanceledHandler(ctx);

        look.action.performed += ctx => LookHandler(ctx);
        look.action.canceled += ctx => LookCanceledHandler(ctx);

        interact.action.performed += ctx => InteractHandler(ctx);
        interact.action.canceled += ctx => InteractCanceledHandler(ctx);

        equip.action.performed += ctx => EquipHandler(ctx);
        equip.action.canceled += ctx => EquipCanceledHandler(ctx);

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
    }

    private void InteractCanceledHandler(InputAction.CallbackContext ctx)
    {
        isInteracting = false;
    }

    public void Interacting()
    {
        isInteracting = true;   // Can pass this variable to another script that handels interaction that needs to know if the interact key is currently being passed
    }

    private void EquipHandler(InputAction.CallbackContext ctx)
    {
        isEquipping = true;
    }

    private void EquipCanceledHandler(InputAction.CallbackContext ctx)
    {
        isEquipping = false;
    }

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

    public void EquipEnable()
    {
        equip.action.Enable();
    }

    public void EquipDisable()
    {
        equip.action.Disable();
    }
    #endregion

}
