
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PipetteFunctions : MonoBehaviour
{
    private bool canDispense = false;
    private bool isOverlapping = false;
    private GameObject currentContainer;
    [SerializeField] private ResizeFluid internalFluid;
    [SerializeField] private GameObject bulbColliderObject;
    private Collider bulbCollider;
    [SerializeField] private bool LockChemFluid = false;
    [SerializeField] private bool lockDispenseAmount = false;
    public bool isHeld = false;

    [SerializeField]private float dispenseAmount = 10f; // Amount to dispense per interaction
    [SerializeField] public ChemFluid currentFluids;

    // Hand tracking variables
    private enum HandType { None, Left, Right }
    private HandType currentHand = HandType.None;

    private void Start()
    {
        bulbCollider = bulbColliderObject.GetComponent<SphereCollider>();
    }

    public void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        if (args.interactorObject != null)
        {
            currentHand = DetermineGrabbingHand(args.interactorObject);
            EnableHandSpecificButtonListener();
        }
        else
        {
            currentHand = HandType.Right; // Default to right hand if interactor is null
            EnableHandSpecificButtonListener();
        }
        bulbCollider.enabled = true; // Enable the bulb collider when grabbed

    }

    public void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        DisableButtonListeners();
        currentHand = HandType.None;
        bulbCollider.enabled = false; // Disable the bulb collider when released
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
                GameEventsManager.instance.inputEvents.onLTriggerPressed += onLTriggerPressed;
                break;
            case HandType.Right:
                GameEventsManager.instance.inputEvents.onRTriggerPressed += onRTriggerPressed;
                break;
        }
    }

    private void DisableButtonListeners()
    {
        GameEventsManager.instance.inputEvents.onRTriggerPressed -= onRTriggerPressed;
        GameEventsManager.instance.inputEvents.onLTriggerPressed -= onLTriggerPressed;
    }

    private void onRTriggerPressed(InputAction.CallbackContext context)
    {
        if (isHeld && currentHand == HandType.Right)
        {
            AbsorbAndDispense();
        }
    }

    private void onLTriggerPressed(InputAction.CallbackContext context)
    {
        if (isHeld && currentHand == HandType.Left)
        {
            AbsorbAndDispense();
        }
    }

    private void AbsorbAndDispense()
    //This is the main function for controlling the pipette. It has been modified to include the LockChemFluid and lockDispenseAmount features.
    //LockChemFluid makes it so the pipette can only be filled once, and then will only dispense that exact chemical mixture. May need to create some way to empty it in the future, but this can always be toggled off via the Serialized Field in the Unity inspector.
    //LockDispenseAmount forces the pipette to only be able to fill a specific fluid and amount to any container it fills, resetting whatever mixture is actively in that container and replacing it with the pipette's. This is to prevent the users from overfilling
    //the volumetric flasks in the Glassware Use Module. This feature can cause problems if the pipette is used on unintended containers, so if this becomes problematic, just deactivate the Serialized Field in the inspector.
    {
        if (!lockDispenseAmount)
        {
            if (!LockChemFluid)
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
            if (LockChemFluid)
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
                        //This line is commented out for the Lock Chem feature, to force the pipette to always use the same chemical once it has been filled once.
                        //canDispense = false;

                        Debug.Log($"Dispensed {dispenseAmount} of {currentFluids} into container.");
                        //Prevents the internal fluid object from hiding since this pipette will never be empty.
                        //internalFluid.gameObject.SetActive(false); // Hide the internal fluid after dispensing
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
        }
        else if (lockDispenseAmount)
        {
            if (!LockChemFluid)
            {
                if (canDispense)
                {
                    if (isOverlapping)
                    {
                        ChemContainer container = currentContainer.GetComponent<ChemContainer>();
                        container.EmptyChem(); //Additional line of code for specifically the lock dispense amount.
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
            if (LockChemFluid)
            {
                if (canDispense)
                {
                    if (isOverlapping)
                    {
                        ChemContainer container = currentContainer.GetComponent<ChemContainer>();
                        container.EmptyChem(); //Additional line of code for specifically the lock dispense amount.
                        for (int i = 0; i < currentFluids.GetChemArray().Length; i++)
                        {
                            container.AddChem(currentFluids.GetChemArray()[i].type, currentFluids.GetChemArray()[i].volume);
                        }
                        container.UpdateChem();
                        GameEventsManager.instance.chemistryEvents.PipetteDispense(this, container, currentFluids);
                        GameEventsManager.instance.chemistryEvents.PourIn(container, container.GetChemFluid());
                        //This line is commented out for the Lock Chem feature, to force the pipette to always use the same chemical once it has been filled once.
                        //canDispense = false;

                        Debug.Log($"Dispensed {dispenseAmount} of {currentFluids} into container.");
                        //Prevents the internal fluid object from hiding since this pipette will never be empty.
                        //internalFluid.gameObject.SetActive(false); // Hide the internal fluid after dispensing
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



