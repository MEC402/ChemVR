# Pour Aiming — Change Status

**Branch:** `stevec_dev`
**Commit:** `1ea70cd9` — *"added larger ray and path guides"* (2026-08-07)
**Status:** Committed and wired up in `WebLabGlassware-Zack.unity`. Not yet play-tested to a conclusion.

---

## The problem

Pouring the soap bottle into the 65 mL volumetric flask was extremely hard to line up. Players
could not reliably get the stream to register, even when the bottle looked correctly positioned
over the flask.

## Root cause

The difficulty was not in the flask's collider. It was the pour detection in
`ChemContainer.FixedUpdate()`:

```csharp
RaycastHit hit;
if (Physics.Raycast(pourPoint.transform.position, Vector3.down, out hit, Mathf.Infinity)) {
    pouringPossible = pouringPossible && (hit.collider.gameObject.layer == LayerMask.NameToLayer("Chem"));
}
```

Four issues compounded:

1. **Zero-radius ray.** An infinitely thin ray meant the pour point had to be lined up exactly.
   There was no tolerance at all.

2. **Very small target.** The 65 mL flask's `Opening` is a unit sphere scaled
   `0.0513 × 0.00517 × 0.0513` — roughly 5 cm across. It is a volumetric flask, so the neck is
   deliberately narrow.

3. **Aim and pour gesture fight each other.** The soap bottle has `pouringUsesActivator: 0` and
   `pourAngle: 90`, so it only pours once tilted a full 90°. The ray fires downward in *world*
   space from `pourPoint.transform.position`, and tilting swings that point through an arc. The
   act of tilting to pour moves the aim away from wherever it was placed.

4. **No layer mask, first hit wins.** `Physics.Raycast` with no mask returned the nearest collider
   on any layer. The flask's own neck geometry, the tray, or any stray collider between the pour
   point and the opening became the hit, and the layer check then failed silently.

---

## Changes made

### 1. `Assets/Scripts/Interactables/ChemContainer.cs` — pour detection

Replaced the thin ray with a bounded sphere cast that resolves a real recipient:

```csharp
bool hasTarget = TryFindPourTarget(out hit, out recipient);
pouringPossible = pouringPossible && hasTarget;
```

`TryFindPourTarget` casts a sphere straight down from the pour point, masked to the `Chem` layer,
and skips any hit belonging to this same container. It addresses all four issues: real tolerance,
occluding geometry can no longer eat the hit, the bottle cannot target its own opening, and the
recipient is guaranteed non-null.

**Target selection is by lateral offset, not cast distance.** Sphere-cast distance measures travel
along the ray, so a container sitting lower but well off to one side would beat the one the bottle
is directly over. The winner is instead the container whose `Opening` is closest to the pour
point's vertical axis:

```csharp
Vector3 target = (candidateContainer.opening != null)
    ? candidateContainer.opening.transform.position
    : candidate.point;
float offset = Vector2.Distance(new Vector2(origin.x, origin.z),
                                new Vector2(target.x, target.z));
```

This is what allows a generous radius without pouring into neighbouring glassware — widening the
cast adds slack without making aiming vaguer.

**New inspector fields on `ChemContainer`:**

| Field | Default | Purpose |
|---|---|---|
| `pourCastRadius` | `0.06` | Aiming tolerance in metres. 12 cm band against the flask's ~5 cm opening |
| `pourCastMask` | empty → `Chem` | Layers the cast considers |
| `pourCastMaxDistance` | `1.5` | Stops pouring through a bench into something stored below |
| `aimGuideTiltFraction` | `0.5` | When the guide appears, as a fraction of `pourAngle` |

All have C# field initializers, so existing prefabs pick up working defaults without needing
re-serialization.

**Held tracking.** The aim feedback only appears while the player is actually holding the
container, on either platform, with no inspector wiring required:

```csharp
public bool IsHeld => (grabInteractable != null && grabInteractable.isSelected) || webGLHeld;
```

- **VR** — read from `XRBaseInteractable.isSelected` (XRI 2.6.4).
- **WebGL** — there is no interactable selection, so it subscribes to
  `webGLEvents.OnObjectGrabbed` / `OnObjectReleased`, matching the idiom in `SnapBulbToPipet` and
  `JarLid`. `WebGLGrab` passes whichever collider the raycast hit, which may be a child, so the
  handlers resolve through `GetComponentInParent<ChemContainer>()`.

Activator-poured containers (the burette) are exempt from the held requirement — they sit in a
holder and are never held, so an open stopcock is treated as intent on its own.

**Incidental fix.** The old code had a known intermittent crash, flagged in a source comment:

```csharp
ChemContainer recipient = hit.collider.gameObject.GetComponentInParent<ChemContainer>();
// BUG! Every now and again if you drop a chem container a null error throws here.
```

`TryFindPourTarget` only reports a target once it has resolved to a `ChemContainer`, so `recipient`
can no longer be null at that point.

### 2. `Assets/Scripts/Interactables/PourAimGuide.cs` — new, 107 lines

Draws a line straight down from the pour point showing where the pour will land.

