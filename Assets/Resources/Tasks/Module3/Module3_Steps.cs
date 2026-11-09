using System.Collections.Generic;

/// <summary>
/// Single source of truth for the Module 3 (Physical and Chemical Changes, Reaction #1) step table.
/// Drives the instruction popup (Module3_Overview) while steps are still click-through placeholders.
/// Order here MUST match the taskStepPrefabs order on Module3_Task.asset.
/// See docs/module_environment.md and docs/module3_task_sequence.md.
/// </summary>
public static class Module3Steps
{
    public const string TaskId = "Module3_Task";

    public struct Step
    {
        public string key;          // matches the placeholder prefab name suffix
        public string title;
        public string station;
        public string[] labware;
        public string vrText;       // controller wording
        public string webText;      // mouse/keyboard wording
        public string replaceWith;  // what real interaction replaces the click-through
    }

    const string VrNext = "\n\n[placeholder step - press A to continue]";
    const string WebNext = "\n\n[placeholder step - press the skip key to continue]";

    public static readonly Step[] All = new Step[]
    {
        new Step { key = "00_Background", title = "Background", station = "PPE",
            labware = new string[] { },
            vrText = "Welcome to Physical and Chemical Changes, Reaction #1: the chromate-dichromate equilibrium.\nLe Chatelier's Principle: if stress is applied to a system at equilibrium, it shifts to relieve the stress.",
            webText = "Welcome to Physical and Chemical Changes, Reaction #1: the chromate-dichromate equilibrium.\nLe Chatelier's Principle: if stress is applied to a system at equilibrium, it shifts to relieve the stress.",
            replaceWith = "Popup dismiss (button press). Keep click-through or use a dismiss-popup step." },
        new Step { key = "01_PPE", title = "Put on PPE", station = "PPE",
            labware = new string[] { "goggles", "lab coat", "gloves" },
            vrText = "Put on your safety goggles, lab coat and gloves.",
            webText = "Put on your safety goggles, lab coat and gloves.",
            replaceWith = "Copy Put_On_Gear_CC: miscEvents.onPutOnRightGlove/onPutOnLeftGlove + WearCoat/WearGoggles.IsWearing()." },
        new Step { key = "02_Gather", title = "Gather equipment", station = "Mixing bench, lab drawer, fume hood",
            labware = new string[] { "2 test tubes", "test tube rack", "stir rod", "squirt bottle (DI)", "bench pipette", "Sharpie", "K2CrO4, NaOH, H2SO4 bottles", "notebook + pen" },
            vrText = "Gather your equipment. Label two test tubes #1 and #2 and bring the three reagent bottles from the fume hood to the bench.",
            webText = "Gather your equipment. Label two test tubes #1 and #2 and bring the three reagent bottles from the fume hood to the bench.",
            replaceWith = "Copy Gather_More_Tools / Bring_To_Hood: interactableEvents.onPlayerGrabInteractable per item (WebGL: webGLEvents.OnObjectGrabbed)." },
        new Step { key = "03_MeasureChromate", title = "Measure 1 mL potassium chromate", station = "Mixing bench",
            labware = new string[] { "graduated cylinder", "K2CrO4 bottle", "test tubes #1 and #2" },
            vrText = "Use the graduated cylinder to measure 1 mL of 0.1 M potassium chromate and pour it into test tubes #1 and #2.",
            webText = "Use the graduated cylinder to measure 1 mL of 0.1 M potassium chromate and pour it into test tubes #1 and #2.",
            replaceWith = "chemistryEvents.onPourIn / onPourOut with a 1 mL auto-stop (PourActivator)." },
        new Step { key = "04_AddNaOH_Tube1", title = "Add 4 drops of 1 M NaOH to test tube #1", station = "Mixing bench",
            labware = new string[] { "1 M NaOH bottle + dropper", "test tube #1" },
            vrText = "Hold the NaOH dropper over test tube #1 and add 4 drops. The solution should stay yellow.",
            webText = "Hold the NaOH dropper over test tube #1 and add 4 drops. The solution should stay yellow.",
            replaceWith = "chemistryEvents.onPipetteDispense counted to 4 drops (auto-stop)." },
        new Step { key = "05_Stir", title = "Stir the solution", station = "Mixing bench",
            labware = new string[] { "stir rod", "test tube #1" },
            vrText = "Stir test tube #1 with the glass rod for at least 5 seconds.",
            webText = "Stir test tube #1 with the glass rod for at least 5 seconds.",
            replaceWith = "Copy Stir_NaOH: miscEvents.OnStirBeaker accumulated to 5 s." },
        new Step { key = "06_Record", title = "Record observations", station = "Mixing bench",
            labware = new string[] { "lab notebook", "pen", "test tube #1" },
            vrText = "Is this a physical or a chemical change? Then record what you see in the lab notebook.",
            webText = "Is this a physical or a chemical change? Then record what you see in the lab notebook.",
            replaceWith = "Copy Ask_Q1 (yes/no buttons) + notebook statement picker. Correct answer for each tube is still undecided." },
        new Step { key = "07_AddH2SO4_Tube2", title = "Add 3 M sulfuric acid to test tube #2", station = "Mixing bench",
            labware = new string[] { "3 M H2SO4 bottle + dropper", "test tube #2", "stir rod", "notebook + pen" },
            vrText = "Add 4 drops of 3 M sulfuric acid to test tube #2, stir, and record what you see. The solution should turn orange.",
            webText = "Add 4 drops of 3 M sulfuric acid to test tube #2, stir, and record what you see. The solution should turn orange.",
            replaceWith = "Same pieces as steps 04-06 with the H2SO4 dropper." },
        new Step { key = "08_AddNaOH_Tube2", title = "Add 1 M NaOH to test tube #2", station = "Mixing bench",
            labware = new string[] { "1 M NaOH bottle + dropper", "test tube #2", "stir rod", "notebook + pen" },
            vrText = "Add 4 drops of 1 M NaOH to test tube #2, stir, and record what you see. The solution should return to yellow.",
            webText = "Add 4 drops of 1 M NaOH to test tube #2, stir, and record what you see. The solution should return to yellow.",
            replaceWith = "Same pieces as steps 04-06; drop count still to be confirmed." },
        new Step { key = "09_Waste", title = "Waste disposal", station = "Fume hood, sink and cleanup",
            labware = new string[] { "both test tubes", "chemical waste container", "glass waste bin", "sink + soap" },
            vrText = "Empty both test tubes into the chemical waste container in the fume hood, then put the tubes in the glass waste. Never pour metal ion waste down the drain.",
            webText = "Empty both test tubes into the chemical waste container in the fume hood, then put the tubes in the glass waste. Never pour metal ion waste down the drain.",
            replaceWith = "chemistryEvents.onFluidDispose + GlassDisposal; wash hands via Wash_Hands." },
        new Step { key = "10_WrapUp", title = "Wrap-up", station = "Any",
            labware = new string[] { },
            vrText = "NaOH removes H+, shifting the equilibrium toward yellow chromate; acid shifts it toward orange dichromate. No precipitate forms.",
            webText = "NaOH removes H+, shifting the equilibrium toward yellow chromate; acid shifts it toward orange dichromate. No precipitate forms.",
            replaceWith = "Keep click-through; end of module." },
    };

    public static string BuildText(int index, bool webgl)
    {
        if (index >= All.Length)
        {
            return "This concludes the Physical and Chemical Changes module. Thank you for your time.";
        }
        Step s = All[index];
        string body = webgl ? s.webText : s.vrText;
        string labware = (s.labware != null && s.labware.Length > 0) ? "\nLabware: " + string.Join(", ", s.labware) : "";
        return "Step " + (index + 1) + " of " + All.Length + ": " + s.title + "\nStation: " + s.station + labware + "\n\n" + body + (webgl ? WebNext : VrNext);
    }
}
