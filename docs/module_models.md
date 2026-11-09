# Module 3 — Model Inventory (Existing vs. Missing)

Companion to [module_environment.md](module_environment.md) (labware list and station map). This document checks each labware item against what is in the Unity project, so each gap can be assigned and closed.

**Method:** searched `Assets/` for `.fbx` / `.blend` / `.obj` sources and `.prefab` files, and read the base mesh reference of the reagent bottle prefab. I did not open the models in Unity or Blender, so "usable" below means "exists and plausibly fits", not visually verified. Items marked **VERIFY** need someone to look at them in the Editor.

**Targets:** Quest 3 (XR) and WebGL. Budgets in section 4 apply to both; WebGL is the tighter one.

---

## 1. Summary

| Status | Count | Meaning |
|---|---|---|
| ✅ Exists, ready | 10 (E1–E9, E16) | Model + prefab present, used in another module, no change needed beyond placement |
| 🔧 Exists, needs work | 6 (E10–E15) | A model or placeholder exists but needs relabeling, a variant, or an extension |
| 🆕 Missing | 8 (M1–M8) | No model or prefab in the project; must be made or sourced |

Correction to `module_environment.md`: its master list showed the reagent bottles (items 10–12) as ready variants of the `ChemBottle` prefabs. The `ChemBottle` prefab mesh is a **Unity built-in primitive** (mesh `fileID 10206`, the default Cylinder), not an authored bottle model. They are placeholders, so item E11 below says "needs a real model".

---

## 1a. Status in the Module 3 scene (updated 2026-11-09)

