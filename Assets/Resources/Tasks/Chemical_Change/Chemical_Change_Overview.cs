using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Instruction popup for the Physical & Chemical Changes module (Lab 3, Reaction #1:
// the chromate-dichromate equilibrium). Popup text follows the module document
// (docs/VR Chemistry Module 3 - Physical and Chemical Changes UPDATED.docx), see docs/chemmodule_tasks.md.
// Each popup is ONE task, one per required action in the document (see docs/chemmodule_tasks.md).
// Every task is currently a click-through placeholder (Module3_ClickThroughStep);
// see docs/module3_task_sequence.md for how to replace each with its working piece.
//
// IMPORTANT: text[] and webGLText[] must have exactly one more entry than the number of
// taskStepPrefabs on Chemical_Change_Task.asset (the extra last entry is the completion message).
public class Chemical_Change_Overview : MonoBehaviour
{
    public TextMeshProUGUI ccText;
    int curStep;

    // Controllers for starting tasks
    [Header("Start Task Controllers")]
    public Start_Module taskPreper;

    // Read-and-dismiss tasks (0-based indices into text[]; see docs/module3_task_sequence.md:
    // tasks 1-3, 5, 28, 31-32). They show in a large 16:9 panel with an Advance button.
    // Real tasks (task 4 onward) use the normal small corner popup.
    [Header("Info-only (read and Advance) panel")]
    [Tooltip("Panel width as a fraction of the screen width (WebGL / screen overlay).")]
    [SerializeField] float panelWidthFraction = 0.8f;
    [Tooltip("Distance in front of the camera for the world-space (VR) panel.")]
    [SerializeField] float vrPanelDistance = 1.2f;
    static readonly HashSet<int> infoSteps = new HashSet<int> { 0, 1, 2, 4, 27, 30, 31 };

    const float PanelW = 1600f, PanelH = 900f; // 16:9 in canvas units

    Transform popup;            // the corner popup quad, hidden while the info panel shows
    Vector3 cornerLocalScale;
    bool poseCaptured;
    bool infoMode;              // current task is read-only
    bool panelShown;
    bool useOverlay;            // screen overlay (WebGL) vs world-space (VR)
    GameObject panelRoot;
    Image panelBg;
    static readonly Color PanelColor = new Color(0.05f, 0.09f, 0.14f, 0.96f);
    static readonly Color PanelFlashColor = new Color(0.25f, 0.45f, 0.65f, 0.96f);
    // True while the large centered panel (not the corner popup) is the active display.
    public static bool InfoModeActive { get; private set; }
    TextMeshProUGUI panelText;
    RectTransform advanceRect;
    ToggleTextSimple toggle;
    Camera cam;
    FirstPersonMovement firstPerson;

    // WebGL builds show the keyboard/mouse wording.
    static bool IsWebGL => Application.platform == RuntimePlatform.WebGLPlayer;
    string[] CurrentText => IsWebGL ? webGLText : text;

    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = 0;
        ccText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            ccText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAbandonTask += cc_abandonMe;
        GameEventsManager.instance.taskEvents.onAdvanceTask += AdvanceCheTask;
        GameEventsManager.instance.miscEvents.onTextPopUp += FlashPanel;

        //Start the Module on opening this scene
        taskPreper.Show();
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
        GameEventsManager.instance.taskEvents.StartTask("Chemical_Change_Task");
        ccText.text = CurrentText[curStep];
        CapturePose();
        BuildInfoPanel();
        UpdateMode();
    }

    void OnDisable()
    {
        RestoreCorner();
        //Remove any listeners
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        GameEventsManager.instance.taskEvents.onAbandonTask -= cc_abandonMe;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= AdvanceCheTask;
        GameEventsManager.instance.miscEvents.onTextPopUp -= FlashPanel;
        InfoModeActive = false;
    }
    bool IsInfoStep(int step) => infoSteps.Contains(step);

    void CapturePose()
    {
        if (poseCaptured || ccText == null) return;
        // Walk up from the text's canvas to the child of this object (the popup quad).
        Transform t = ccText.canvas != null ? ccText.canvas.transform : ccText.transform;
        while (t.parent != null && t.parent != transform) t = t.parent;
        popup = t;
        cornerLocalScale = popup.localScale;
        toggle = GetComponent<ToggleTextSimple>();
        poseCaptured = true;
    }

    // Builds the big 16:9 panel (dark background, text, Advance button) in code.
    void BuildInfoPanel()
    {
        if (panelRoot != null || ccText == null) return;
        firstPerson = FindFirstObjectByType<FirstPersonMovement>();
        useOverlay = IsWebGL || firstPerson != null;
        int layer = ccText.gameObject.layer;

        panelRoot = new GameObject("Info Panel (runtime)", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
        panelRoot.layer = layer;
        var canvas = panelRoot.GetComponent<Canvas>();
        var rootRect = panelRoot.GetComponent<RectTransform>();
        if (useOverlay)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = panelRoot.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
        }
        else
        {
            canvas.renderMode = RenderMode.WorldSpace;
            rootRect.sizeDelta = new Vector2(PanelW, PanelH);
        }

        var bg = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        bg.layer = layer;
        bg.transform.SetParent(panelRoot.transform, false);
        var br = bg.GetComponent<RectTransform>();
        br.anchorMin = br.anchorMax = br.pivot = new Vector2(0.5f, 0.5f);
        // Overlay: 16:9 sized as a fraction of the 1920-wide reference. World: the full canvas.
        float w = useOverlay ? 1920f * panelWidthFraction : PanelW;
        br.sizeDelta = new Vector2(w, w * 9f / 16f);
        br.anchoredPosition = Vector2.zero;
        panelBg = bg.GetComponent<Image>();
        panelBg.color = PanelColor;
        float k = br.sizeDelta.x / PanelW; // scale layout metrics with panel size

        var textGo = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textGo.layer = layer;
        textGo.transform.SetParent(bg.transform, false);
        var tr = textGo.GetComponent<RectTransform>();
        tr.anchorMin = Vector2.zero; tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2(90f * k, 190f * k);
        tr.offsetMax = new Vector2(-90f * k, -70f * k);
        panelText = textGo.GetComponent<TextMeshProUGUI>();
        panelText.font = ccText.font;
        panelText.alignment = TextAlignmentOptions.TopLeft;
        panelText.enableAutoSizing = true;
        panelText.fontSizeMin = 28f * k;
        panelText.fontSizeMax = 56f * k;
        panelText.color = Color.white;
        panelText.raycastTarget = false;

        var btn = new GameObject("Advance Button", typeof(RectTransform), typeof(Image));
        btn.layer = layer;
        btn.transform.SetParent(bg.transform, false);
        advanceRect = btn.GetComponent<RectTransform>();
        advanceRect.anchorMin = advanceRect.anchorMax = advanceRect.pivot = new Vector2(0.5f, 0f);
        advanceRect.sizeDelta = new Vector2(440f * k, 100f * k);
        advanceRect.anchoredPosition = new Vector2(0f, 50f * k);
        btn.GetComponent<Image>().color = new Color(0.15f, 0.55f, 0.25f, 1f);

        var label = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        label.layer = layer;
        label.transform.SetParent(btn.transform, false);
        var lr = label.GetComponent<RectTransform>();
        lr.anchorMin = Vector2.zero; lr.anchorMax = Vector2.one;
        lr.offsetMin = lr.offsetMax = Vector2.zero;
        var tmp = label.GetComponent<TextMeshProUGUI>();
        tmp.font = ccText.font;
        tmp.text = useOverlay ? "Next (N)" : "Next (A)";
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 44f * k;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        panelRoot.SetActive(false);
    }

    // Title line larger and bold, rest as normal body text.
    void SetPanelText(string full)
    {
        if (panelText == null) return;
        int nl = full.IndexOf('\n');
        panelText.text = nl < 0 ? full : "<size=125%><b>" + full.Substring(0, nl) + "</b></size>" + full.Substring(nl);
    }

    // Info-only tasks: big panel, small popup hidden. Real tasks: corner popup only.
    void UpdateMode()
    {
        if (!poseCaptured) return;
        infoMode = IsInfoStep(curStep) && curStep < CurrentText.Length - 1;
        InfoModeActive = infoMode;
        popup.localScale = infoMode ? Vector3.zero : cornerLocalScale;
        if (infoMode) SetPanelText(ccText.text);
        RefreshPanelVisibility();
    }

    // The panel follows the Y/Z popup toggle, and the cursor is only freed while it is showing.
    void RefreshPanelVisibility()
    {
        bool show = infoMode && (toggle == null || toggle.IsTextVisible());
        if (panelRoot != null && panelRoot.activeSelf != show) panelRoot.SetActive(show);
        if (show != panelShown)
        {
            panelShown = show;
            SetCursorFree(show);
        }
    }

    // WebGL locks the cursor for mouse-look. While the panel is up, free the cursor
    // (and freeze look/move) so the Advance button can be clicked; re-lock afterwards.
    void SetCursorFree(bool free)
    {
        if (firstPerson == null) firstPerson = FindFirstObjectByType<FirstPersonMovement>();
        if (firstPerson == null) return; // VR scenes: nothing to do
        if (free)
        {
            firstPerson.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Time.timeScale != 0f) // not while the pause menu owns the cursor
        {
            firstPerson.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void RestoreCorner()
    {
        infoMode = false;
        InfoModeActive = false;
        if (poseCaptured && popup != null) popup.localScale = cornerLocalScale;
        if (panelRoot != null) panelRoot.SetActive(false);
        if (panelShown) { panelShown = false; SetCursorFree(false); }
    }

    // The corner highlight flashes at the corner popup, which is hidden in info mode,
    // so flash the large panel instead. Wait a frame so UpdateMode has run for the new step.
    void FlashPanel()
    {
        if (isActiveAndEnabled) StartCoroutine(FlashPanelRoutine());
    }

    IEnumerator FlashPanelRoutine()
    {
        yield return null;
        if (!infoMode || panelBg == null) yield break;
        for (int i = 0; i < 2; i++)
        {
            panelBg.color = PanelFlashColor;
            yield return new WaitForSeconds(0.175f);
            panelBg.color = PanelColor;
            yield return new WaitForSeconds(0.175f);
        }
    }

    void PlaceWorldPanel()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;
        float viewH = 2f * vrPanelDistance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float width = Mathf.Min(viewH * cam.aspect * 0.75f, viewH * 0.8f * (PanelW / PanelH));
        var t = panelRoot.transform;
        t.position = cam.transform.position + cam.transform.forward * vrPanelDistance;
        t.rotation = cam.transform.rotation;
        t.localScale = Vector3.one * (width / PanelW);
    }

    void LateUpdate()
    {
        if (panelShown && !useOverlay) PlaceWorldPanel();
    }

    void Update()
    {
        if (!poseCaptured) return;
        RefreshPanelVisibility();
        // Mouse click on the Advance button (no EventSystem needed).
        if (!panelShown || advanceRect == null) return;
        var mouse = Mouse.current;
        if (mouse == null || !mouse.leftButton.wasPressedThisFrame) return;
        Camera hitCam = useOverlay ? null : (cam != null ? cam : Camera.main);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(advanceRect, mouse.position.ReadValue(), hitCam, out Vector2 local)
            && advanceRect.rect.Contains(local))
        {
            ClickNext();
        }
    }

    public void restart()
    {
        cc_abandonMe("Chemical_Change_Overview");
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
    }

    // Hook a UI Button's OnClick to this to advance a click-through placeholder step.
    public void ClickNext()
    {
        GameEventsManager.instance.miscEvents.ClickThroughNext();
    }

    // VR wording (controllers)
    string[] text = {
                    /*0*/"Task 1 of 32: Background: reaction #1\nStation: 1 PPE\n\nIn this experiment, 9 reactions at equilibrium will be perturbed, and the resulting chemical and physical changes will be observed. A physical change occurs when matter changes form. A chemical change occurs when new products are formed. For this simulation, you will do reaction #1 from Lab 3: The Law of Mass Action: Le Chatelier's Principle.",
                    /*1*/"Task 2 of 32: Background: Le Chatelier's Principle\nStation: 1 PPE\n\nLe Chatelier's Principle allows us to predict the effect of change upon a chemical system in equilibrium. It says that if stress is imposed on a system at equilibrium, the equilibrium concentrations of reactants and products will shift in such a manner as to relieve the stress.",
                    /*2*/"Task 3 of 32: Safety precaution\nStation: 1 PPE\n\nSAFETY PRECAUTION: Several of the reactions in this experiment use acids and bases at concentrations of 1 M or more. Be extremely careful when using high concentrations of corrosive chemicals such as acids and bases. All spills or drips should be cleaned up immediately as directed by your TA. Be particularly careful not to leave drops of acid or base on the sides of the bottles.",
                    /*3*/"Task 4 of 32: Put on goggles, gloves and lab coat\nStation: 1 PPE\n\nBefore starting any chemistry experiment, you must wear proper Personal Protective Equipment (PPE). At the PPE station, press the goggles button and then the lab coat button, then put on your gloves. Each item equips automatically, and your hands turn blue to show that you are wearing gloves.",
                    /*4*/"Task 5 of 32: Before you start: lab notes\nStation: 1 PPE\n\nGoggles protect your eyes from splashes; lab coats protect your skin and clothing. In this lab you will generate metal ion waste, disposed of in the fume hood, and glass waste (the test tubes). This is a qualitative experiment: measurements do not have to be exact, detailed observations of color, precipitation and temperature changes are what matter most.",
                    /*5*/"Task 6 of 32: Set up the test tubes\nStation: 5 TUBE\n\nGather your equipment. At the TUBE station, put the test tube rack in your work area, place two test tubes inside it, and pick up the glass stir rod.",
                    /*6*/"Task 7 of 32: Estimate 1 mL\nStation: 2 MIXING\n\nThis lab uses approximately 1 mL volumes, which do not have to be exact. Take the squirt bottle of water and a disposable pipette from the bench and measure about 1 mL of water into one test tube to learn the amount. Also place the 10 mL graduated cylinder in your work area.",
                    /*7*/"Task 8 of 32: Label the test tubes\nStation: 2 MIXING\n\nLabel one empty test tube #1 and the other #2 with the black Sharpie.",
                    /*8*/"Task 9 of 32: Collect the reagents\nStation: 3 HOOD\n\nGo to the fume hood. The reagents are stored in two trays labeled acids and bases, and each bottle has a disposable pipette taped to it. Bring the potassium chromate (K2CrO4), sodium hydroxide (NaOH) and sulfuric acid (H2SO4) bottles to your work area.",
                    /*9*/"Task 10 of 32: Measure 1 mL of chromate\nStation: 2 MIXING\n\nPick up the graduated cylinder and hold it under the potassium chromate reagent bottle. Pour: the yellow solution stops flowing automatically at exactly 1 mL.",
                    /*10*/"Task 11 of 32: Pour into test tube #1\nStation: 2 MIXING\n\nPour the yellow potassium chromate from the cylinder into test tube #1.",
                    /*11*/"Task 12 of 32: Repeat for test tube #2\nStation: 2 MIXING\n\nMeasure another 1 mL of potassium chromate in the cylinder and pour it into test tube #2.",
                    /*12*/"Task 13 of 32: NaOH #1: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.",
                    /*13*/"Task 14 of 32: NaOH #1: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #1 and push the button to add drops, drop by drop. The system stops automatically after four drops.",
                    /*14*/"Task 15 of 32: NaOH #1: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #1 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.",
                    /*15*/"Task 16 of 32: NaOH #1: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.",
                    /*16*/"Task 17 of 32: NaOH #1: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #1, then pick up the pen and write it in the lab notebook.",
                    /*17*/"Task 18 of 32: H2SO4 #2: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 3 M sulfuric acid: pick it up in one hand and its dropper in the other, then put the bottle down.",
                    /*18*/"Task 19 of 32: H2SO4 #2: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.",
                    /*19*/"Task 20 of 32: H2SO4 #2: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.",
                    /*20*/"Task 21 of 32: H2SO4 #2: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.",
                    /*21*/"Task 22 of 32: H2SO4 #2: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.",
                    /*22*/"Task 23 of 32: NaOH #2: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.",
                    /*23*/"Task 24 of 32: NaOH #2: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.",
                    /*24*/"Task 25 of 32: NaOH #2: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.",
                    /*25*/"Task 26 of 32: NaOH #2: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.",
                    /*26*/"Task 27 of 32: NaOH #2: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.",
                    /*27*/"Task 28 of 32: Waste reminder\nStation: 3 HOOD\n\nWASTE REMINDER: Dispose of waste in the designated waste containers. Metal ion waste should NEVER be poured down the drain. You are not yet qualified to distinguish when acids and bases may be poured down the drain, so you should always dispose of acid and base waste in the designated containers.",
                    /*28*/"Task 29 of 32: Empty the test tubes\nStation: 3 HOOD\n\nTake test tubes #1 and #2 to the fume hood and empty both into the chemical waste container.",
                    /*29*/"Task 30 of 32: Test tubes to glass waste\nStation: 4 SINK\n\nPut both empty test tubes in the glass waste.",
                    /*30*/"Task 31 of 32: Answer: what happened\nStation: 2 MIXING\n\nMixing chromate ions (CrO4 2-) with sodium hydroxide (NaOH) keeps the chromium in its yellow chromate form. Because NaOH is a base, it prevents the solution from becoming acidic, which stops yellow chromate from turning into orange dichromate (Cr2O7 2-). NaOH adds hydroxide ions (OH-), which remove free hydrogen ions (H+), so the equilibrium shifts toward chromate. No precipitate forms; the solution stays a clear yellow liquid.",
                    /*31*/"Task 32 of 32: Safety and handling\nStation: 2 MIXING\n\nSodium hydroxide is a strong alkali and causes severe skin burns. Chromate compounds contain hexavalent chromium, which is toxic and harmful to the environment. Always wear gloves and eye protection.",
                    /*32*/"This concludes the Physical and Chemical Changes module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
            };

    // WebGL wording (keyboard / mouse)
    string[] webGLText = {
                    /*0*/"Task 1 of 32: Background: reaction #1\nStation: 1 PPE\n\nIn this experiment, 9 reactions at equilibrium will be perturbed, and the resulting chemical and physical changes will be observed. A physical change occurs when matter changes form. A chemical change occurs when new products are formed. For this simulation, you will do reaction #1 from Lab 3: The Law of Mass Action: Le Chatelier's Principle.",
                    /*1*/"Task 2 of 32: Background: Le Chatelier's Principle\nStation: 1 PPE\n\nLe Chatelier's Principle allows us to predict the effect of change upon a chemical system in equilibrium. It says that if stress is imposed on a system at equilibrium, the equilibrium concentrations of reactants and products will shift in such a manner as to relieve the stress.",
                    /*2*/"Task 3 of 32: Safety precaution\nStation: 1 PPE\n\nSAFETY PRECAUTION: Several of the reactions in this experiment use acids and bases at concentrations of 1 M or more. Be extremely careful when using high concentrations of corrosive chemicals such as acids and bases. All spills or drips should be cleaned up immediately as directed by your TA. Be particularly careful not to leave drops of acid or base on the sides of the bottles.",
                    /*3*/"Task 4 of 32: Put on goggles, gloves and lab coat\nStation: 1 PPE\n\nBefore starting any chemistry experiment, you must wear proper Personal Protective Equipment (PPE). At the PPE station, press the goggles button and then the lab coat button, then put on your gloves. Each item equips automatically, and your hands turn blue to show that you are wearing gloves.",
                    /*4*/"Task 5 of 32: Before you start: lab notes\nStation: 1 PPE\n\nGoggles protect your eyes from splashes; lab coats protect your skin and clothing. In this lab you will generate metal ion waste, disposed of in the fume hood, and glass waste (the test tubes). This is a qualitative experiment: measurements do not have to be exact, detailed observations of color, precipitation and temperature changes are what matter most.",
                    /*5*/"Task 6 of 32: Set up the test tubes\nStation: 5 TUBE\n\nGather your equipment. At the TUBE station, put the test tube rack in your work area, place two test tubes inside it, and pick up the glass stir rod.",
                    /*6*/"Task 7 of 32: Estimate 1 mL\nStation: 2 MIXING\n\nThis lab uses approximately 1 mL volumes, which do not have to be exact. Take the squirt bottle of water and a disposable pipette from the bench and measure about 1 mL of water into one test tube to learn the amount. Also place the 10 mL graduated cylinder in your work area.",
                    /*7*/"Task 8 of 32: Label the test tubes\nStation: 2 MIXING\n\nLabel one empty test tube #1 and the other #2 with the black Sharpie.",
                    /*8*/"Task 9 of 32: Collect the reagents\nStation: 3 HOOD\n\nGo to the fume hood. The reagents are stored in two trays labeled acids and bases, and each bottle has a disposable pipette taped to it. Bring the potassium chromate (K2CrO4), sodium hydroxide (NaOH) and sulfuric acid (H2SO4) bottles to your work area.",
                    /*9*/"Task 10 of 32: Measure 1 mL of chromate\nStation: 2 MIXING\n\nPick up the graduated cylinder and hold it under the potassium chromate reagent bottle. Pour: the yellow solution stops flowing automatically at exactly 1 mL.",
                    /*10*/"Task 11 of 32: Pour into test tube #1\nStation: 2 MIXING\n\nPour the yellow potassium chromate from the cylinder into test tube #1.",
                    /*11*/"Task 12 of 32: Repeat for test tube #2\nStation: 2 MIXING\n\nMeasure another 1 mL of potassium chromate in the cylinder and pour it into test tube #2.",
                    /*12*/"Task 13 of 32: NaOH #1: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.",
                    /*13*/"Task 14 of 32: NaOH #1: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #1 and push the button to add drops, drop by drop. The system stops automatically after four drops.",
                    /*14*/"Task 15 of 32: NaOH #1: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #1 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.",
                    /*15*/"Task 16 of 32: NaOH #1: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.",
                    /*16*/"Task 17 of 32: NaOH #1: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #1, then pick up the pen and write it in the lab notebook.",
                    /*17*/"Task 18 of 32: H2SO4 #2: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 3 M sulfuric acid: pick it up in one hand and its dropper in the other, then put the bottle down.",
                    /*18*/"Task 19 of 32: H2SO4 #2: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.",
                    /*19*/"Task 20 of 32: H2SO4 #2: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.",
                    /*20*/"Task 21 of 32: H2SO4 #2: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.",
                    /*21*/"Task 22 of 32: H2SO4 #2: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.",
                    /*22*/"Task 23 of 32: NaOH #2: get the bottle and dropper\nStation: 2 MIXING\n\nOpen the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.",
                    /*23*/"Task 24 of 32: NaOH #2: add 4 drops\nStation: 2 MIXING\n\nHold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.",
                    /*24*/"Task 25 of 32: NaOH #2: stir\nStation: 2 MIXING\n\nPlace the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.",
                    /*25*/"Task 26 of 32: NaOH #2: physical or chemical?\nStation: 2 MIXING\n\nIs this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.",
                    /*26*/"Task 27 of 32: NaOH #2: record the observation\nStation: 2 MIXING\n\nSelect the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.",
                    /*27*/"Task 28 of 32: Waste reminder\nStation: 3 HOOD\n\nWASTE REMINDER: Dispose of waste in the designated waste containers. Metal ion waste should NEVER be poured down the drain. You are not yet qualified to distinguish when acids and bases may be poured down the drain, so you should always dispose of acid and base waste in the designated containers.",
                    /*28*/"Task 29 of 32: Empty the test tubes\nStation: 3 HOOD\n\nTake test tubes #1 and #2 to the fume hood and empty both into the chemical waste container.",
                    /*29*/"Task 30 of 32: Test tubes to glass waste\nStation: 4 SINK\n\nPut both empty test tubes in the glass waste.",
                    /*30*/"Task 31 of 32: Answer: what happened\nStation: 2 MIXING\n\nMixing chromate ions (CrO4 2-) with sodium hydroxide (NaOH) keeps the chromium in its yellow chromate form. Because NaOH is a base, it prevents the solution from becoming acidic, which stops yellow chromate from turning into orange dichromate (Cr2O7 2-). NaOH adds hydroxide ions (OH-), which remove free hydrogen ions (H+), so the equilibrium shifts toward chromate. No precipitate forms; the solution stays a clear yellow liquid.",
                    /*31*/"Task 32 of 32: Safety and handling\nStation: 2 MIXING\n\nSodium hydroxide is a strong alkali and causes severe skin burns. Chromate compounds contain hexavalent chromium, which is toxic and harmful to the environment. Always wear gloves and eye protection.",
                    /*32*/"This concludes the Physical and Chemical Changes module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"
            };

    void AdvanceCheTask(string context)
    {
        if (context.Contains("Chemical_Change_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            ccText.text = CurrentText[Mathf.Min(curStep, CurrentText.Length - 1)];
            UpdateMode();
        }
    }
    public void cc_abandonMe(string context)
    {
        if (context.Contains("Chemical"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
            GameObject ccPop = GameObject.Find("Chemical Change PopUp");
            ccText.text = "Don't look at me I'm inactive.";
            if (ccPop != null)
            {
                ccPop.SetActive(false);
            }
            curStep = 0;
            RestoreCorner();
        }
    }
}
