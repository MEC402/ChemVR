using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

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
        RaycastHit hit;
        if (Physics.Raycast(pourPoint.transform.position, Vector3.down, out hit, Mathf.Infinity)) {
            pouringPossible = pouringPossible && (hit.collider.gameObject.layer == LayerMask.NameToLayer("Chem"));
        }
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


            if (hit.collider != null) //Added all of this into this if statement to try to catch the bug that is occassionally thrown.
            {
                //the receiving obj has to have the chemContainer script attatched? AW
                ChemContainer recipient = hit.collider.gameObject.GetComponentInParent<ChemContainer>();            // BUG! Every now and again if you drop a chem container a null error throws here. I'm trying to find it above.
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

    public void SelectEnter(XRBaseInteractable interactable) {
        GameEventsManager.instance.interactableEvents.PlayerGrabInteractable(gameObject);
    }

    public void SelectExit(XRBaseInteractable interactable) {
        GameEventsManager.instance.interactableEvents.PlayerDropInteractable(gameObject);
    }
}
