using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PipetteFunctions : MonoBehaviour
{
    private bool canDispense = false;
    private bool isOverlapping = false;
    private GameObject currentContainer;
    [SerializeField]private ResizeFluid internalFluid;
    public bool isHeld = false;

    [SerializeField]private float dispenseAmount = 10f; // Amount to dispense per interaction
    [SerializeField] public ChemFluid currentFluids;

    // Hand tracking variables
    private enum HandType { None, Left, Right }
    private HandType currentHand = HandType.None;

    public void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        currentHand = DetermineGrabbingHand(args.interactorObject);
        EnableHandSpecificButtonListener();
    }

    public void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        DisableButtonListeners();
        currentHand = HandType.None;
    }

    private HandType DetermineGrabbingHand(IXRInteractor interactor)
    {
        var controllerTransform = interactor.transform;

        // Check transform hierarchy for left/right indicators
        Transform current = controllerTransform;
        while (current != null)
        {
            string name = current.name.ToLower();
            if (name.Contains("left")) return HandType.Left;
            if (name.Contains("right")) return HandType.Right;
            current = current.parent;
        }

        // Check XRController component for handedness
        var xrController = controllerTransform.GetComponentInParent<XRController>();
        if (xrController != null)
        {
            var controllerNode = xrController.controllerNode;
            if (controllerNode == UnityEngine.XR.XRNode.LeftHand) return HandType.Left;
            if (controllerNode == UnityEngine.XR.XRNode.RightHand) return HandType.Right;
        }

        // Default to right hand
        return HandType.Right;
    }

    private void EnableHandSpecificButtonListener()
    {
        switch (currentHand)
        {
            case HandType.Left:
                GameEventsManager.instance.inputEvents.onXButtonPressed += OnXButtonPressed;
                break;
            case HandType.Right:
                GameEventsManager.instance.inputEvents.onAButtonPressed += OnAButtonPressed;
                break;
        }
    }

    private void DisableButtonListeners()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= OnAButtonPressed;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= OnXButtonPressed;
    }

    private void OnAButtonPressed(InputAction.CallbackContext context)
    {
        if (isHeld && currentHand == HandType.Right)
        {
            AbsorbAndDispense();
        }
    }

    private void OnXButtonPressed(InputAction.CallbackContext context)
    {
        if (isHeld && currentHand == HandType.Left)
        {
            AbsorbAndDispense();
        }
    }

    private void AbsorbAndDispense()
    {
        if (canDispense)
        {
            if (isOverlapping)
            {
                ChemContainer container = currentContainer.GetComponent<ChemContainer>();

                for (int i = 0; i < currentFluids.GetChemArray().Length; i++)
                {
                    container.AddChem(currentFluids.GetChemArray()[i].type, currentFluids.GetChemArray()[i].volume);
                }
                container.UpdateChem();
                GameEventsManager.instance.chemistryEvents.PipetteDispense(this, container, currentFluids);
                GameEventsManager.instance.chemistryEvents.PourIn(container, container.GetChemFluid());
                canDispense = false; // Reset after dispensing
                Debug.Log($"Dispensed {dispenseAmount} of {currentFluids} into container.");
                internalFluid.gameObject.SetActive(false); // Hide the internal fluid after dispensing
            }
            else
            {
                Debug.LogWarning("Cannot dispense: Not overlapping with a ChemContainer.");
            }
        }
        else if (!canDispense)
        {
            if (isOverlapping)
            {
                ChemContainer container = currentContainer.GetComponent<ChemContainer>();
                Chem[] activeChems = container.GetChemFluid(dispenseAmount);
                currentFluids.AssignNewChems(activeChems); // Update currentFluid to the main chem type
                canDispense = true; // Allow dispensing again
                internalFluid.gameObject.SetActive(true); // Show the internal fluid when ready to dispense
                Color chemColor = ChemistryManager.instance.GetColor(currentFluids);
                internalFluid.SetColor(chemColor);

            }
            else
            {
                Debug.LogWarning("Cannot absorb: Not overlapping with a ChemContainer.");
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ChemContainer>() != null)
        {
            isOverlapping = true;
            currentContainer = other.gameObject; // Store the current container reference
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ChemContainer>() != null)
        {
            isOverlapping = false;
            currentContainer = null; // Clear the current container reference
        }
    }
}



