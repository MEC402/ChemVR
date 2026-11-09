# Physical & Chemical Changes (Lab 3, Reaction #1) - Click-Through Task Sequence

The `Chemical_Change` module now runs the chromate-dichromate task sequence from [module_environment.md](module_environment.md). Every step is currently a **click-through placeholder** that advances on a click, so the whole module can be walked end to end (including in a WebGL build) with the instruction text on screen. Each step is then replaced, one at a time, by its real working piece.

Built on the existing task system ([TaskSystem.md](TaskSystem.md)): `Chemical_Change_Task.asset` (a `TaskInfoSO` of step prefabs), the `Chemical_Change_Overview` popup, and the `GameEventsManager` event bus.

## What changed

| File | Change |
|---|---|
| `Resources/Tasks/Chemical_Change/Chemical_Change_Overview.cs` | `text[]` replaced with the 11 new steps + completion message. New `webGLText[]`, chosen automatically on WebGL builds (`Application.platform`). New `ClickNext()` for a UI button. Serialized fields (`ccText`, `taskPreper`) unchanged, so both Chemical Change scenes keep their wiring. |
| `Resources/Tasks/Chemical_Change/Chemical_Change_Task.asset` | `taskStepPrefabs` now lists the 11 placeholder prefabs (was the 24 sugar / CrCl3 steps). Task id unchanged, so menus and `StartTasks` still work. |
| `Resources/Tasks/Chemical_Change/Module3_ClickThroughStep.cs` | Generic placeholder `TaskStep`. Finishes on the **A button** (VR), the **N key** (WebGL skip), a UI Next button, or **B** (dev mode). |
| `Resources/Tasks/Chemical_Change/Prefabs/M3_Step00_...` to `M3_Step10_...` | One placeholder prefab per step. |
| `Scripts/Events/MiscellaneousEvents.cs` | New `OnClickThroughNext` / `ClickThroughNext()` event. |

The old sugar / CrCl3 step scripts and prefabs are still in the folder, unreferenced, so they can be copied from. Their old popup text is in git history (before the commit that introduced this change).

**Constraint:** `text[]` and `webGLText[]` must always have exactly one more entry than `taskStepPrefabs` (the last entry is the completion message), or the popup runs off the end of the array. Nothing checks this at build time.

**Name notes:** the prefabs are named `M3_StepNN_...` because `StartTasks.RemoveUnstarted()` destroys any `TaskManager` child whose name contains `"_0"`. The task id is still `Chemical_Change_Task`, and `Chemical_Change_Overview.cc_abandonMe` matches any id containing `"Chemical"`.

## Steps and what replaces each placeholder

| # | Prefab | Step | Station | Labware | Replace with |
|---|---|---|---|---|---|
| 0 | `M3_Step00_Background` | Background | PPE | - | Popup dismiss (button press). Keep click-through or use a dismiss-popup step. |
| 1 | `M3_Step01_PPE` | Put on PPE | PPE | goggles, lab coat, gloves | Copy Put_On_Gear_CC: miscEvents.onPutOnRightGlove/onPutOnLeftGlove + WearCoat/WearGoggles.IsWearing(). |
| 2 | `M3_Step02_Gather` | Gather equipment | TUBE station (5), Mixing bench (2), fume hood (3) | 2 test tubes, test tube rack, stir rod, squirt bottle (DI), bench pipette, Sharpie, K2CrO4, NaOH, H2SO4 bottles, notebook + pen | Copy Gather_More_Tools / Bring_To_Hood: interactableEvents.onPlayerGrabInteractable per item (WebGL: webGLEvents.OnObjectGrabbed). |
| 3 | `M3_Step03_MeasureChromate` | Measure 1 mL potassium chromate | Mixing bench | graduated cylinder, K2CrO4 bottle, test tubes #1 and #2 | chemistryEvents.onPourIn / onPourOut with a 1 mL auto-stop (PourActivator). |
| 4 | `M3_Step04_AddNaOH_Tube1` | Add 4 drops of 1 M NaOH to test tube #1 | Mixing bench | 1 M NaOH bottle + dropper, test tube #1 | chemistryEvents.onPipetteDispense counted to 4 drops (auto-stop). |
| 5 | `M3_Step05_Stir` | Stir the solution | Mixing bench | stir rod, test tube #1 | Copy Stir_NaOH: miscEvents.OnStirBeaker accumulated to 5 s. |
| 6 | `M3_Step06_Record` | Record observations | Mixing bench | lab notebook, pen, test tube #1 | Copy Ask_Q1 (yes/no buttons) + notebook statement picker. Correct answer for each tube is still undecided. |
| 7 | `M3_Step07_AddH2SO4_Tube2` | Add 3 M sulfuric acid to test tube #2 | Mixing bench | 3 M H2SO4 bottle + dropper, test tube #2, stir rod, notebook + pen | Same pieces as steps 04-06 with the H2SO4 dropper. |
| 8 | `M3_Step08_AddNaOH_Tube2` | Add 1 M NaOH to test tube #2 | Mixing bench | 1 M NaOH bottle + dropper, test tube #2, stir rod, notebook + pen | Same pieces as steps 04-06; drop count still to be confirmed. |
| 9 | `M3_Step09_Waste` | Waste disposal | Fume hood, sink and cleanup | both test tubes, chemical waste container, glass waste bin, sink + soap | chemistryEvents.onFluidDispose + GlassDisposal; wash hands via Wash_Hands. |
| 10 | `M3_Step10_WrapUp` | Wrap-up | Any | - | Keep click-through; end of module. |

