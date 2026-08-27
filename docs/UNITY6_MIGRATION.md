# ChemVR — Unity 2022.3 → Unity 6.3 LTS migration

**Branch:** `unity6-migration` (off `zack-GlasswareTesting`)
**Editor:** `6000.3.9f1` (Unity 6.3 LTS, supported through Dec 2027)
**Status:** migration complete — project compiles, scenes load, no Safe Mode.

`main` and `zack-GlasswareTesting` are untouched and still open in 2022.3.62f3.

---

## Outcome

| | 2022.3 | Unity 6.3 |
|---|---|---|
| URP | 14.0.12 | **17.3.0** |
| XR Interaction Toolkit | 2.6.5 | **3.3.1** |
| XR Hands | 1.4.0 | **1.7.3** |
| TextMeshPro | `com.unity.textmeshpro` 3.0.7 | **bundled `com.unity.ugui` 2.0.0** |
| Timeline / Visual Scripting / collab-proxy | 1.7.7 / 1.9.4 / 2.7.1 | 1.8.10 / 1.9.9 / 2.11.3 |
| Removed | AR Foundation, ARCore, mock-hmd, iet-framework | — |

### The XRI surprise

We deliberately pinned XRI at **2.6.5** to isolate the editor upgrade from an API
refactor. **Unity's upgrader overrode that pin and resolved 3.3.1 anyway.** Plan
adjusted: we took XRI 3 in the same pass.

It was cheaper than feared. Every type still exists in 3.3.1 — they only moved
out of the root namespace:

| Types | New namespace |
|---|---|
| `XRGrabInteractable`, `XRBaseInteractable`, `XRSimpleInteractable`, `IXRSelectInteractable` | `.Interactables` |
| `XRRayInteractor`, `XRDirectInteractor`, `XRBaseInteractor`, `IXRInteractor`, `IXRSelectInteractor`, `XRInteractionGroup`, `IXRGroupMember` | `.Interactors` |
| `XRInteractorLineVisual` | `.Interactors.Visuals` |
| `XRPokeFilter` | `.Filtering` |

So 45 of the 46 compile errors were missing `using` directives. Unity's API
Updater "fixed" them by expanding every reference to an inline fully-qualified
name; we replaced that with short names plus a `using`, across 27 files.

**One genuine API break**, in `DialRotator.GrabbedBy` — `selectingInteractor`
was removed in XRI 3:

```csharp
interactor = GetComponent<XRGrabInteractable>().GetOldestInteractorSelecting() as XRBaseInteractor;
```

(the cast is required because that call returns `IXRSelectInteractor`).

### Why this was low-risk

The pre-flight survey found no custom `ScriptableRendererFeature` or
`ScriptableRenderPass` — normally the worst URP 14 → 17 breakage, since Unity 6
makes Render Graph the default. Also no `WWW`, no `UnityWebRequest`, no
`Graphics.Blit`, and one deprecated API across 229 scripts.

---

## Commits

| Commit | Change |
|---|---|
| `4d614ad1` | Removed `xr.arcore` + `xr.arfoundation` (entirely unused) and their orphaned assets |
| `5336a518` | Bumped the packages valid on both 2022.3 and Unity 6 |
| `75860f04` | Regenerated `packages-lock.json` |
| `fb73a80b` | Removed `learn.iet-framework` + the stock VR template tutorial |
| `1adc9c0d` | Removed `xr.mock-hmd` 1.4.0-preview.2 |
| `e0ca758e` | **XRI 3.3.1 namespace + API migration** (27 files) |
| `8b870cb3` | URP 17 material re-serialization (148 files) |
| `2a94ba4a` | Unity 6 / URP 17 project settings upgrade |
| `afc56a0c` | Unity 6 TMP Essential Resources import |

Steps before the editor upgrade were each verified by a headless 2022.3
`-batchmode` run (exit 0, no compile or resolution errors).

---

