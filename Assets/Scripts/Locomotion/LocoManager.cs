using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit.LocomotionSystem.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class LocoManager : MonoBehaviour
{
    #region Variables
    [SerializeField] LocoOptions locoOptions;
    //public ActionBasedControllerManager ControllerManager;
    public GameObject xrOrigin;

    public GameObject rightController;
    public GameObject leftController;
    public GameObject turn;
    public GameObject move;
    public GameObject vignette;

    public static LocoManager Instance;

    #endregion
    #region Unity Methods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        ChangeLocoOptions();
    }
    private void OnEnable()
    {
        if (locoOptions != null)
        {
            locoOptions.OnVlauesChanged += ChangeLocoOptions;
        }
    }

    private void OnDisable()
    {
        if (locoOptions != null)
        {
            locoOptions.OnVlauesChanged -= ChangeLocoOptions;
        }
    }

    #endregion
    #region Custom Methods
   
    public void ChangeLocoOptions()
    {
        if (locoOptions == null)
            return;

        vignette.SetActive(locoOptions.vignette);

        var turnProvider = turn.GetComponent<ContinuousTurnProviderBase>();
        if (turnProvider != null)
        {
            turnProvider.turnSpeed = locoOptions.turnSpeed;
            //sets smooth turn speed
        }

        var snapTurnProvider = turn.GetComponent<SnapTurnProviderBase>();
        if (snapTurnProvider != null)
        {
            snapTurnProvider.turnAmount = locoOptions.snapTurnAmount;
            //sets snap turn amount
        }

        var rightInputManager = rightController.GetComponent<ActionBasedControllerManager>();
        if (rightInputManager != null)
        {
            rightInputManager.smoothTurnEnabled = !locoOptions.snapTurn;
            //enables smoothTurn on the right controller
            //do we need a case to disable smooth turn on the left controller?
        }

        var moveProvider = move.GetComponent<DynamicMoveProvider>();
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = locoOptions.moveSpeed;
            //sets the move/walk speed value
        }

        var leftInputManager = leftController.GetComponent<ContinuousMoveProviderBase>();
        if (leftInputManager != null)
        {
            moveProvider.enabled = locoOptions.smoothMove; 
            //leftInputManager.smoothMoveEnabled = locoOptions.smoothMove;
        }
    }

    /*
     * freezes or unfreezes the player based on the freeze bool.
     * <param name="freeze"> true to freeze the player, false to unfreeze </param>
     */
    public void FreezePlayer(bool freeze)
    {
        rightController.GetComponent<ActionBasedControllerManager>().enabled = !freeze;
        leftController.GetComponent<ActionBasedControllerManager>().enabled = !freeze;
        turn.GetComponent<ContinuousTurnProviderBase>().enabled = !freeze;
        turn.GetComponent<SnapTurnProviderBase>().enabled = !freeze;
        move.GetComponent<DynamicMoveProvider>().enabled = !freeze;
    }
    #endregion
}