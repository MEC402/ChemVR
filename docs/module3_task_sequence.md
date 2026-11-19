# Physical & Chemical Changes (Lab 3, Reaction #1) - Click-Through Task Sequence

The `Chemical_Change` module now runs the chromate-dichromate task sequence from [module_environment.md](module_environment.md). Every step except task 4 (put on PPE, a real step) is currently a **click-through placeholder** that advances on a click, so the whole module can be walked end to end (including in a WebGL build) with the instruction text on screen. Each step is then replaced, one at a time, by its real working piece.

Built on the existing task system ([TaskSystem.md](TaskSystem.md)): `Chemical_Change_Task.asset` (a `TaskInfoSO` of step prefabs), the `Chemical_Change_Overview` popup, and the `GameEventsManager` event bus.

## What changed

| File | Change |
|---|---|
| `Resources/Tasks/Chemical_Change/Chemical_Change_Overview.cs` | `text[]` holds one popup per task (32) + the completion message. The wording follows the module document (the .docx in this folder, written up in [chemmodule_tasks.md](chemmodule_tasks.md)), which is the source of truth for the steps. New `webGLText[]`, chosen automatically on WebGL builds (`Application.platform`). New `ClickNext()` for a UI button. Serialized fields (`ccText`, `taskPreper`) unchanged, so both Chemical Change scenes keep their wiring. |
| `Resources/Tasks/Chemical_Change/Chemical_Change_Task.asset` | `taskStepPrefabs` now lists the 32 task prefabs (was the 24 sugar / CrCl3 steps). Task id unchanged, so menus and `StartTasks` still work. |
| `Resources/Tasks/Chemical_Change/Module3_ClickThroughStep.cs` | Generic placeholder `TaskStep`. Finishes on the **A button** (VR), the **N key** (WebGL skip), a UI Next button, or **B** (dev mode). |
| `Resources/Tasks/Chemical_Change/Prefabs/M3_T01_...` to `M3_T33_...` | One placeholder prefab per task (replaces the 11 `M3_StepNN_...` prefabs). The gloves task was later merged into task 4, so task 4 uses the real `Put_On_Gear_CC` prefab and the old `M3_T05_...` prefab is unreferenced. From there on a prefab's `Tnn` is one higher than its task number (task 5 is `M3_T06_...`). |
| `Scripts/Events/MiscellaneousEvents.cs` | New `OnClickThroughNext` / `ClickThroughNext()` event. |

The old sugar / CrCl3 step scripts and prefabs are still in the folder, unreferenced, so they can be copied from. Their old popup text is in git history (before the commit that introduced this change).

**Constraint:** `text[]` and `webGLText[]` must always have exactly one more entry than `taskStepPrefabs` (the last entry is the completion message), or the popup runs off the end of the array. Nothing checks this at build time.

**Name notes:** the prefabs are named `M3_Tnn_...` because `StartTasks.RemoveUnstarted()` destroys any `TaskManager` child whose name contains `"_0"`. The task id is still `Chemical_Change_Task`, and `Chemical_Change_Overview.cc_abandonMe` matches any id containing `"Chemical"`.

## Tasks and what replaces each placeholder

| Tasks | What they are | Replace with |
|---|---|---|
| 1-3, 5, 28, 31-32 | Read-and-dismiss popups (background, safety, lab notes, waste reminder, answer, safety and handling). Shown in the large centered panel (see below). | Keep click-through, or a dismiss-popup step. |
| 4 | Put on goggles, gloves and lab coat (one task) | **Done:** `Put_On_Gear_CC` (miscEvents.onPutOnRightGlove/onPutOnLeftGlove + WearCoat/WearGoggles.IsWearing()). |
| 6 | Set up the test tubes | **Done:** `Set_Up_Test_Tubes_CC` finishes when the `TubesInHolder` object is set down within 2.5 m of the `Station2_MIXING` anchor (prefab `M3_T07_SetUpTheTestTubes`). |
| 9 | Collect equipment (rack, tubes, stir rod; three reagent bottles from the hood) | Copy Gather_More_Tools / Bring_To_Hood: interactableEvents.onPlayerGrabInteractable per item (WebGL: webGLEvents.OnObjectGrabbed). |
| 7 | Estimate 1 mL: pour DI water into a test tube | **Done:** `Fill_Tube_With_Water_CC` (prefab `M3_T08_Estimate1ML`) finishes when a container named `Tube...` (Tube 1-3) receives chemistryEvents.onPourIn and holds >= 1 water. |
| 8 | Label tubes #1 and #2 | Pipette fill event; Sharpie auto-label on contact (not built yet). |
| 10-12 | Measure 1 mL of chromate and pour into each tube | chemistryEvents.onPourIn / onPourOut with a 1 mL auto-stop (PourActivator). |
| 13, 18, 23 | Get a reagent bottle and its dropper | Grab / release events. |
| 14, 19, 24 | Add 4 drops to the tube | chemistryEvents.onPipetteDispense counted to 4 drops (auto-stop). |
| 15, 20, 25 | Stir 5 s, rod back on the counter | Copy Stir_NaOH: miscEvents.OnStirBeaker accumulated to 5 s. |
| 16, 21, 26 | Physical or chemical question | Copy Ask_Q1 (buttons). Correct answers still undecided. |
| 17, 22, 27 | Choose the observation and write it in the notebook | Notebook statement picker + Pencil_To_Paper pattern. |
| 29-30 | Empty both tubes into chemical waste; tubes to glass waste | chemistryEvents.onFluidDispose + GlassDisposal. |

