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
        }

        //if needed, write method to enable snapTurn on LeftController for edge case of using smoothTurn on right controller
        //also will need to set the turn radius of the snap controller to 0 when smooth turn is enabled
        //will then need to reset the turn radius of snap controller to 45 when snap turn is enabled

        var moveProvider = move.GetComponent<DynamicMoveProvider>();
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = locoOptions.moveSpeed;
            //sets the move/walk speed value
        }
        /* //m_SmoothMotionEnabled
         var leftMoveProvider = leftController.GetComponent<ContinuousMoveProviderBase>(); //DynamicMoveProvider //ContinuousMoveProviderBase
         if (leftMoveProvider != null)
         {
             leftMoveProvider.enabled = locoOptions.smoothMove; 
             //leftInputManager.smoothMoveEnabled = locoOptions.smoothMove;
         }*/
        var leftMoveProvider = leftController.GetComponent<ActionBasedControllerManager>(); //DynamicMoveProvider //ContinuousMoveProviderBase
        if (leftMoveProvider != null)
        {
            leftMoveProvider.smoothMotionEnabled = locoOptions.smoothMove;
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
        move.GetComponent<ActionBasedControllerManager>().enabled = !freeze;
    }
    #endregion
}