using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class ChemContainer : MonoBehaviour {
    [System.Serializable]
    public struct Defaults {
        public Defaults(float volume, float rate, float angle) {
            originalVolume = volume;
            originalPourRate = rate;
            originalPourAngle = angle;
        }
        [Header("Do Not Edit (used for restoring state on certain flag changes)")]
        public float originalVolume;
        public float originalPourRate;
        public float originalPourAngle;
    }

    [System.Serializable]
    public struct PourPhysicsSettings {
        public PourPhysicsSettings(float minRate, float maxRate, float minAngle, float maxAngle, float minSize, float maxSize, float minSpeed, float maxSpeed) {
            minPourRate = minRate;
            maxPourRate = maxRate;
            minPourAngle = minAngle;
            maxPourAngle = maxAngle;
            minParticleSize = minSize;
            maxParticleSize = maxSize;
            minParticleEmissionRate = minSpeed;
            maxParticleEmissionRate = maxSpeed;
        }
        [Header("Pour Rate")]
        public float minPourRate;
        public float maxPourRate;
        [Header("Pour Angle")]
        public float minPourAngle;
        public float maxPourAngle;
        [Header("Pour Particle Settings")]
        public float minParticleSize;
        public float maxParticleSize;
        public float minParticleEmissionRate;
        public float maxParticleEmissionRate;
    }

    [System.Serializable]
    public struct Flags {
        public Flags(bool anglePhysics, bool ratePhysics, bool activator, bool simple, bool source, bool sink) {
            pourAngleUsesPhysics = anglePhysics;
            pourRateUsesPhysics = ratePhysics;
            pouringUsesActivator = activator;
            useSimpleFluidLevel = simple;
            infiniteFluid = source;
            infiniteCapacity = sink;
        }
        [Header("Behavior Modifiers")]
        [Tooltip("Denotes whether or not pouring is triggered by activation of another object (e.g., faucet, spout, button, etc). When set to true, a Pour Activator must be specified.")]
        public bool pouringUsesActivator;
        [Tooltip("When enabled, pouring from this container will not remove any ChemFluid or decrease its volume.\nAdditionally, the ResizeFluid's fill property is always 100%.")]
        public bool infiniteFluid;
        [Tooltip("When enabled, pouring into this container is always possible and will not add any ChemFluid or increase its volume.\nPouring into a container with this flag enabled effectively disposes of the fluid.")]
        public bool infiniteCapacity;
        [Header("Experimental")]
        [Tooltip("[EXPERIMENTAL] When enabled, pourAngle is parametrized by fill percentage. This produces a more realistic effect where pouring requires less tilt when a container is more full.")]
        public bool pourAngleUsesPhysics;
        [Tooltip("[EXPERIMENTAL] When enabled, pourRate increases based on how far the container is tilted past the pourAngle. This also increases the magnitude of the pouring Particle System effect.")]
        public bool pourRateUsesPhysics;
        [Header("For Development/Debug Use Only")]
        [Tooltip("Useful for debugging.\nWhen enabled, fluid level uses the value of currentVolume in the Inspector rather than the volume of the ChemFluid.\n" +
            "Pouring is only allowed when both origin and recipient containers have the same useSimpleFluidLevel setting.\nSimple pouring does not generate ChemistryEvents.")]
        public bool useSimpleFluidLevel;        
    }

    [Header("CONTAINER COMPONENTS")]
    public ResizeFluid internalFluid;
    public GameObject pourPoint;
    public GameObject opening;
    public ParticleSystem pourEffect;
    public PourActivator pourActivator;

    [Header("CONTAINER PROPERTIES")]
    [Tooltip("The maximum volume (in mL) of fluid that this container can hold")]
    public float maxVolume;
    [Tooltip("The current volume of fluid in the container")]
    public float currentVolume;
    [Tooltip("The rate (in mL/s) at which fluid pours")]
    public float pourRate;
    [Tooltip("The angle (from the world +y-axis) at which to activate pouring")]
    public float pourAngle = 90;

    [Header("POUR TARGETING")]
    [Tooltip("Radius (in metres) of the cast used to find what this container is pouring into.\n" +
             "A zero-radius ray requires the pour point to be lined up exactly, which makes " +
             "narrow-necked glassware very hard to aim at. Raise this to make pouring more forgiving.\n\n" +
             "When several containers fall inside the radius, the one nearest the pour point's " +
             "vertical axis wins, so widening this does not make aiming vaguer - it just adds slack.")]
    public float pourCastRadius = 0.06f;
    [Tooltip("Layers the pour cast considers. Must include \"Chem\", the layer container openings sit on.\n" +
             "Leave empty to use \"Chem\" only.")]
    public LayerMask pourCastMask = 0;
    [Tooltip("How far below the pour point a container can be and still receive the pour.\n" +
             "The cast only looks at the Chem layer, so solid geometry no longer blocks it - this " +
             "is what stops you pouring through a bench into something stored underneath.")]
    public float pourCastMaxDistance = 1.5f;
    [Tooltip("Fraction of pourAngle at which the aim guide appears, so the player can line up " +
             "before fluid starts flowing. Only used when a PourAimGuide is attached.")]
    [Range(0f, 1f)] public float aimGuideTiltFraction = 0.5f;


    [Header("DEV OPTIONS")]
    [SerializeField, Tooltip("Various settings for changing the behavior of the ChemContainer.")]
    private Flags flags = new Flags(false, false, false, false, false, false);
    [SerializeField, Tooltip("Settings for the allowable ranges of physics-based pourRate and pourAngle, as well as particle effect settings.")]
    private PourPhysicsSettings pourPhysics = new PourPhysicsSettings(5, 50, 25, 115, 0.05f, 0.2f, 50, 100);
    [SerializeField, Tooltip("Do not edit.\nUsed for restoring state on certain flag changes.")]
    private Defaults defaults = new Defaults(0, 5, 90); //These values are overwritten in Start()

    [Header("FLUID INITIALIZATION")]
    [SerializeField, Tooltip("Toggle this to reset the ChemFluid and re-initialize it with the Chems in initialContents.\nVolume constraints still apply.\n\nOnly works in Play mode.")]
    private bool reInitialize = false;
    [SerializeField, Tooltip("Specify the initial contents that the container will hold.\n[NOTE: If the volume of initial contents exceeds container's max volume, no initial contents will be added.]")]
    private Chem[] initialContents;

    [Header("CHEM FLUID\n - Do not edit these values directly.\n - Use initialContents to set initial ChemFluid contents.\n - Use \"Re Initialize\" (above) to modify contents while in Play mode.")]
    [SerializeField]
    private ChemFluid chemFluid;

    
    // Start is called before the first frame update
    void Start() {
        defaults.originalVolume = currentVolume;
        defaults.originalPourRate = pourRate;
        defaults.originalPourAngle = pourAngle;

        // Check that pourActivator is assigned if pouringUsesActivator is true
        if (flags.pouringUsesActivator && pourActivator == null) {
            Debug.LogWarning("This ChemContainer is set to pour with an activator but no activator is specified. pouringUsesActivator will be set to false.");
            flags.pouringUsesActivator = false;
        }

        // Check that initialContents do not exceed maxVolume, and initialize chemFluid 
        float sum = initialContents.Sum(chem => chem.volume);
        if (sum > maxVolume) {
            Debug.LogWarning($"Initial contents exceed maxVolume of this ChemContainer {{{gameObject.name}}}. No contents will be added.\n" +
                            $"{sum} (initial contents volume)\n" +
                            $"{maxVolume} (max volume)");
            chemFluid = new ChemFluid();
        } else {
            chemFluid = new ChemFluid(initialContents);
        }

        // If not using simple chemFluid level, set currentVolume to volume of ChemFluid
        if (!flags.useSimpleFluidLevel) {
            currentVolume = chemFluid.totalVolume;
        }

        // Check that the opening collider has the correct layer assigned
        if (opening.layer != LayerMask.NameToLayer("Chem")) {
            Debug.LogWarning($"The opening of this ChemContainer {{{gameObject.name}}} does not have the correct Layer ({LayerMask.NameToLayer("Chem")}: \"Chem\") assigned.\n" +
                             $"The layer must be set correctly for pouring detection to work for this ChemContainer.");
        }

        // Check that an internalFluid has been specified, and initialize its fill and color properties
        if (internalFluid == null) {
            Debug.LogWarning($"This ChemContainer {{{gameObject.name}}} does not have an internalFluid set.");
        } else {
            internalFluid.fill = (flags.infiniteFluid) ? 1 : currentVolume / maxVolume;
            Color chemColor = ChemistryManager.instance.GetColor(chemFluid);
            internalFluid.color = chemColor;
        }
        
        // Check that a pourPoint has been set
        if (pourPoint == null) {
            Debug.LogWarning($"This ChemContainer {{{gameObject.name}}} does not have a pourPoint set.");
        }

        // Check that a pourEffect particle system has been set, and generate a dummy one if not.
        if (pourEffect == null) {
            Debug.LogWarning($"This ChemContainer {{{gameObject.name}}} does not have a pourEffect Particle System set.");
            GameObject dummyParticleSystem = new GameObject();
            dummyParticleSystem.name = $"dummy particle system {gameObject.name}";
            dummyParticleSystem.transform.parent = transform;
            dummyParticleSystem.AddComponent<ParticleSystem>();
            pourEffect = dummyParticleSystem.GetComponent<ParticleSystem>();
        } else {
            pourEffect.gameObject.transform.localPosition = pourPoint.transform.localPosition;
            pourEffect.gameObject.SetActive(true);
        }

        // Optional pour feedback. Both are opt-in: add the component to make it appear.
        aimGuide = GetComponent<PourAimGuide>();

        // Held tracking, so the aim guide only appears while the player has the container.
        // VR selection is read straight off the interactable, which needs no inspector wiring.
        grabInteractable = GetComponent<XRBaseInteractable>();

        // WebGL has no interactable selection - WebGLGrab reparents the object and raises these.
        if (GameEventsManager.instance != null && GameEventsManager.instance.webGLEvents != null) {
            GameEventsManager.instance.webGLEvents.OnObjectGrabbed += WebGLGrabbed;
            GameEventsManager.instance.webGLEvents.OnObjectReleased += WebGLReleased;
        }
    }

    private void OnDestroy() {
        if (GameEventsManager.instance != null && GameEventsManager.instance.webGLEvents != null) {
            GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= WebGLGrabbed;
            GameEventsManager.instance.webGLEvents.OnObjectReleased -= WebGLReleased;
        }
    }

    // WebGLGrab hands us whichever collider the player's raycast hit, which may be a child.
    private void WebGLGrabbed(GameObject grabbed) {
        if (grabbed != null && grabbed.GetComponentInParent<ChemContainer>() == this) webGLHeld = true;
    }

    private void WebGLReleased(GameObject released) {
        if (released != null && released.GetComponentInParent<ChemContainer>() == this) webGLHeld = false;
    }

    // Update is called once per frame
    void Update() {
        if (currentVolume == 0) {
            internalFluid.gameObject.SetActive(false);
        } else {
            internalFluid.gameObject.SetActive(true);
        }
        internalFluid.fill = (flags.infiniteFluid) ? 1 : currentVolume / maxVolume;
        Color chemColor = flags.useSimpleFluidLevel ? ChemistryManager.instance.GetColor(ChemType.WATER) : ChemistryManager.instance.GetColor(chemFluid);
        internalFluid.color = chemColor;
        ParticleSystem.MainModule psmain = pourEffect.main;
        psmain.startColor = chemColor;
    }

    private void FixedUpdate() {
        if (flags.pourAngleUsesPhysics) {
            // linear interpolation between two scalars. This might need to be changed to a different function shape later.
            pourAngle = pourPhysics.maxPourAngle - ((currentVolume / maxVolume) * (pourPhysics.maxPourAngle - pourPhysics.minPourAngle));            
        } else {
            pourAngle = defaults.originalPourAngle;
        }
        float tilt = Vector3.Angle(Vector3.up, transform.up);
        bool pouringPossible = false;
        if (flags.pouringUsesActivator) {
            if (pourActivator.IsActivated()) {
                pouringPossible = true;
            }
        } else {
            if (tilt >= pourAngle) {
                pouringPossible = true;
            }
        }
        // Find what is underneath the pour point. This uses a thick cast rather than a thin ray
        // so the player does not have to line the pour point up exactly - tilting to pour swings
        // the pour point through an arc, so an exact-aim requirement fights the pour gesture.
        RaycastHit hit;
        ChemContainer recipient;
        bool hasTarget = TryFindPourTarget(out hit, out recipient);
        pouringPossible = pouringPossible && hasTarget;

        // Aim feedback is driven by the same test the pour uses, so the guide and the highlight
        // are truthful: if they say the pour will land, it lands.
        // Tilt-poured containers only show feedback while actually in hand - a bottle left
        // resting at an angle on a bench should not sit there drawing a guide. Activator-poured
        // containers (the burette) are fixed in a holder, so an open stopcock is intent enough.
        bool aiming = flags.pouringUsesActivator
            ? (pourActivator != null && pourActivator.IsActivated())
            : (IsHeld && tilt >= pourAngle * aimGuideTiltFraction);
        UpdatePourFeedback(aiming, hasTarget, hit, recipient);

        if (pouringPossible) {
            float particleSize = 0.1f;
            float particleScale = 1.0f;
            float particleEmissionRate = 50f;
            if (flags.pourRateUsesPhysics) {
                pourRate = (((tilt - pourAngle) / (180 - pourAngle)) * (pourPhysics.maxPourRate - pourPhysics.minPourRate)) + pourPhysics.minPourRate;
                particleScale = (pourRate - pourPhysics.minPourRate) / (pourPhysics.maxPourRate - pourPhysics.minPourRate);
                particleSize = pourPhysics.minParticleSize + (particleScale * (pourPhysics.maxParticleSize - pourPhysics.minParticleSize));
                particleEmissionRate = pourPhysics.maxParticleEmissionRate - (particleScale * (pourPhysics.maxParticleEmissionRate - pourPhysics.minParticleEmissionRate));
            } else {
                pourRate = defaults.originalPourRate;
            }


            // recipient is guaranteed non-null here: TryFindPourTarget only reports a target when
            // it resolves to a ChemContainer, which also fixes the intermittent null deref that
            // used to throw when a container was dropped mid-pour.
            {
                float amountPoured = Mathf.Min(pourRate * Time.fixedDeltaTime, currentVolume, (recipient.flags.infiniteCapacity ? float.MaxValue : recipient.maxVolume - recipient.currentVolume));

                //Adjust amount poured to match how much the turner is opened
                if (flags.pouringUsesActivator)
                {
                    if (pourActivator.isPercentActivated)
                    {
                        amountPoured *= pourActivator.getFlow();
                    }
                }

                if (amountPoured > 0 && flags.useSimpleFluidLevel == recipient.flags.useSimpleFluidLevel) {

                    if (flags.useSimpleFluidLevel) {
                        if (!flags.infiniteFluid) {
                            currentVolume -= amountPoured;
                        }
                        if (!recipient.flags.infiniteCapacity) {
                            recipient.currentVolume += amountPoured;
                        }
                    } else {
                        ChemFluid portion = chemFluid.PortionFromVolume(amountPoured);
                        if (!flags.infiniteFluid) {
                            chemFluid.Remove(portion);
                            GameEventsManager.instance.chemistryEvents.PourOut(this, portion);
                        }
                        if (recipient.flags.infiniteCapacity) {
                            GameEventsManager.instance.chemistryEvents.FluidDispose(recipient, portion);
                        } else {
                            recipient.chemFluid.Add(portion);
                            GameEventsManager.instance.chemistryEvents.PourIn(recipient, portion);
                        }
                        currentVolume = chemFluid.totalVolume;
                        recipient.currentVolume = recipient.chemFluid.totalVolume;
                    }
                    ParticleSystem.MainModule psmain = pourEffect.main;
                    ParticleSystem.EmissionModule psemit = pourEffect.emission;

                    psmain.startSize = particleSize;
                    psemit.rateOverTime = particleEmissionRate;
                    psmain.startLifetime = hit.distance + 0.1f;

                    pourEffect.Play();
                } else {
                    if (flags.useSimpleFluidLevel != recipient.flags.useSimpleFluidLevel) {
                        Debug.LogWarning("Unable to pour between two ChemContainers with different UseSimpleFluidLevel settings.");
                    }
                    pourEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        } else {
            pourEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    // Shared scratch buffer so the per-frame cast in FixedUpdate does not allocate.
    // Safe to share: FixedUpdate is single-threaded and results are consumed immediately.
    private static readonly RaycastHit[] pourCastHits = new RaycastHit[16];

    private PourAimGuide aimGuide;
    private PourTargetHighlight litTarget;

    private XRBaseInteractable grabInteractable;
    private bool webGLHeld;

    /// <summary>
    /// True while the player is holding this container, on either platform.
    /// </summary>
    public bool IsHeld => (grabInteractable != null && grabInteractable.isSelected) || webGLHeld;

    private int PourCastMask =>
        (pourCastMask.value != 0) ? pourCastMask.value : (1 << LayerMask.NameToLayer("Chem"));

    private float PourCastRadius => Mathf.Max(pourCastRadius, 0.001f);

    /// <summary>
    /// Finds the ChemContainer this one is currently positioned to pour into.
    /// Casts a sphere straight down from the pour point, ignoring this container's own colliders.
    ///
    /// Where several containers fall inside the cast, the winner is the one whose opening is
    /// closest to the pour point's vertical axis - not the one the sphere happens to reach first.
    /// Sphere-cast distance is travel along the ray, so without this a container sitting lower but
    /// well off to one side would beat the one you are holding the bottle directly over. Selecting
    /// on lateral offset is what makes a generous pourCastRadius safe next to crowded glassware.
    /// </summary>
    private bool TryFindPourTarget(out RaycastHit hit, out ChemContainer recipient) {
        hit = default;
        recipient = null;
        float bestOffset = float.MaxValue;

        Vector3 origin = pourPoint.transform.position;
        int count = Physics.SphereCastNonAlloc(
            origin, PourCastRadius, Vector3.down,
            pourCastHits, Mathf.Max(pourCastMaxDistance, 0.01f), PourCastMask,
            QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++) {
            RaycastHit candidate = pourCastHits[i];
            if (candidate.collider == null) continue;

            ChemContainer candidateContainer = candidate.collider.GetComponentInParent<ChemContainer>();
            // The cast starts at our own lip, so without this we would always hit ourselves first.
            if (candidateContainer == null || candidateContainer == this) continue;

            // Horizontal distance from the pour axis to the target's opening.
            Vector3 target = (candidateContainer.opening != null)
                ? candidateContainer.opening.transform.position
                : candidate.point;
            float offset = Vector2.Distance(new Vector2(origin.x, origin.z),
                                            new Vector2(target.x, target.z));

            if (offset < bestOffset) {
                bestOffset = offset;
                hit = candidate;
                recipient = candidateContainer;
            }
        }

        return recipient != null;
    }

    /// <summary>
    /// Drives the optional aim guide on this container and the optional highlight on whatever
    /// it is aimed at. Both are no-ops unless the relevant component has been attached.
    /// </summary>
    private void UpdatePourFeedback(bool aiming, bool hasTarget, RaycastHit hit, ChemContainer recipient) {
        if (aimGuide != null) {
            aimGuide.SetAim(aiming, pourPoint.transform.position, hit.point, hasTarget);
        }

        PourTargetHighlight next = (aiming && hasTarget && recipient != null)
            ? recipient.GetComponent<PourTargetHighlight>()
            : null;

        if (next == litTarget) return;
        // Requests are made in this container's name so that dropping ours never cancels a glow
        // another container - or a pipette lined up on the same target - still wants.
        if (litTarget != null) litTarget.SetHighlighted(this, false, litTarget.highlightColor);
        litTarget = next;
        if (litTarget != null) litTarget.SetHighlighted(this, true, litTarget.highlightColor);
    }

    private void OnDisable() {
        // Do not leave a target glowing if this container is disabled or destroyed mid-pour.
        if (litTarget != null) {
            litTarget.SetHighlighted(this, false, litTarget.highlightColor);
            litTarget = null;
        }
        if (aimGuide != null) {
            aimGuide.SetAim(false, Vector3.zero, Vector3.zero, false);
        }
    }

    // Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector.
    public void OnValidate() {
        // We do not want the particle effect to play in the editor because it's annoying
        if (pourEffect != null) {
            if (!Application.IsPlaying(gameObject)) {
                pourEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                pourEffect.gameObject.SetActive(false);
            }
        }
        
        // Re-Initialize ChemFluid from initialContents
        if (reInitialize) {
            reInitialize = false;
            if (Application.IsPlaying(gameObject)) {
                Debug.Log($"Re-initializing contents of ChemFluid in ChemContainer {{{gameObject.name}}}");
                if (initialContents.Sum(chem => chem.volume) > maxVolume) {
                    Debug.LogWarning($"Initial contents exceed maxVolume of this ChemContainer {{{gameObject.name}}}. No contents will be added.");
                } else {
                    chemFluid.SetToEmpty();
                    chemFluid.Add(new ChemFluid(initialContents));
                    currentVolume = chemFluid.totalVolume;
                }
            } else {
                Debug.Log("Unable to re-initialize ChemFluid in Editor mode. Re-initialize only works in Play mode.");
            }            
        }
    }

    public string getContents()
    {
        return chemFluid.ContentsToString();
    }
    public Chem[] GetChemFluid(float pipetteVolume)
    {
        Chem[] currentChems = chemFluid.GetChemArray();
        Chem[] adjustedVolumes = new Chem[currentChems.Length];

        for (int i = 0; i < currentChems.Length; i++)
        {
            adjustedVolumes[i] = new Chem(currentChems[i].type, pipetteVolume * (currentChems[i].volume / chemFluid.totalVolume));
        }
       return adjustedVolumes;
    }

    public ChemFluid GetChemFluid() => chemFluid;

    public void SelectEnter(XRBaseInteractable interactable)
    {
        GameEventsManager.instance.interactableEvents.PlayerGrabInteractable(gameObject);
    }

    public void SelectExit(XRBaseInteractable interactable) {
        GameEventsManager.instance.interactableEvents.PlayerDropInteractable(gameObject);
    }

    public void AddChem(ChemType type, float amount) => chemFluid.Add(type, amount);

    public void UpdateChem()
    {
        currentVolume = chemFluid.totalVolume;
        internalFluid.fill = (flags.infiniteFluid) ? 1 : currentVolume / maxVolume;
    }
    public void EmptyChem() => chemFluid.SetToEmpty();
    public void SetChem(ChemFluid setChem) => chemFluid.AssignNewChemFluid(setChem);

    public float GetPourAngle()
    {
        return pourAngle;
    }

}