Drawn **at the width of the actual sphere cast** (`pourCastRadius * 2`), so it shows the real volume
that must overlap the target's opening rather than acting as a decorative pointer. Cyan over a valid
container, red when nothing is lined up. Builds its own child `LineRenderer` and material at runtime,
so it never conflicts with a `LineRenderer` already on the object.

| Field | Default |
|---|---|
| `validColor` | cyan `0, 0.956, 0.965`, alpha `0.75` (matches `Materials/Highlight.mat`) |
| `invalidColor` | red `1, 0.35, 0.35`, alpha `0.45` |
| `matchCastRadius` | `true` — draw at the real cast width |
| `lineWidth` | `0.01` — only used when `matchCastRadius` is off |
| `missLength` | `0.4` — how far the line draws when nothing is underneath |

### 3. `Assets/Scripts/Interactables/PourTargetHighlight.cs` — new, 96 lines

Lights up a container while another container is aimed at it.

Driven by the same test the pour uses, so it is a truthful indicator rather than a proximity guess:
if it lights, the pour lands. Instances the object's materials once in `Awake`, then toggles
`_EmissionColor` and the `_EMISSION` keyword, restoring the originals on exit and destroying the
instances in `OnDestroy`.

| Field | Default |
|---|---|
| `highlightColor` | cyan `0, 0.956, 0.965` (matches `Materials/Highlight.mat`) |
| `renderersToTint` | empty → every enabled mesh renderer on the object and its children |

---

## Current wiring

Both components are opt-in — `ChemContainer` finds them with `GetComponent`, and nothing draws
until they are attached. They are currently **scene overrides on `WebLabGlassware-Zack.unity`**, not
prefab edits, so no other scene is affected.

| Component | Attached to | Enabled | Settings |
|---|---|---|---|
| `PourAimGuide` | `soapBottle` | yes | defaults |
| `PourAimGuide` | `DI_Water_New_Two` | yes | defaults |
| `PourAimGuide` | `DI_Water_New_Two (1)` | yes | defaults |
| `PourTargetHighlight` | `volumetricFlaskWrapLabel65mL` | yes | defaults |

The `ChemContainer` detection change needs no wiring and applies to **every container in the
project** automatically.

---

## Scope and risk

**The detection change is shared.** Every container — beakers, flasks, cylinders, DI water, the
burette — now uses the more forgiving cast. This was a deliberate decision: the narrow-ray problem
was general, not specific to the soap bottle.

**Occlusion behaviour changed.** Masking to the `Chem` layer means solid geometry no longer blocks
the pour cast. `pourCastMaxDistance` (1.5 m) bounds the consequences, but containers stacked
vertically within that range could now receive a pour that previously would have been blocked by a
shelf or bench. Tighten the distance if this shows up.

**Not compile-verified by tooling.** There is no Unity editor or C# compiler on the build machine,
so the code was checked by review and brace/paren balance only. Unity did import all three scripts
and generate `.meta` files, which confirms they compile.

**Not yet play-tested to a conclusion.** The sphere cast was reported as an improvement before the
guides were attached. Behaviour with the guides visible and the radius at 0.06 has not been
reported back.

---

## Open items

1. **Play-test with the guides on.** The guide will show directly whether a failed pour is a lateral
   miss or something else.

2. **Possible remaining issue: pour point swing.** If the guide appears but sits beside the opening
   as the bottle rotates to 90°, that is issue 3 above and no cast radius fixes it. The fix would be
   repositioning the `Pour Point` child closer to the bottle's grab axis, or lowering `pourAngle` so
   less rotation is needed. Not yet investigated.

3. **Highlight coverage is incomplete.** Only the 65 mL flask has `PourTargetHighlight`. Pouring DI
   water at a beaker or graduated cylinder shows a guide but no target highlight. Adding it broadly
   is the point at which editing the prefabs rather than the scene starts to pay off.

4. **Per-container tuning.** `pourCastRadius` is per-`ChemContainer`. The DI bottles pour into
   narrower targets than the soap bottle does and may want their own values.

5. **Promote to prefabs?** The components are scene-local to `WebLabGlassware-Zack`. If the approach
   holds up, moving them onto the prefabs would carry them into the Aayushi, Woodhouse, Alpha, and VR
   glassware scenes.

6. **Cosmetic.** A redundant `{ }` block remains at `ChemContainer.cs:254`, left over from the removed
   `if (hit.collider != null)` guard. Kept to avoid re-indenting ~50 lines in the same commit.

---

## Files changed in `1ea70cd9`

```
Assets/Scripts/Interactables/ChemContainer.cs           +169  modified
Assets/Scripts/Interactables/PourAimGuide.cs            +107  new
Assets/Scripts/Interactables/PourTargetHighlight.cs      +96  new
Assets/Scenes/WebGL/WebLabGlassware-Zack.unity          +138  component wiring
Assets/Materials/ChemSolids.mat                           +2  incidental
Assets/Materials/SoapFluid.mat                            +2  incidental
```

See also [TaskSystem.md](TaskSystem.md) and [Scenes.md](Scenes.md).
