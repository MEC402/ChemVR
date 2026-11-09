# Module 3 — Environment & Work Breakdown

**Module:** Physical and Chemical Changes (Reaction #1, chromate–dichromate equilibrium)
**Targets:** Unity WebGL (first-person, mouse/keyboard) and Meta Quest 3 (XR)
**Companion doc:** [Module3-MajorSteps.md](Module3-MajorSteps.md) (step summary, open issues in the source docx)
**Base scene to fork:** `LabSceneGlasswareUse` (VR) / `WebLabGlassware-Aayushi` (WebGL). Task system conventions: [TaskSystem.md](TaskSystem.md).

The module has three segments:

| Segment | Steps | Station(s) |
|---|---|---|
| **A. Preparation** | 0 Background, 0b PPE | PPE station |
| **B. Reaction #1** | 1–7 (gather, measure, add reagents, stir, observe, repeat for tube #2) | Fume hood (reagents), mixing station (bench) |
| **C. Cleanup & Wrap-up** | Waste disposal, explanation | Sink & cleanup station, fume hood waste |

**Status legend:** ✅ prefab exists in the project · 🔧 exists but needs a variant or modification · 🆕 must be built.

---

# PART 1 — Environment and Labware

## 1.1 Stations and location map

The glassware module already has four stations, each with a teleport anchor (`TeleAnchor_*`) under one shared parent transform. Module 3 reuses all four; no new rooms or buildings are needed.

| Station | Anchor | Local pos (x, y, z) | What lives here (Module 3) |
|---|---|---|---|
| **PPE** | `TeleAnchor_PPE` | (0.02, 0, 3.59) | `goggles`, `labcoat`, `glovebox` on the PPE table (≈ z 6.3–7.1 in the scene) |
| **Mixing station** (bench) | `TeleAnchor_Glassware` | (-1.51, 0, 3.53) | Test tube rack, test tubes, stir rod, graduated cylinder, squirt bottle, lab notebook + pen, Sharpie, pH/litmus paper. The existing `Glassware_Table` / bench area is the working surface. |
| **Fume hood** | `TeleAnchor_Hood` | (1.91, 0, -2.85) | Two secondary-containment trays (Acids, Bases) holding the reagent bottles, each with a taped pipette; metal-ion/chemical waste container. `Fume Hood.prefab` |
| **Sink & cleanup** | `TeleAnchor_Sink` | (-7.68, 0, -7.94) | Sink, soap, drying rack, glass-waste bin (`glassDisposalTrash`), trash |
| *Lab drawer* (new) | none; part of the mixing bench | — | Drawer under the bench holding the test tubes, rack and stir rod (Step 1 says "inside your lab drawer"). Reuse the cabinet/`DoorOpen` pattern. |

Coordinates are copied from the `LabSceneGlasswareUse` scene file and are local to the shared anchor parent, so use them for relative layout, not as world positions. Verify in the Editor before placing objects. `TeleAnchor_Scale` and `TeleAnchor_Jar` (balance and solids jar) are not used by this module and can be hidden or removed.

### Floor plan (schematic, not to scale)

```
                       ┌──────────────────────────────┐
                       │          LAB ROOM            │
                       │                              │
   SINK & CLEANUP      │                  FUME HOOD   │
   ┌───────────────┐   │               ┌────────────┐ │
   │ sink · soap   │   │               │ Acid tray  │ │
   │ drying rack   │   │               │ Base tray  │ │
   │ glass waste   │   │               │ waste jug  │ │
   └───────────────┘   │               └────────────┘ │
   (-7.7, -7.9)        │               (1.9, -2.8)    │
                       │                              │
                       │      MIXING STATION (bench)  │
                       │      rack · tubes · stir rod │
                       │      cylinder · notebook     │
                       │          (-1.5, 3.5)         │
                       │                              │
                       │      PPE STATION             │
                       │      goggles · coat · gloves │
                       │          (0.0, 3.6)          │
                       └──────────────────────────────┘
```

### Glassware and item placement

| Item | Starts at | Moves to |
|---|---|---|
| Goggles, lab coat, gloves | PPE station | Player (auto-equip) |
| Test tubes ×2, test tube rack, stir rod | Lab drawer (mixing bench) | Mixing bench, tubes in rack |
| Squirt bottle (DI water), disposable pipette, Sharpie, pen, lab notebook | Mixing bench (already out) | Stay on bench |
| 0.1 M K₂CrO₄, 1 M NaOH, 3 M H₂SO₄ bottles (each with a taped pipette) | Fume hood trays (K₂CrO₄ and NaOH in the Bases tray, H₂SO₄ in the Acids tray). **Confirm tray assignment with the lab coordinator.** | Carried to the mixing bench, returned to hood |
| Graduated cylinder (10 mL) | Mixing bench | Stays on bench |
| Used test tubes | Rack | Glass-waste bin at the sink |
| Chemical waste (acid/base/chromate) | Test tubes | Waste container in the fume hood |

## 1.2 Master labware list

| # | Item | Qty | Status | Existing asset / notes |
|---|---|---|---|---|
| 1 | Safety goggles | 1 | ✅ | `Prefabs/goggles.prefab`; `WearGoggles` script |
| 2 | Lab coat | 1 | ✅ | `Prefabs/labcoat.prefab`; `WearCoat` script |
| 3 | Gloves (hands turn blue) | 1 pair | ✅ | `Prefabs/glovebox.prefab`; `AddGloves` script |
| 4 | Test tube (19 mL, 120 mm) | 2 | 🆕 | No test tube prefab exists. Model + `ChemContainer` + `ResizeFluid`; needs a writable label surface for Sharpie labels |
| 5 | Test tube rack | 1 | 🆕 | Needs snap sockets for 2 tubes (follow the `SnapToTray` pattern) |
| 6 | Glass stir rod | 1 | ✅ | `Interactables/StirRod.prefab`, `Not_Ready/glass_stir_rod.prefab` |
| 7 | Graduated cylinder, 10 mL | 1 | ✅ | `Graduated_Glassware/GraduatedCylinderMesh_Fluid.prefab` |
| 8 | Squirt bottle, DI water | 1 | 🔧 | `DI_Water_New_Two.prefab` is a bottle, not a squirt bottle; confirm whether it is acceptable |
| 9 | Disposable plastic pipette (bench, for the 1 mL estimate) | 1 | 🔧 | Based on the `titration_dropper` model; add 1 mL fill behavior |
| 10 | Reagent bottle — 0.1 M potassium chromate (yellow) | 1 | 🔧 | `Tools (Static)/Yellow ChemBottle Variant`; `ChemContainer` solution |
| 11 | Reagent bottle — 1 M sodium hydroxide (colorless) | 1 | 🔧 | `Tools (Static)/White ChemBottle Variant` or `ChemBottle`; relabel |
| 12 | Reagent bottle — 3 M sulfuric acid (colorless) | 1 | 🔧 | `Tools (Static)/ChemBottle`; relabel |
| 13 | Taped dropper pipettes ×3 (one per reagent, held in a glass tube) | 3 | 🆕 | `titration_dropper.prefab` base; needs the auto-drop (4 drops) behavior |
| 14 | Secondary-containment trays (Acids, Bases) | 2 | 🔧 | `Interactables/tray.prefab`; new labels |
| 15 | Fume hood | 1 | ✅ | `Fume Hood.prefab` (`DoorOpen`/sash logic already exists) |
| 16 | Lab notebook (with selectable observation statements) | 1 | 🆕 | A `Pencil_To_Paper` script exists for the glove module; the notebook UI is new |
| 17 | Pen / pencil | 1 | ✅ | Existing pencil from the glove hygiene module |
| 18 | Black Sharpie | 1 | 🆕 | Model + label-write interaction (can reuse the pen mechanic) |
| 19 | pH paper / litmus paper | 1 pack | 🆕 | Not yet used by any step (see 2.9, open item) |
| 20 | Chemical (metal-ion/acid/base) waste container | 1 | 🆕 | Sits in the fume hood; accepts tube contents |
| 21 | Glass-waste bin | 1 | ✅ | `glassDisposalTrash.prefab`, `GlassDisposal.cs` |
| 22 | Sink, soap, drying rack | 1 set | ✅ | `sink_base`, `soapBottle`, `Drying_rack` |
| 23 | Lab drawer (bench) | 1 | 🔧 | Cabinet pattern (`DoorOpen`) |
| 24 | Educational-note popups, hint and instruction UI | — | ✅ | `Chemical_Change_Overview` / `Start_Module`; add `webGLText[]` since the existing module has none |

**Totals:** 24 labware/UI groups; 7 ready to use as-is (✅), 9 needing variants or small changes (🔧), 8 to be built from scratch (🆕, items 4, 5, 13, 16, 18, 19, 20 plus the drawer interaction).

---

# PART 2 — Steps and Required Labware

Each entry lists the **station**, **labware needed for that step**, then the **action tasks**. Task IDs use the `Chemical_Change`-style prefab convention (`Resources/Tasks/<Module>/Prefabs/`) described in [TaskSystem.md](TaskSystem.md#adding-a-new-step). Suggested module folder: `Resources/Tasks/Physical_Chemical_Changes/`. Each step has a platform note for VR vs. WebGL where behavior differs.

## SEGMENT A — Preparation

### Step 0: Background
- **Station:** PPE (start position, `TeleAnchor_PPE`)
- **Labware:** none (UI only)
- **Action tasks:**
  1. Show educational popup 1: nine equilibria are perturbed in the full lab, this module does #1; physical vs. chemical change.
  2. Show educational popup 2: Le Châtelier's Principle.
  3. Player dismisses each popup (button/click) → step advances.

### Step 0b: Put On PPE
- **Station:** PPE
- **Labware:** goggles, lab coat, gloves
- **Action tasks:**
  1. Show safety popup (acids/bases ≥ 1 M, clean up spills, no drops left on bottle sides).
  2. Player presses the goggles button → goggles auto-equip with animation.
  3. Player presses the lab coat button → coat auto-equips with animation.
  4. Player presses the gloves button → hands turn blue.
  5. Checkmarks appear on each equipped item.
  6. Show remaining popups: goggles/coat purpose, metal-ion and glass waste, and "this is a qualitative experiment".
  7. Gate: goggles + coat + gloves all equipped → advance. (Hint after 15 s.)
- **Reuses:** `Put_On_Gear_CC` step pattern, `WearGoggles`, `WearCoat`, `AddGloves`.

## SEGMENT B — Reaction #1: The chromate–dichromate equilibrium

### Step 1: Gather Equipment
- **Station:** Mixing bench, lab drawer, fume hood
- **Labware:** test tubes ×2, test tube rack, stir rod, squirt bottle (DI), disposable pipette, Sharpie, 3 reagent bottles with droppers, notebook + pen
- **Action tasks:**
  1. Show the "approximately 1 mL" qualitative-lab popup.
  2. Player opens the bench drawer and takes out two test tubes, the rack and the stir rod.
  3. Player places the two tubes in the rack.
  4. Player uses the bench pipette to draw ~1 mL from the squirt bottle into a tube as a size reference (display the 1 mL reference image), then empties it.
  5. Player labels tube #1 and tube #2 with the Sharpie (auto-label on contact).
  6. Player walks to the fume hood and locates K₂CrO₄, NaOH and H₂SO₄ in the Acids/Bases trays.
  7. Player brings all three bottles to the bench.
  8. Gate: all items in the work zone, tubes labeled. (Hint after 15 s.)
- **Needs:** items 4, 5, 9, 13, 14, 18, 23 from the master list; reagent bottle art (still pending in the source doc).

### Step 2: Measure 1 mL of 0.1 M potassium chromate
- **Station:** Mixing bench
- **Labware:** graduated cylinder, K₂CrO₄ bottle, test tubes #1 and #2 in the rack
- **Action tasks:**
  1. Player picks up the graduated cylinder and holds it under the K₂CrO₄ bottle.
  2. Player pours; flow auto-stops at exactly 1 mL (chime + visual cue; overfilling impossible).
  3. Player pours the cylinder contents into tube #1.
  4. Repeat for tube #2.
  5. Gate: both tubes contain 1 mL of yellow solution. (Hint after 5 s.)
- **Reuses:** `PourActivator`, `ChemContainer`, `ResizeFluid`.

### Step 3: Drop-wise addition of 1 M NaOH to test tube #1
- **Station:** Mixing bench
- **Labware:** 1 M NaOH bottle + dropper, test tube #1, stir rod
- **Action tasks:**
  1. Player picks up the NaOH bottle and its dropper, then puts the bottle down.
  2. Player positions the dropper over tube #1; a prompt appears ("push button").
  3. On button press, drops dispense automatically; auto-stop after exactly 4 drops (chime).
  4. Expected result: solution stays yellow (chromate favored); tube #1 volume increases slightly.
  5. Player places the stir rod in tube #1, stirs (see Step 4), and puts the rod on the counter.
  6. Gate: 4 drops added and stirred. (Hint after 25 s.)
- **Reuses:** `titration_dropper` model, `onPipetteDispense` event.

### Step 4: Stir the solution
- **Station:** Mixing bench
- **Labware:** stir rod, the tube just dosed
- **Action tasks:**
  1. Player picks up the stir rod and places it in the tube (prompt: "push button").
  2. VR: detect a circular hand motion, or hold the trigger for auto-stir. WebGL: hold the button for auto-stir. (Choose the easier option per the design doc: auto-stir.)
  3. Stir for at least 5 s; the fluid visibly swirls and mixes.
  4. Gate: ≥ 5 s of stirring + success chime. (Hint after 25 s.)
- **Reuses:** `Stir_NaOH` / `Stir_Sugar` steps (`OnStirBeaker` event), adapted from beaker to test tube.

### Step 5: Record your observations
- **Station:** Mixing bench
- **Labware:** lab notebook, pen, the dosed tube
- **Action tasks:**
  1. Show the physical-vs-chemical-change question with two large buttons; wrong answer shows the hint and allows retry.
  2. Player picks up the pen and moves to the notebook; the tube remains visible beside the page.
  3. The notebook shows pre-written observation statements (e.g., "solution stayed yellow", "solution turned orange", "no precipitate"); the player selects the one matching the tube.
  4. Wrong selection → retry; correct selection → entry written to the notebook.
  5. Gate: correct classification + correct notebook entry.
- **Open items:** the correct answer/wording for each tube is not yet defined in the source docx (see Module3-MajorSteps.md, Open Items).

### Step 6: Add 3 M sulfuric acid to test tube #2
- **Station:** Mixing bench
- **Labware:** 3 M H₂SO₄ bottle + dropper, test tube #2, stir rod, notebook + pen
- **Action tasks:** repeat Steps 3–5 for tube #2 with sulfuric acid (4 drops, stir ≥ 5 s, record). Expected observation: yellow → orange (dichromate). Hint timings as in Step 3.
- **Safety:** reinforce the corrosive-acid popup when the bottle is picked up.

### Step 7: Add 1 M sodium hydroxide to test tube #2
- **Station:** Mixing bench
- **Labware:** 1 M NaOH bottle + dropper, test tube #2 (now orange), stir rod, notebook + pen
- **Action tasks:** repeat Steps 3–5 for tube #2 with NaOH (4 drops, stir ≥ 5 s, record). Expected observation: orange → yellow (equilibrium shifts back). Show the Le Châtelier explanation after the correct entry.
- **Open item:** confirm drop count and whether more NaOH is needed to reverse the color change fully.

## SEGMENT C — Cleanup & Wrap-up

### Step 8: Waste Disposal
- **Station:** Fume hood, then sink & cleanup
- **Labware:** both test tubes, chemical-waste container, glass-waste bin, reagent bottles, sink, soap
- **Action tasks:**
  1. Show the waste reminder popup (metal-ion waste never down the drain; acid/base waste only in designated containers).
  2. Player takes tubes #1 and #2 to the fume hood and empties each into the chemical-waste container.
  3. Player returns the three reagent bottles to their hood trays.
  4. Player takes the empty tubes to the glass-waste bin.
  5. Player discards the used pipette/dropper if it was not already returned.
  6. Player washes hands at the sink (reuse `Wash_Hands`), removes gloves/coat as needed.
  7. Gate: both tubes emptied into the waste container and disposed of in glass waste.
- **Reuses:** `GlassDisposal`, `ToggleSink`, `WashHandsTrigger`, `Bring_To_Hood`, `Put_Away` steps, `HygieneManager` (contamination tracking if cross-contamination is wanted).

### Step 9: Wrap-up
- **Station:** any (player stays where Step 8 ends)
- **Labware:** none (UI)
- **Action tasks:**
  1. Show the closing explanation: NaOH removes H⁺, so the equilibrium shifts toward yellow CrO₄²⁻, and acid shifts it toward orange Cr₂O₇²⁻; no precipitate.
  2. Show a safety recap (hexavalent chromium toxicity, corrosive bases).
  3. Show the completion screen; return the player to the Start Menu.

---

# PART 3 — Implementation Work Breakdown

## 3.1 Scene and environment

- [ ] Fork `LabSceneGlasswareUse` (VR) and `WebLabGlassware-Aayushi` (WebGL) into `LabSceneChemicalChange3` / `WebLabSceneChemicalChange3`. Do **not** fork from `LabSceneBase`; it has drifted (see Scenes.md).
- [ ] Keep the four stations and anchors; hide or remove the Scale and Jar anchors and the glassware inspection table.
- [ ] Add the module to build settings and `StartMenuController` / `HelpMenuController` / `SpawnAtStation` (substring match, avoid colliding with the existing `"che"` Chemical Change module).
- [ ] Add a `Managers` object with `TaskManager` and the new module's overview script.

## 3.2 Assets (🆕 items)

- [ ] Test tube + `ChemContainer`, writable label
- [ ] Test tube rack with 2 snap sockets
- [ ] Disposable dropper with 4-drop auto-dispense
- [ ] Notebook with selectable observation statements
- [ ] Sharpie (and the auto-label interaction)
- [ ] pH/litmus paper (if a step is added)
- [ ] Chemical-waste container in the fume hood
- [ ] Reagent bottle art (potassium chromate, sodium hydroxide, sulfuric acid) and 1 mL reference image

## 3.3 Scripts and task steps

- [ ] 11 new `TaskStep` subclasses + prefabs (one each for Steps 0, 0b, 1, 2, 3, 4, 5, 6, 7, 8, 9); Steps 6 and 7 can subclass Step 3/4/5 with parameters
- [x] `Chemical_Change_Overview` now carries `text[]` and `webGLText[]` arrays of size N+1 (see [module3_task_sequence.md](module3_task_sequence.md))
- [ ] `TaskInfoSO` asset with the ordered step prefabs
- [ ] Reaction logic: chromate/dichromate color state machine driven by H⁺/OH⁻ drops (yellow ⇄ orange); used by Steps 3, 6, 7
- [ ] Auto-stop pouring (1 mL) and 4-drop dispensing
- [ ] Observation/classification UI
- [ ] Checkpoint data for `ResetTaskSystem`, one per step
- [ ] WebGL variants: `OnObjectGrabbed` / `OnInteractPressed` handling, mouse-driven stir and dropper

## 3.4 Platform notes

| Topic | Quest 3 (VR) | WebGL |
|---|---|---|
| Interaction | XR grab, trigger to pour/drop/stir | E-key interact, held button for pour/stir |
| Player | `Complete XR Origin Set Up Variant` | `FP Player` |
| Text | `text[]` | `webGLText[]` (the Chemical Change module has none today) |
| Performance | Test tubes and fluid are small objects; reuse existing fluid shader | Trim extra glassware (the base scene has ~100 `Flask` instances that this module does not need) |
| Skip / dev tools | B button (dev mode) | `onWebGLSkipTask` |

## 3.5 Open questions for the lab coordinator

1. Which tray holds each reagent (K₂CrO₄ with the bases?), and does the waste container hold one stream or separate acid/base/metal-ion streams?
2. Should the step list include the pH/litmus paper test from the learning objectives, and in which step?
3. Is a DI-water bottle acceptable in place of a squirt bottle?
4. Exact drop counts for Steps 6 and 7 (the source doc says "repeat all steps above").
5. Expected correct answer and observation wording for each tube's Step 5 question.