Scene: `Assets/Scenes/WebGL/WebLabSceneChemicalChange.unity`. Stations are the numbered anchors under `Station Anchors` (see [module_environment.md](module_environment.md#11-stations-and-location-map)); copies of existing prefabs were placed on the benches under `Station Models` by `ChemVR > Place Station Models`. Positions were then adjusted by hand.

| Station | Placed in scene | Prefab | Notes |
|---|---|---|---|
| 1 PPE | goggles, lab coat, glove box (already in the environment) | `goggles`, `labcoat`, `glovebox` | Not copied; the existing objects carry the task wiring |
| 2 MIXING | graduated cylinder, DI-water bottle (squirt-bottle stand-in), 1 dropper (bench pipette), pencil | `GraduatedCylinderMesh_Fluid`, `DI_Water_New_Two`, `titration_dropper`, `pencil` | Notebook, Sharpie, pH paper: no model yet (M5, M6, M8) |
| 3 HOOD | 3 reagent bottles (yellow K₂CrO₄, white NaOH, plain H₂SO₄), 2 trays, 3 droppers | `Yellow/White ChemBottle Variant`, `ChemBottle`, `tray`, `titration_dropper` | Fume Hood is in the environment (`Environment/Lab/Building/Fume Hood`). Waste container missing (M7). Bottles are still the primitive-cylinder placeholders (E11) |
| 4 SINK | nothing added | none | Sink, soap, drying rack and glass-waste bin were already in the scene |
| 5 TUBE | stir rod; the test tubes and rack already exist as `FlasksInHolder` | `StirRod`; `Tools (Particle)/FlasksInHolder` (nested in `WebGL/Lab/Lab.prefab`) | The tubes appear in the scene as `Flask (Particle) (#)`. They are flask models standing in for test tubes; M1/M2 stay open until real test tube and rack models exist |

Fixes made while placing these (worth knowing for any new prop):
- The dropper and stir-rod prefabs use layers `ChemIgnore` / `HoldableCollision`, which the player camera does not render. The placed copies were moved to `Holdable`, otherwise they are invisible in the Game view.
- The dropper prefab is tall and tips over when upright, so the copies lie flat. The prefab also has a missing-script reference that should be cleaned up.
- The lab drawer no longer exists; the TUBE station is a standard bench.

---

## 2. Existing models

| # | Item | Model source | Prefab(s) | Used by | Status | Notes |
|---|---|---|---|---|---|---|
| E1 | Safety goggles | `Prefabs/Models` (via prefab) | `Prefabs/goggles.prefab`, `goggles 1.prefab` | All modules (`WearGoggles`) | ✅ | Two prefabs; confirm which is current |
| E2 | Lab coat | `Prefabs/Models/Lab_Coat_base01.blend` (+ `_base.blend`) | `Prefabs/labcoat.prefab` | All modules (`WearCoat`) | ✅ | |
| E3 | Glove box (PPE glove dispenser) | `Prefabs/Models/glovebox.fbx` | `Prefabs/glovebox.prefab` | All modules (`AddGloves`) | ✅ | |
| E4 | Fume hood | `3D Models/Fume_Hood.fbx` | `Prefabs/Fume Hood.prefab` | Glassware module | ✅ | Sash logic exists. Present in the Module 3 scene; 2 trays placed at the HOOD station |
| E5 | Glass stir rod | `Prefabs/Models/glass_stir_rod.blend`, `stirRod.fbx` | `Interactables/StirRod.prefab`, `Not_Ready/glass_stir_rod.prefab`, `WebGL/Glassware/glass_stir_rod.prefab` | Glassware, Chemical Change | ✅ | Three prefabs; the WebGL one is the lighter version |
| E6 | Graduated cylinder | `Prefabs/Models/Graduated_Glassware/graduatedCylinderLabeled.fbx` | `Interactables/Graduated_Glassware/GraduatedCylinderMesh_Fluid.prefab` | Glassware | ✅ | Confirm the labeled 10 mL variant; the project may only have the larger size |
| E7 | Pencil | `Prefabs/Models/pencil.blend` | `WebGL/Glove Hygiene Items/pencil.prefab` | Glove Hygiene (`Pencil_To_Paper`) | ✅ | Used for notebook writing; no Sharpie or pen exists |
| E8 | Glass-waste bin | `Prefabs/Models/trash.blend`, `trashWrinkle.blend` | `Prefabs/glassDisposalTrash.prefab`, `WebGL/Lab/trash.prefab` | Glassware (`GlassDisposal`) | ✅ | |
| E9 | Sink, soap, drying rack | `sink_base.blend`, `soapBottle.fbx`, `drying_rack.blend` | `Interactables/sink_base.prefab`, `webGLSink.prefab`, `soapBottle.prefab`, `Drying_rack.prefab` | Glassware | ✅ | Sink cleanup route already built |
| E10 | Trays (secondary containment) | `Models/tray.fbx` | `Interactables/tray.prefab` | Glassware (`SnapToTray`) | 🔧 | Needs Acids/Bases labels and a snap layout for 3 bottles |
| E11 | Reagent bottles (K₂CrO₄, NaOH, H₂SO₄) | none (Unity cylinder primitive) | `Tools (Static)/ChemBottle.prefab`, plus Yellow/White/Red/Orange/Blue/Green variants | Chemical Change, Glove Hygiene | 🔧 | Placeholder shape only. Needs a proper reagent bottle mesh with cap and label area. Variants differ by color, not by shape |
| E12 | Disposable dropper / pipette | `Prefabs/Models/titration_dropper.blend`, `titration_dropper_UP.blend` | `Tools (Particle)/titration_dropper.prefab` | Glove Hygiene (titration) | 🔧 | A dropper shape exists. Needs the taped-to-bottle variant and a 4-drop behavior |
| E13 | Lab notebook | `Prefabs/Models/clip_board.blend` | `WebGL/Glove Hygiene Items/clip_board.prefab` | Glove Hygiene (WebGL) | 🔧 | A clipboard, not a notebook. **VERIFY** whether it works as a stand-in; the selectable-statement UI is new either way |
| E14 | DI-water bottle | `Prefabs/Models/DI_water.blend` | `DI_Water_New_Two.prefab` | Glassware | 🔧 | Closest thing to a squirt/wash bottle. **VERIFY** the shape; may need a nozzle |
| E15 | Racks | `Prefabs/Models/chem_rack.blend`, `spaced_chem_rack.blend`, `evenly_spaced_chem_rack.blend`, `evenly_spaced_chem_rack_small.blend` | none of the four has a prefab | none found | 🔧 | **VERIFY**: these may be usable as a test tube rack base (the names suggest a rack with evenly spaced slots). They are unreferenced by any prefab |
| E16 | Paper towels | `paper_towels.blend`, `single_paper_towels.blend` | `Not_Ready/paper_towels.prefab`, `single_paper_towels.prefab` | Glassware | ✅ | Optional for spill cleanup |
| E17 | Petri dish | `Prefabs/Models/petridish.fbx` | none | none | — | Not needed; listed for completeness |

Other reusable glassware (flasks, beakers, volumetric pipettes, weigh boats, jars) exists but is not needed for Module 3.

---

## 3. Missing models

| # | Item | Qty | Needed in step(s) | Spec (from the source doc) | Why not covered by an existing model | Suggested resolution | Owner | Priority |
|---|---|---|---|---|---|---|---|---|
| M1 | **Test tube** | 2 | 1–9 | Borosilicate glass, 19 mL capacity, 120 mm / 5 in long (Fisherbrand disposable, plain end, 10-pack per the doc's link) | No tube model or prefab exists | Build in Blender, then add `ChemContainer` + `ResizeFluid`. Needs a fluid mesh scaled to 1 mL (≈ 6 mm of liquid in a 120 mm tube) and a label surface | TBD | 🔴 Blocker |
| M2 | **Test tube rack** | 1 | 1–8 | Holds ≥ 2 tubes upright, 2 labeled slots | Existing `*_chem_rack` models are unverified for tubes and have no prefab | VERIFY first; if the shape fits, make a prefab with 2 socket positions; otherwise build new | TBD | 🔴 Blocker |
| M3 | **Disposable plastic pipette (bench)** | 1 | 1 | Clear plastic transfer pipette, for the 1 mL estimate | Titration dropper is a glass dropper shape | Reuse the dropper model with a plastic material | TBD | 🟠 High |
| M4 | **Pipette-in-test-tube holder (taped to each bottle)** | 3 | 1, 3, 6, 7 | A dropper held in a small glass tube, taped to the bottle (doc: "taped to it, held inside a glass test tube") | No taped/holder variant exists | Reuse M1 (tube) + dropper; attach to the bottle prefab | TBD | 🟠 High |
| M5 | **Lab notebook** | 1 | 5–7 | Open notebook, page with pre-written selectable statements | Only a clipboard exists | Build a simple book model, or accept the clipboard (decision needed) | TBD | 🟠 High |
| M6 | **Black Sharpie (marker)** | 1 | 1 | Marker used to label tubes #1/#2 | No marker model; only a pencil | Reuse the pencil model with a marker material, or build new | TBD | 🟡 Medium |
| M7 | **Chemical waste container** | 1 | 8 | Labeled jug/carboy for acid, base and metal-ion waste, in the fume hood | No waste container; the only waste prefab is for glass | Build a labeled jug; confirm the number of streams with the lab coordinator | TBD | 🟠 High |
| M8 | **pH paper / litmus paper** | 1 pack | none yet (learning objective only) | Strips, possibly with a color-chart | No model, and no step uses them yet | Defer until the lab coordinator decides whether a step is added | TBD | 🟢 Low / deferred |

### Items that exist but have placeholder art (see E11)

The three **reagent bottles** (K₂CrO₄, NaOH, H₂SO₄) are tracked under E11 because a prefab exists, but the model is a primitive cylinder. They need real bottle models and labels. The source doc also lists "insert a picture of what the reagent bottles look like" as pending art. Treat this as a **🔴 blocker** alongside M1 and M2, since the bottles are the first thing the player looks for in Step 1.

---

## 4. Asset specification for new models

| Constraint | Quest 3 (XR) | WebGL |
|---|---|---|
| Triangle budget per small prop (tube, marker, dropper) | ≤ 1,500 | ≤ 1,000 |
| Triangle budget per medium prop (rack, notebook, jug, bottle) | ≤ 4,000 | ≤ 2,500 |
| Materials per prop | 1–2 | 1 |
| Texture size | ≤ 1024² (labels 512²) | ≤ 512² |
| Format | `.fbx` with a Unity prefab | same prefab for both |
| Colliders | Convex mesh or primitive collider; grabbable props need a rigidbody | same |
| Transparency | Glass uses the project's existing glass material from `Assets/Materials/LabeledGlass/` | same, check overdraw in the browser |
| Naming | Match existing: `testTube.fbx`, `testTubeRack.fbx`, etc. | same |
| Origin / scale | Real-world metres: 1 unit = 1 m; test tube ≈ 0.12 m long, 16 mm diameter | same |
| Fluid | A matching fluid mesh scaled by `ResizeFluid`, following `GraduatedCylinderMesh_Fluid` | same |

These budgets are my suggested starting points, not values taken from the project. Adjust them if there are existing performance targets.

---

## 5. Decisions needed

| # | Question | Affects | Who decides |
|---|---|---|---|
| D1 | Are the `chem_rack` models usable as a test tube rack? Open them in Unity and check. (A stand-in `FlasksInHolder` is in the scene at the TUBE station.) | M2 | Art / Steve |
| D2 | Is the clipboard acceptable as the lab notebook, or do we need a book? | M5 | Lab coordinator / design |
| D3 | Is the DI-water bottle acceptable as the squirt bottle? | E14 | Lab coordinator |
| D4 | Which reagent goes in which tray, and how many waste streams exist? | E10, M7 | Lab coordinator |
| D5 | Will pH/litmus paper be added as a step? | M8 | Design |
| D6 | Build new models in Blender, or buy/source them? | M1–M7 | Art lead |
| D7 | Should the reagent bottles be one shared model with different labels, or three models? | E11 | Art |

---

## 6. Suggested build order

1. **M1 test tube** (blocks Steps 1–9, includes the fluid mesh)
2. **E11 reagent bottle model** (shared model with three label textures)
3. **M2 test tube rack** (after D1)
4. **M3/M4 droppers** (derive from the existing dropper once M1 is done)
5. **M7 waste container**
6. **M5 notebook, M6 Sharpie**
7. **M8 pH paper** (only if D5 is yes)