## Post-upgrade verification

**Missing-script audit.** Every `m_Script` GUID referenced across all scenes and
prefabs (300 distinct) was checked against every `.meta` in `Assets/` and
`Library/PackageCache/` (16,087 known GUIDs).

**All XRI component GUIDs resolve** — XRI kept them stable across 2 → 3:

| Component | Scene references |
|---|---|
| `XRGrabInteractable` | 279 |
| `XRSimpleInteractable` | 30 |
| `XRRayInteractor` | 28 |
| `XRInteractorLineVisual` | 14 |
| `XRDirectInteractor` / `XRInteractionManager` | 3 each |

TextMeshPro GUIDs also resolve — the TMP import changed **zero** GUIDs, so all
font and material references survived.

22 GUIDs do not resolve, and **none are migration-caused**:

- **11** — your own scripts deleted long ago; `git log -S` traces them to the
  initial commit and the 2022 LTS migration
- **10** — old Oculus Integration / Interaction SDK (`_pointable`,
  `_planeSurface`, `_transferOnSecondSelection`, `m_allowOffhandGrab`). That
  package was stripped from `Assets/Oculus` long ago — 0 `.cs` files remain
- **1** — in `Assets/Samples/XR Hands/1.3.0/HandVisualizer.unity`, a demo scene
  that is not shipped

---

## Still outstanding

1. **Rebake lighting.** Unity 6 changed the lightmap format, so
   `LightingData` assets are incompatible. Affects scenes with baked lightmaps
   (`Scenes/LabScene/Materials/Lightmap-*`). Scenes render with realtime/ambient
   until rebaked — Window → Rendering → Lighting → Generate Lighting.
2. **Stale samples.** `Assets/Samples/XR Interaction Toolkit/2.4.3/` and
   `2.5.2/`, and `XR Hands/1.3.0/`, are sample code from the old package
   versions. Deliberately **not** deleted — VR template scenes reference the
   Starter Assets prefabs, so deleting breaks scene references. Re-import from
   Package Manager and repoint references if you want them current.
3. **Build verification.** Quest (Android/IL2CPP) and one WebGL lab scene.
   Oculus is the only XR loader assigned to any platform.
4. **XRI deprecation warnings.** The Affordance System
   (`Vector3TweenableVariable`), `ContinuousTurnProviderBase`, and
   `hideControllerOnSelect` are obsolete-but-working in XRI 3. Non-blocking;
   worth a follow-up branch.
5. **`com.unity.connect.share`.** Deprecated, unreferenced in C#, but
   `webgl_sharing` holds a live Unity Play project GUID, so it was kept.

## Rollback

Every step is a separate commit and no other branch was touched:

```bash
git checkout zack-GlasswareTesting
```

Then reopen with 2022.3.62f3, deleting `Library/` if the editor is confused by
Unity 6 artefacts.

---

## Unity MCP

`com.unity.ai.assistant` 2.18.0-pre.2 has been added to the manifest. This is
the package that actually required Unity 6 — the reason for this migration.

In the editor:

1. Edit → Project Settings → **AI → Unity MCP**; confirm the Unity Bridge shows
   a green **Running**. (Unity will ask you to accept its AI terms — that is an
   account-level agreement, so it is yours to accept, not something I can do.)
2. Under **Integrations**, find Claude Desktop → **Configure**
3. Start Claude Desktop. Unity shows a **Pending Connection** notice → **Accept**
4. Verify Claude Desktop lists Unity tools such as `Unity_ReadConsole`

**Alternative — [CoplayDev/unity-mcp](https://github.com/CoplayDev/unity-mcp).**
Third-party, supports Unity 2021.3 → 6.x, needs Python 3.10+ via `uv`. Package
Manager → Add package from git URL:

```
https://github.com/CoplayDev/unity-mcp.git?path=/MCPForUnity#main
```

then Window → MCP for Unity → Configure All Detected Clients.