The full numbered list, with each task's station and instruction, is in [chemmodule_tasks.md](chemmodule_tasks.md).

## Walking through it

- **VR:** read the popup, press **A** to advance.
- **WebGL:** press **N** (the `SkipTask` binding in `WebGL.inputactions`).
- **Read-only tasks:** a green **Next (N)** (WebGL / keyboard-and-mouse scenes) or **Next (A)** (VR) button on the large panel. Click it with the mouse, or use the key.
- **Dev:** **B**, only when dev-ops mode is on.

The popup shows `Task n of 32`, the station, and the single instruction for that task. There is no on-screen "placeholder" or "press X to continue" line any more.

### Read-only tasks: the large panel

Tasks 1-3, 5, 28 and 31-32 (the 0-based indices in `infoSteps` in `Chemical_Change_Overview.cs`; update it by hand if tasks are added, removed or reordered) show in a large 16:9 panel in the centre of the view instead of the small corner popup. The panel is built in code at runtime (no scene or prefab edits): a screen overlay on WebGL (`panelWidthFraction` of the screen width, default 0.8), a world-space panel `vrPanelDistance` in front of the camera in VR. While it is up on WebGL, the cursor is freed and look/move are frozen so the Next button can be clicked, then re-locked. The Y / Z toggle hides it like the corner popup. When a real task (task 4 on) starts, the corner popup returns at its authored size and position.

## Building for WebGL

The Chemical Change scenes are currently **disabled** in `ProjectSettings/EditorBuildSettings.asset`.

1. In Build Settings, enable `Scenes/WebGL/WebLabSceneChemicalChange.unity` (and disable the others you don't want).
2. Make sure that scene's `Chemical_Change_Overview` has `ccText` and `taskPreper` assigned (they were set in the scene before this change).
3. Build WebGL. The `Resources/Tasks` content is picked up automatically.

`WebLabSceneChemicalChange` is an older port (April 2025) and lacks `WebGLInput` and `IconHider`, which the newer glassware WebGL scenes have. `WebGLInput` is what turns the N key into the skip event, so **the N key will not advance steps until that scene has a `WebGLInput` object**. If it is missing, use the on-screen button or copy the object over from `WebLabGlassware-Aayushi`.

## Dropping in a working piece

1. Write the real step as a `TaskStep` subclass. Copy the closest existing step from this folder (see the table above and [TaskSystem.md](TaskSystem.md#adding-a-new-step)). Do not declare `Start()` or `OnDestroy()`; the base class owns them.
2. Make a prefab for it in `Chemical_Change/Prefabs/`.
3. Open `Chemical_Change_Task.asset` and replace that index's placeholder with the new prefab. Keep the task order from chemmodule_tasks.md.
4. Update that step's strings in `Chemical_Change_Overview.cs` (both `text[]` and `webGLText[]`) (the popup text has no placeholder line to remove). Easier: edit the task line in chemmodule_tasks.md and run `python3 tools/sync_task_text.py`.

Steps not yet replaced keep working as click-through, so the module is always runnable.

## Known limits

- The C# and the generated prefab/asset YAML were written without a Unity Editor and have **not been compiled or opened**. First open in Unity 6 to confirm the console is clean and no step slot on the asset shows "Missing".
- Placeholder steps verify nothing, so the module cannot yet teach or grade. Step 6's correct answers are still undecided (see [Module3-MajorSteps.md](Module3-MajorSteps.md)).
- No reset checkpoints exist for this module yet (`ResetTaskSystem` needs one per step).
- The old sugar / CrCl3 experiment is no longer reachable from the menu. Recover it from git history if it is needed again.
- `Task.StartOver()` and `Task.Kill()` find step clones by name. Each placeholder prefab has a distinct name, so this should be fine, but it is untested.
