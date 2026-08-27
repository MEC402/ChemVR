# ChemVR — Unity 2022.3 → Unity 6.3 LTS migration

**Branch:** `unity6-migration` (off `zack-GlasswareTesting`)
**Target editor:** `6000.3.9f1` (Unity 6.3 LTS, supported through Dec 2027)
**Status:** offline prep complete and verified. The editor upgrade itself is still to do.

`main` and `zack-GlasswareTesting` are untouched and still open cleanly in
2022.3.62f3. Nothing here is visible to the rest of the team until this branch
is merged.

---

## Why this migration is low-risk

The codebase turned out to be unusually clean for a Unity 6 jump:

- **No custom `ScriptableRendererFeature` or `ScriptableRenderPass`** — this is
  normally the single biggest URP 14 → 17 breakage, because Unity 6 makes Render
  Graph the default. We have none, so there is nothing to port.
- **No `WWW`, `UnityWebRequest`, or `Graphics.Blit`** anywhere.
- **One** deprecated API across 229 scripts (`FindObjectsOfType`), which is a
  warning in Unity 6, not an error.
- Only 3 Shader Graphs and 4 handwritten shaders. Shader Graph self-upgrades;
  `VRTemplateAssets/Shaders/FauxBlurURP.shader` is the one to eyeball, since it
  `#include`s URP paths directly.

---

## What has already been done (committed on this branch)

| Commit | Change |
|---|---|
| `4d614ad1` | Removed `com.unity.xr.arcore` + `com.unity.xr.arfoundation` and their orphaned loader/settings assets |
| `5336a518` | Bumped XRI → 2.6.5, Cinemachine → 2.10.7, XR Management → 4.7.0, Oculus → 4.5.5, Linux toolchain → 2.0.11 |
| `75860f04` | Regenerated `packages-lock.json` |
| `fb73a80b` | Removed `com.unity.learn.iet-framework` and the stock VR template tutorial |
| `1adc9c0d` | Removed `com.unity.xr.mock-hmd` 1.4.0-preview.2 and its orphaned loader/settings assets |

**AR Foundation was entirely unused** — no C# references, no scene or prefab
references, and its ARCore loader was never assigned to any build target's
loader list (only the Oculus loader is). AR Foundation 5.2 has no Unity 6
compatible version, so deleting it removed the migration's largest blocker for
free.

Every step above was verified with a headless `-batchmode` run against
2022.3.62f3: exit code 0, zero compile errors, zero package resolution errors.

---

## Step 1 — Install the editor (needs you; requires the Hub UI + your licence)

`6000.3.9f1` currently exists in the Hub as an **empty 0 GB folder** — it is
registered but not actually installed. Install it with these modules, to match
what 2022.3.62f3 has today:

- **Android Build Support** (+ OpenJDK, + Android SDK & NDK Tools) — for Quest
- **WebGL Build Support** — for the WebGL lab scenes
- **Windows Build Support (IL2CPP)**
- *(optional)* Linux Build Support — only if anyone still uses the
  `toolchain.win-x86_64-linux-x86_64` package

Roughly 25 GB. `6000.4.3f1` **is** fully installed with the right modules, but
6.4 is a "Supported Update", not LTS, and is already past end-of-life — not
somewhere to land a multi-year project.

## Step 2 — Back up, then open

```bash
git status   # must be clean; you are on unity6-migration
```

In the Hub, open the project **explicitly with 6000.3.9f1** (Hub → project row →
editor-version dropdown). Accept the upgrade prompt.

Expect a long one-time reimport — this project has ~30 scenes plus large model
and texture packs. Leave it alone until it settles.

> Do **not** open the project in `6000.4.3f1` first "just to peek". Unity
> re-serialises assets on upgrade, and opening a 6.4-serialised project back in
> 6.3 is a downgrade Unity does not support.

## Step 3 — Let Unity move the packages it owns

On first open Unity rewrites these itself; no manual manifest editing:

- `com.unity.render-pipelines.universal` 14.0.12 → 17.x (editor-bundled)
- `com.unity.textmeshpro` 3.0.7 → **removed**, replaced by bundled
  `com.unity.ugui` 2.0.0. The `TMPro` namespace is unchanged, so the 14 scripts
  using it keep compiling.
- `com.unity.timeline`, `com.unity.visualscripting`, `com.unity.collab-proxy`

## Step 4 — Bump the packages that needed Unity 6 first

Only after the project is open and compiling in 6.3:

| Package | From | To | Note |
|---|---|---|---|
| `com.unity.xr.hands` | 1.4.0 | 1.9.0 | requires `unity=6000.0` |

XR Hands ships its HandVisualizer sample **into `Assets/`**, and ours is still
the v1.3.0 copy at `Assets/Samples/XR Hands/1.3.0/`. After bumping, re-import
the sample from Package Manager → XR Hands → Samples, then delete the old 1.3.0
folder. `Assets/Scripts` has one `IXRHandProcessor` implementation to re-check.

## Step 5 — Verify

- All ~30 scenes open without missing-script (`Mono Script`) warnings
- The 3 Shader Graphs still compile; check `FauxBlurURP.shader` visually
- Quest build (Android/IL2CPP) — the Oculus loader is the only XR loader
  assigned to any platform
- One WebGL lab scene builds
- Grab / socket / poke interactions still work — XRI stayed on 2.6.5, so this
  should be unchanged, but it is the highest-value thing to smoke-test

---

## Deliberately deferred

- **XRI 2.6.5 → 3.6.0.** XRI 3 renames namespaces, removes `XRController`
  entirely (we have 16 uses across 2 files) and splits the interactor classes.
  That is a real refactor across 37 files *plus* rewiring every prefab and scene
  component reference. Keeping XRI at 2.6.5 — which still resolves on Unity 6 —
  isolates the editor upgrade so any breakage is unambiguously the editor's
  fault, not XRI's. Do XRI 3 as its own branch afterwards.
- **`com.unity.connect.share`.** Deprecated and unreferenced in C#, but
  `webgl_sharing` holds a live Unity Play project GUID, so it is deliberately
  kept. Drop it if it errors on Unity 6.
(`com.unity.xr.mock-hmd` was on this list and has since been removed — see the
commit table above.)

## Rollback

Every step is a separate commit, and no other branch was touched:

```bash
git checkout zack-GlasswareTesting
```

Then reopen with 2022.3.62f3. Delete `Library/` if the editor is confused by the
Unity 6 artefacts left in it.

---

## The point of all this: Unity MCP

Two options once we are on 6.3.

**Unity's official MCP** — this is the one that actually required Unity 6:

1. Install `com.unity.ai.assistant` (use `2.18.0-pre.2`; the 2.x line needs
   `6000.0.60f1` or later, which 6.3.9 satisfies)
2. Edit → Project Settings → AI → Unity MCP; confirm Unity Bridge shows a green
   **Running**
3. Under Integrations, find Claude Desktop → **Configure**
4. Start Claude Desktop; Unity shows a "Pending Connection" notice → **Accept**
5. Test with the `Unity_ReadConsole` tool

**[CoplayDev/unity-mcp](https://github.com/CoplayDev/unity-mcp)** — third-party,
supports Unity 2021.3 → 6.x, so it would have worked on 2022.3 as well. Needs
Python 3.10+ via `uv`. Package Manager → Add package from git URL:

```
https://github.com/CoplayDev/unity-mcp.git?path=/MCPForUnity#main
```

then Window → MCP for Unity → Configure All Detected Clients.
