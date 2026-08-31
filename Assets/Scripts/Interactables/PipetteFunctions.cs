using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PipetteFunctions : MonoBehaviour
{
    private bool canDispense = false;
    private bool isOverlapping = false;
    private GameObject currentContainer;
    [SerializeField] private ResizeFluid internalFluid;
    [SerializeField] private GameObject bulbCollider;
    [SerializeField] private bool LockChemFluid = false;
    [SerializeField] private bool lockDispenseAmount = false;
    public bool isHeld = false;

    [SerializeField]private float dispenseAmount = 10f; // Amount to dispense per interaction
    [SerializeField] public ChemFluid currentFluids;

    [Header("AIM FEEDBACK")]
    [SerializeField, Tooltip("Light up the pipette and whatever it is lined up with whenever using " +
        "the pipette (F on desktop, trigger in VR) would actually do something.")]
    private bool showAimHighlight = true;
    [SerializeField, Tooltip("Glow colour while the pipette is empty and lined up to draw fluid.")]
    private Color drawHighlightColor = new Color(0f, 0.9559735f, 0.9646866f); // matches Materials/Highlight.mat
    [SerializeField, Tooltip("Glow colour while the pipette is loaded and lined up to dispense.")]
    private Color dispenseHighlightColor = new Color(0.25f, 1f, 0.4f);
    [SerializeField, Range(1f, 8f), Tooltip("Multiplier on the glow. Above 1 the emission runs hot, " +
        "which is what makes a valid line-up read as a strong glow rather than a faint tint.")]
    private float glowIntensity = 3f;
    [SerializeField, Tooltip("Object whose renderers glow. Left empty, the pipette body this " +
        "collider belongs to is used.")]
    private Transform highlightRoot;

    // Every ChemContainer collider the tip is currently inside. A list rather than a single
    // reference because the tip can straddle two containers, and because one container can carry
    // several colliders - leaving one of them must not read as leaving the container.
    private readonly List<ChemContainer> overlappingContainers = new List<ChemContainer>();
    private PourTargetHighlight pipetteGlow;
    private PourTargetHighlight targetGlow;

    // Hand tracking variables
    private enum HandType { None, Left, Right }
    private HandType currentHand = HandType.None;

    //Adding and removing task reset event listener.
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        ClearAimHighlight();
        if (ResetTaskManager.instance != null)
        {
            ResetTaskManager.instance.onResetCalled -= ResetPipette;
        }
    }

    private void Update()
    {
        UpdateAimHighlight();
    }

    private void Start()
    {
        ResetTaskManager.instance.onResetCalled += ResetPipette;
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
        bulbCollider.SetActive(true); // Enable the bulb collider when grabbed

    }

    public void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        DisableButtonListeners();
        currentHand = HandType.None;
        bulbCollider.SetActive(false); // Disable the bulb collider when released
        ClearAimHighlight(); // A pipette on the bench is not aiming at anything
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

    #region Aim Feedback
    /// <summary>
    /// The container the pipette is lined up with and could actually act on right now, or null.
    /// Where the tip straddles two, the one whose opening is nearest wins.
    /// </summary>
    private ChemContainer CurrentTarget()
    {
        ChemContainer best = null;
        float bestDistance = float.MaxValue;

        for (int i = overlappingContainers.Count - 1; i >= 0; i--)
        {
            ChemContainer candidate = overlappingContainers[i];
            if (candidate == null)
            {
                overlappingContainers.RemoveAt(i); // Destroyed with the tip still inside it
                continue;
            }
            if (!CanInteractWith(candidate)) continue;

            Vector3 reference = (candidate.opening != null)
                ? candidate.opening.transform.position
                : candidate.transform.position;
            float distance = (reference - transform.position).sqrMagnitude;

            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = candidate;
            }
        }

        return best;
    }

    /// <summary>
    /// Whether pressing use right now would do something with this container. Absorbing needs
    /// fluid to draw - without this check an empty container hands back NaN volumes, which would
    /// quietly poison the pipette's contents.
    /// </summary>
    private bool CanInteractWith(ChemContainer container)
    {
        if (container == null || !container.isActiveAndEnabled) return false;
        if (canDispense) return true; // Loaded: any container can receive it

        ChemFluid contents = container.GetChemFluid();
        return contents != null && contents.totalVolume > 0f;
    }

    /// <summary>
    /// Lights the pipette and its current target while a use would succeed, so the player can see
    /// that the key will work before pressing it. Driven off the same test the use itself runs.
    /// </summary>
    private void UpdateAimHighlight()
    {
        ChemContainer target = (showAimHighlight && isHeld) ? CurrentTarget() : null;

        // Alpha is meaningless for emission, so scaling the whole colour is safe.
        Color glow = (canDispense ? dispenseHighlightColor : drawHighlightColor) * glowIntensity;

        PourTargetHighlight next = (target != null) ? HighlightOn(target.gameObject) : null;
        if (next != targetGlow)
        {
            if (targetGlow != null) targetGlow.SetHighlighted(this, false, glow);
            targetGlow = next;
        }
        if (targetGlow != null) targetGlow.SetHighlighted(this, true, glow);

        // Only build the pipette's own highlight once it has something to react to, so pipettes
        // sitting untouched on a bench never instance materials they will not use.
        PourTargetHighlight self = (target != null) ? PipetteHighlight() : pipetteGlow;
        if (self != null) self.SetHighlighted(this, target != null, glow);
    }

    private void ClearAimHighlight()
    {
        if (targetGlow != null) targetGlow.SetHighlighted(this, false, Color.black);
        targetGlow = null;
        if (pipetteGlow != null) pipetteGlow.SetHighlighted(this, false, Color.black);
    }

    /// <summary>
    /// The highlight on the pipette body. PipetteFunctions sits on the tip collider, whose own
    /// renderer is switched off, so the glow has to go on the body the tip belongs to.
    /// </summary>
    private PourTargetHighlight PipetteHighlight()
    {
        if (pipetteGlow != null) return pipetteGlow;

        Transform root = highlightRoot;
        if (root == null)
        {
            Rigidbody body = GetComponentInParent<Rigidbody>();
            root = (body != null) ? body.transform
                 : (transform.parent != null) ? transform.parent
                 : transform;
        }

        pipetteGlow = HighlightOn(root.gameObject);
        return pipetteGlow;
    }

    /// <summary>
    /// Finds the object's highlight, adding one where it has none. Adding on demand is what lets
    /// every container in every scene light up without hand-wiring each prefab.
    /// </summary>
    private static PourTargetHighlight HighlightOn(GameObject target)
    {
        if (target == null) return null;
        PourTargetHighlight existing = target.GetComponent<PourTargetHighlight>();
        return (existing != null) ? existing : target.AddComponent<PourTargetHighlight>();
    }
    #endregion

    private void AbsorbAndDispense()
    //This is the main function for controlling the pipette. It has been modified to include the LockChemFluid and lockDispenseAmount features.
    //LockChemFluid makes it so the pipette can only be filled once, and then will only dispense that exact chemical mixture. May need to create some way to empty it in the future, but this can always be toggled off via the Serialized Field in the Unity inspector.
    //LockDispenseAmount forces the pipette to only be able to fill a specific fluid and amount to any container it fills, resetting whatever mixture is actively in that container and replacing it with the pipette's. This is to prevent the users from overfilling
    //the volumetric flasks in the Glassware Use Module. This feature can cause problems if the pipette is used on unintended containers, so if this becomes problematic, just deactivate the Serialized Field in the inspector.
    {
        // Resolve the target through the same test the highlight uses, so what glows is exactly
        // what a press acts on - and nothing else gets acted on.
        ChemContainer target = CurrentTarget();
        isOverlapping = target != null;
        currentContainer = isOverlapping ? target.gameObject : null;

        if (!isOverlapping)
        {
            Debug.LogWarning(canDispense
                ? "Cannot dispense: Not lined up with a ChemContainer."
                : "Cannot absorb: Not lined up with a ChemContainer that holds fluid.");
            return;
        }

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


    //Used to reset pipettes during checkpoint reset. Assigned to the onResetCalled event from the Reset Task Manager.
    private void ResetPipette(object sender, EventArgs e)
    {
        if(canDispense)
        {
            currentFluids.SetToEmpty();
            internalFluid.gameObject.SetActive(false);
            canDispense = false;
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        ChemContainer container = other.GetComponent<ChemContainer>();
        if (container == null) return;

        // One entry per collider, so a container with several colliders is only forgotten once
        // the tip has left all of them.
        overlappingContainers.Add(container);
        isOverlapping = true;
        currentContainer = container.gameObject; // Store the current container reference
    }
    private void OnTriggerExit(Collider other)
    {
        ChemContainer container = other.GetComponent<ChemContainer>();
        if (container == null) return;

        overlappingContainers.Remove(container);
        if (!overlappingContainers.Contains(container) && currentContainer == container.gameObject)
        {
            isOverlapping = overlappingContainers.Count > 0;
            currentContainer = null; // Clear the current container reference
        }
    }
}