## Walking through it

- **VR:** read the popup, press **A** to advance.
- **WebGL:** press **N** (the `SkipTask` binding in `WebGL.inputactions`).
- **Any platform:** an on-screen Next button wired to `Chemical_Change_Overview.ClickNext()`, if you add one.
- **Dev:** **B**, only when dev-ops mode is on.

The popup shows `Step n of 11`, the station, the labware needed, and the instruction.

## Building for WebGL

The Chemical Change scenes are currently **disabled** in `ProjectSettings/EditorBuildSettings.asset`.

1. In Build Settings, enable `Scenes/WebGL/WebLabSceneChemicalChange.unity` (and disable the others you don't want).
2. Make sure that scene's `Chemical_Change_Overview` has `ccText` and `taskPreper` assigned (they were set in the scene before this change).
3. Build WebGL. The `Resources/Tasks` content is picked up automatically.

`WebLabSceneChemicalChange` is an older port (April 2025) and lacks `WebGLInput` and `IconHider`, which the newer glassware WebGL scenes have. `WebGLInput` is what turns the N key into the skip event, so **the N key will not advance steps until that scene has a `WebGLInput` object**. If it is missing, use the on-screen button or copy the object over from `WebLabGlassware-Aayushi`.

## Dropping in a working piece

1. Write the real step as a `TaskStep` subclass. Copy the closest existing step from this folder (see the table above and [TaskSystem.md](TaskSystem.md#adding-a-new-step)). Do not declare `Start()` or `OnDestroy()`; the base class owns them.
2. Make a prefab for it in `Chemical_Change/Prefabs/`.
3. Open `Chemical_Change_Task.asset` and replace that index's placeholder with the new prefab. Keep the order from the table.
4. Update that step's strings in `Chemical_Change_Overview.cs` (both `text[]` and `webGLText[]`) and remove the "[placeholder step ...]" line.

Steps not yet replaced keep working as click-through, so the module is always runnable.

## Known limits

- The C# and the generated prefab/asset YAML were written without a Unity Editor and have **not been compiled or opened**. First open in Unity 6 to confirm the console is clean and no step slot on the asset shows "Missing".
- Placeholder steps verify nothing, so the module cannot yet teach or grade. Step 6's correct answers are still undecided (see [Module3-MajorSteps.md](Module3-MajorSteps.md)).
- No reset checkpoints exist for this module yet (`ResetTaskSystem` needs one per step).
- The old sugar / CrCl3 experiment is no longer reachable from the menu. Recover it from git history if it is needed again.
- `Task.StartOver()` and `Task.Kill()` find step clones by name. Each placeholder prefab has a distinct name, so this should be fine, but it is untested.
