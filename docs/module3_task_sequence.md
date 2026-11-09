# Module 3 - Click-Through Task Sequence

A runnable skeleton of the Module 3 task sequence ([module_environment.md](module_environment.md)). Every step is a **placeholder that advances on a click**, so the whole module can be walked end to end with the instruction text on screen. Each step is then replaced, one at a time, by its real working piece.

Built on the existing task system ([TaskSystem.md](TaskSystem.md)): a `TaskInfoSO` of step prefabs, an overview popup, and the `GameEventsManager` event bus.

## What was added

| File (under `Assets/Resources/Tasks/Module3/`) | Role |
|---|---|
| `Module3_Steps.cs` | Step table: title, station, labware, VR/WebGL text, and what replaces each placeholder. Single source of truth for the popup text. |
| `Module3_ClickThroughStep.cs` | Generic placeholder `TaskStep`. Finishes on the **A button** (VR), the **WebGL skip key**, a **UI Next button**, or **B** (dev mode). |
| `Module3_Overview.cs` | Popup driver, modeled on `Chemical_Change_Overview`. Starts `Module3_Task`, shows the current step, and exposes `ClickNext()` for a UI button. |
| `Module3_Task.asset` | `TaskInfoSO` with id `Module3_Task` and the 11 step prefabs in order. |
| `Prefabs/M3_Step00_...` to `M3_Step10_...` | One placeholder prefab per step. |

Outside that folder, `Scripts/Events/MiscellaneousEvents.cs` gained `OnClickThroughNext` / `ClickThroughNext()`.

**Why the id is `Module3_Task`:** `Chemical_Change_Overview.cc_abandonMe` and `SpawnAtStation` match task ids by substring (`"Chemical"`, `"che"`, `"gla"`, `"glo"`, `"tut"`). An id that avoids those words cannot be picked up by another module's listeners. **Why the prefabs are `M3_StepNN_...`:** `StartTasks.RemoveUnstarted()` destroys any `TaskManager` child whose name contains `"_0"`.

## Steps and what replaces each placeholder

| # | Prefab | Step | Station | Labware | Replace with |
|---|---|---|---|---|---|
| 0 | `M3_Step00_Background` | Background | PPE | - | Popup dismiss (button press). Keep click-through or use a dismiss-popup step. |
| 1 | `M3_Step01_PPE` | Put on PPE | PPE | goggles, lab coat, gloves | Copy Put_On_Gear_CC: miscEvents.onPutOnRightGlove/onPutOnLeftGlove + WearCoat/WearGoggles.IsWearing(). |
| 2 | `M3_Step02_Gather` | Gather equipment | Mixing bench, lab drawer, fume hood | 2 test tubes, test tube rack, stir rod, squirt bottle (DI), bench pipette, Sharpie, K2CrO4, NaOH, H2SO4 bottles, notebook + pen | Copy Gather_More_Tools / Bring_To_Hood: interactableEvents.onPlayerGrabInteractable per item (WebGL: webGLEvents.OnObjectGrabbed). |
| 3 | `M3_Step03_MeasureChromate` | Measure 1 mL potassium chromate | Mixing bench | graduated cylinder, K2CrO4 bottle, test tubes #1 and #2 | chemistryEvents.onPourIn / onPourOut with a 1 mL auto-stop (PourActivator). |
| 4 | `M3_Step04_AddNaOH_Tube1` | Add 4 drops of 1 M NaOH to test tube #1 | Mixing bench | 1 M NaOH bottle + dropper, test tube #1 | chemistryEvents.onPipetteDispense counted to 4 drops (auto-stop). |
| 5 | `M3_Step05_Stir` | Stir the solution | Mixing bench | stir rod, test tube #1 | Copy Stir_NaOH: miscEvents.OnStirBeaker accumulated to 5 s. |
| 6 | `M3_Step06_Record` | Record observations | Mixing bench | lab notebook, pen, test tube #1 | Copy Ask_Q1 (yes/no buttons) + notebook statement picker. Correct answer for each tube is still undecided. |
| 7 | `M3_Step07_AddH2SO4_Tube2` | Add 3 M sulfuric acid to test tube #2 | Mixing bench | 3 M H2SO4 bottle + dropper, test tube #2, stir rod, notebook + pen | Same pieces as steps 04-06 with the H2SO4 dropper. |
| 8 | `M3_Step08_AddNaOH_Tube2` | Add 1 M NaOH to test tube #2 | Mixing bench | 1 M NaOH bottle + dropper, test tube #2, stir rod, notebook + pen | Same pieces as steps 04-06; drop count still to be confirmed. |
| 9 | `M3_Step09_Waste` | Waste disposal | Fume hood, sink and cleanup | both test tubes, chemical waste container, glass waste bin, sink + soap | chemistryEvents.onFluidDispose + GlassDisposal; wash hands via Wash_Hands. |
| 10 | `M3_Step10_WrapUp` | Wrap-up | Any | - | Keep click-through; end of module. |

## Scene setup (one-time, in the Unity Editor)

Not done yet; it needs the Editor.

1. Fork the module scene from the Unity 6 copy of `LabSceneGlasswareUse` (VR) or `WebLabGlassware-Aayushi` (WebGL). Do not fork from `LabSceneBase`.
2. Make sure the scene has `The Managers` (with `TaskManager`) and a `GameEventsManager`. `TaskManager` loads `Module3_Task` automatically from `Resources/Tasks`.
3. Duplicate the Chemical Change popup canvas object. Replace `Chemical_Change_Overview` with `Module3_Overview` on it, assign **M3 Text** (the TMP text) and **Task Preper** (`Start_Module`), and tick **Is WebGL** for the web scene. `ToggleTextSimple` stays on the same object.
4. Optional: add a UI Button to the popup and set its OnClick to `Module3_Overview.ClickNext()`.
5. Add the scene to build settings and to the menu, if wanted.

## Walking through it

- **VR:** read the popup, press **A** to advance.
- **WebGL:** press the skip key (the `skipTask` action bound in `WebGLInput`).
- **Any platform:** the on-screen Next button, if you add one.
- **Dev:** **B**, only when dev-ops mode is on.

The popup shows `Step n of 11`, the station, the labware needed, and the instruction.

## Dropping in a working piece

1. Write the real step as a `TaskStep` subclass. Copy the closest existing step (see the table above and [TaskSystem.md](TaskSystem.md#adding-a-new-step)). Do not declare `Start()` or `OnDestroy()`; the base class owns them.
2. Make a prefab for it.
3. Open `Module3_Task.asset` and replace that index's prefab with the new one. Keep the order from the table.
4. Update the step text in `Module3_Steps.cs` if the wording changes. Once no placeholders remain, remove the placeholder footer in `BuildText`.

Steps not yet replaced keep working as click-through, so the module is always runnable.

## Known limits

- The C# and the generated prefab/asset YAML were written without a Unity Editor and have **not been compiled or opened**. First open in Unity 6 to confirm the console is clean and no step slot on the asset shows "Missing".
- Placeholder steps verify nothing, so the module cannot yet teach or grade. Step 6's correct answers are still undecided (see [Module3-MajorSteps.md](Module3-MajorSteps.md)).
- No reset checkpoints exist for this module yet (`ResetTaskSystem` needs one per step).
- `Task.StartOver()` and `Task.Kill()` find step clones by name. Each placeholder prefab has a distinct name, so this should be fine, but it is untested.
