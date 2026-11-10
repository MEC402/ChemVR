# Module 3 (Physical & Chemical Changes) - Task Instructions

**Source of truth:** `docs/VR Chemistry Module 3 - Physical and Chemical Changes UPDATED.docx` (updated 10 Aug 2026 with the USC lab coordinator). These instructions follow that document step for step. Where the scene or the `Chemical_Change_Overview.cs` popup text differs from the document, the document wins; the differences are listed at the end.

The module mimics Reaction #1 of Lab Exercise #3, "The Law of Mass Action: Le Chatelier's Principle" (CHEM 105B manual, p. 16): the chromate-dichromate equilibrium. Estimated duration 12-15 minutes. **Linear step-gating:** the student must complete each step before the next one starts. Design philosophy: minimal fine motor skill (auto-stopping measurements, button-based controls, forgiving interactions).

**Station labels** (the numbered signs in the scene; the document itself does not name stations, so this mapping is the scene's):

| Station | Name | Used for |
|---|---|---|
| 1 | PPE | Step 0 |
| 2 | MIXING | Steps 1-7 (working bench: squirt bottle, pipette, Sharpie, pen, notebook, graduated cylinder) |
| 3 | HOOD | Reagent bottles in the Acids / Bases trays; metal-ion waste |
| 4 | SINK | Glass waste |
| 5 | TUBE | Test tubes, rack, stir rod |

---

## Step 0 - Background
- **Popup 1:** "In this experiment, 9 reactions at equilibrium will be perturbed, and the resulting chemical and physical changes will be observed. A physical change (solid, liquid, gas) occurs when matter changes form. A chemical change occurs when new products are formed. For this simulation, you will do reaction #1 from Lab 3: The Law of Mass Action: Le Chatelier's Principle."
- **Popup 2:** "Le Chatelier's Principle allows us to predict the effect of change upon a chemical system in equilibrium. It says that if stress is imposed on a system at equilibrium, the equilibrium concentration of reactants and products will shift in such a manner as to relieve the stress."
- **Do:** read both popups.

## Step 0 - Safety first: put on PPE
- **Station:** 1 PPE
- **Safety popup:** "SAFETY PRECAUTION: Several of the reactions in this experiment use acids and bases at concentrations ≥ 1M. Make sure your safety table includes all safety hazards. Be extremely careful when using high concentrations of corrosive chemicals such as acids and bases. All spills or drips should be cleaned up immediately as directed by your TA. Be particularly careful not to leave drops of acid or base on the sides of the bottles."
- **Player instruction:** "Before starting any chemistry experiment, you must wear proper Personal Protective Equipment (PPE). Put on your safety goggles and lab coat."
- **Required actions:**
  1. Approach the PPE station.
  2. Press the goggles button: the goggles equip automatically with an animation.
  3. Press the lab coat button: the coat equips automatically with an animation.
  4. Put on gloves: the player's hands turn blue.
- **Success criteria:** goggles and lab coat equipped (the student can see the goggle frame edge and the coat on their arms). Checkmarks appear on each PPE item once equipped. Auto-advance to Step 1.
- **Hint (15 s):** "Look for the safety goggles and lab coat at the PPE station. Grab each item and bring it toward your body to put it on."
- **Popups after equipping:**
  - "Safety goggles protect your eyes from chemical splashes and flying particles. Lab coats protect your skin and clothing from spills."
  - "Note: In this lab, you will generate metal ion waste. It will be disposed of in the fume hood. You may also generate glass waste. The test tubes are thrown out in glass waste."
  - (Shown as the student is about to start.) "This is a qualitative experiment (no calculations). In a QUALITATIVE experiment, measurements do not have to be exact; detailed observations are what is most important! Look out for changes as you mix things together inside glass test tubes. It is not necessary to determine the actual concentrations of reactants and products to determine if and how a chemical reaction responds to changes in conditions. Be on the lookout for changes in color, precipitation, temperature, etc."
- **Cannot proceed until:** the PPE items are equipped.

---

## Part 1, Reaction #1: the chromate-dichromate ion equilibrium

## Step 1 - Gather equipment
- **Stations:** 5 TUBE, 2 MIXING, 3 HOOD
- **Popup (as the step starts):** "This is a qualitative experiment (no calculations)... In this lab you will be mixing approximately 1 mL solutions, this measurement does not have to be exact."
- **Player instruction:** Gather two test tubes, a test tube rack, one glass stir rod and a squirt bottle filled with water (on the bench). Using a disposable plastic pipette (also on the bench), measure approximately 1 mL of water into one of the test tubes. Use this to estimate the amounts you will add in the later reactions. Then find the reagents: **potassium chromate (K₂CrO₄), sodium hydroxide (NaOH), sulfuric acid (H₂SO₄).**
- **Where the reagents are:** look around the room; the reagents are inside the fume hoods (there are several such stations in the lab). Each hood holds two secondary-containment trays, labeled **acids** and **bases**, each holding several bottles. Each bottle contains liquid and has a disposable pipette taped to it (held in a glass test tube) for dispensing. Tip: read the lab and look up which chemicals are acids and which are bases to find them faster and anticipate the changes.
- **Reference for the test tubes:** 19 mL capacity, 120 mm / 5 in long (Fisherbrand disposable borosilicate glass tubes, 10 pack).
- **Already on the lab bench:** black Sharpie, pen, lab notebook.
- **Required actions:**
  1. Pick up the items and place them in the work area or on their designated spots. All items must be collected.
  2. Place two test tubes inside the test tube rack.
  3. Pick up one empty test tube and label it **test tube #1** with the Sharpie.
  4. Pick up one empty test tube and label it **test tube #2** with the Sharpie.
- **Success criteria:** all 8 items present in the work zone. Message: "Good! You have all the equipment needed." Auto-advance to Step 2.
- **Hint (15 s):** "Look around the lab table for the reagent bottle of potassium chromate, 10 mL graduated cylinder, and stir rod."
- **Cannot proceed until:** all equipment is collected.

## Step 2 - Measure 1 mL of 0.1 M potassium chromate
- **Station:** 2 MIXING
- **Player instruction:** "Use the graduated cylinder to collect 1 mL of 0.1 M potassium chromate solution. Hold the cylinder under the reagent bottle."
- **Required actions:**
  1. Pick up the graduated cylinder.
  2. Move to the potassium chromate (yellow) reagent bottle.
  3. Position the cylinder under the reagent bottle.
  4. Pour out 1 mL (the screen shows what 1 mL looks like, to use as an approximation).
  5. Pour the cylinder's yellow potassium chromate into **test tube #1**.
  6. Repeat with **test tube #2**.
- **Auto-stop:** the yellow solution stops flowing at exactly 1 mL, with a visual/audio cue. Overfilling is not possible.
- **Success criteria:** cylinder positioned under the bottle; pouring activated into the cylinder; 1 mL reached automatically; tubes #1 and #2 positioned under the cylinder; pouring activated into each; tubes #1 and #2 now each contain 1 mL of yellow potassium chromate.
- **Feedback:** yellow liquid is visible in the cylinder and then in each tube; liquid-flow sound. At 1 mL: chime and "Excellent! 1 mL collected." Auto-advance to Step 3.
- **Hint (5 s):** "Position the graduated cylinder directly under the potassium chromate solution and pull the trigger."
- **Cannot proceed until:** 1 mL of potassium chromate is measured and poured into tubes #1 and #2.

## Step 3 - Drop-wise addition of 1 M sodium hydroxide to test tube #1
- **Station:** 2 MIXING
- **Player instruction:** "Open the reagent bottle that says 1 M sodium hydroxide, use the dropper."
- **Required actions:**
  1. Pick up the 1 M sodium hydroxide bottle (colorless liquid) in one hand.
  2. Pick up the bottle's dropper in the other hand.
  3. Put the sodium hydroxide bottle down.
  4. Position the dropper over test tube #1.
  5. Add a few drops of 1 M sodium hydroxide, drop-wise.
- **Auto-stop:** when the dropper hovers over the tube, the system prompts "push button"; drops dispense in small drop-like increments and are seen going into the tube and mixing with the solution. After **four drops** the system stops automatically; a green checkmark appears. (The document asks whether the solution changes color: "the solution does not change color?" with the question mark.)
- **Success criteria:** dropper over tube #1; drops mixing with the potassium chromate; exactly 4 drops (auto-completes); stirring rod placed in tube #1; stirring motion; rod removed from tube #1 and placed on the counter.
- **Feedback:** on 4 drops, chime and "Perfect! 4 drops have been added." Auto-advance to Step 4. If not: "Please continue to add drops by pushing the button." Visual: test tube #1 holds slightly more than 1 mL.
- **Hint (25 s):** "Hold the dropper over test tube #1. The system will stop you at exactly 4 drops."
- **Cannot proceed until:** four drops of 1 M sodium hydroxide have been added drop-wise with stirring.

## Step 4 - Stir the solution
- **Station:** 2 MIXING
- **Player instruction:** "Use the glass stir rod to mix the solution inside the test tube until the drops completely dissolve."
- **Required actions:**
  1. Pick up the glass stirring rod.
  2. Place it in the test tube; the system prompts "push button".
  3. Stir test tube #1: either a circular hand motion, or hold the trigger/button for an automatic stirring animation (the document says to choose the easier option).
- **Success criteria:** stir rod in the tube; stirring for a minimum of 5 seconds (visual timer completes).
- **Feedback:** the solution moves and mixes while stirring; chime and a stirred message; auto-advance. If not: "Please continue to stir by pushing the button."
- **Hint (25 s):** "Hold the stir bar over test tube #1." (see the inconsistencies below)
- **Cannot proceed until:** 5 seconds of stirring plus the visual transition complete.

## Step 5 - Record your observations
- **Station:** 2 MIXING
- **Question (physical or chemical change?):** two large, easy-to-press buttons, "Physical Change" and "Chemical Change". Incorrect: "Not quite. Think about whether a new substance was created. Try again." The buttons stay active for retry. Cannot proceed until the correct answer is selected.
- **Observation statements:** the student selects, from several sentences, the one that matches what they see in the test tube.
- **Record in the notebook - player instruction:** "Record your observations - go to the lab notebook, pick up pen and write what you see." The document notes that this is a classic pH-change reaction.
- **Required actions:**
  1. Pick up the pen.
  2. Move over to the lab notebook.
  3. Write (automatic or button press). The notebook may have pre-written statements that the student clicks.
- **Auto-transfer:** when the student picks up the pen they hover above a notebook page; they can see test tube #1 and the page, which holds the pre-written statements.
- **Success criteria:** pen picked up; positioned over the notebook page; writing triggered.

## Step 6 - Add 3 M sulfuric acid (H₂SO₄) to test tube #2
- **Station:** 2 MIXING
- **Player instruction:** "Open the reagent bottle that says 3 M sulfuric acid, use the dropper."
- **Do (the document says "repeat all steps above"; spelled out here):**
  1. Pick up the bottle and its dropper, then put the bottle down.
  2. Hold the dropper over **test tube #2** and push the button; it stops automatically after 4 drops.
  3. Pick up the stir rod, place it in test tube #2 and stir for at least 5 seconds (hold the button for auto-stir); put the rod on the counter.
  4. Pick up the pen and record what you see in the notebook (select the matching statement).

## Step 7 - Add 1 M sodium hydroxide to test tube #2
- **Station:** 2 MIXING
- **Player instruction:** "Open the reagent bottle that says 1 M sodium hydroxide, use the dropper."
- **Do (the document says "repeat all steps above"; spelled out here):**
  1. Pick up the bottle and its dropper, then put the bottle down.
  2. Hold the dropper over **test tube #2** and push the button; it stops automatically after 4 drops.
  3. Pick up the stir rod, place it in test tube #2 and stir for at least 5 seconds (hold the button for auto-stir); put the rod on the counter.
  4. Pick up the pen and record what you see in the notebook (select the matching statement).

---

## End of module: waste and answer
- **Waste reminder:** "Dispose of waste in the designated waste containers. Metal ion waste should NEVER be poured down the drain. You are not yet qualified to distinguish when acids and bases may be poured down the drain, so you should always dispose of acid/base waste in the designated containers." Metal-ion waste is disposed of in the **fume hood (station 3)**; the test tubes go in **glass waste (station 4)** (from the Step 0 note).
- **Answer:** Mixing chromate ions (CrO₄²⁻) with sodium hydroxide (NaOH) keeps the chromium in its yellow chromate form. Because NaOH is a base, it prevents the solution from becoming acidic, which stops yellow chromate from turning into orange dichromate (Cr₂O₇²⁻). No major chemical reaction occurs between them.
  - **pH effect:** NaOH adds hydroxide ions (OH⁻), which remove any free hydrogen ions (H⁺).
  - **Equilibrium:** the balance between chromate and dichromate relies on acid (H⁺). Adding a base like NaOH shifts the balance entirely toward the yellow chromate ion.
  - **No precipitation:** sodium chromate (Na₂CrO₄) stays completely dissolved as a clear yellow liquid.
- **Safety and handling:** sodium hydroxide is a strong alkali and causes severe skin burns. Chromate compounds contain hexavalent chromium, which is toxic and harmful to the environment; always wear gloves and eye protection.

---

## Where the document is unclear or inconsistent (needs a decision)
1. **Stir rod location:** the document says it is "inside your lab drawer, below the bench". The scene has no drawer; the user's decision is that the stir rod sits on the standard TUBE bench (station 5).
2. **"All 8 items":** the document never lists the 8. The equipment named is two test tubes, rack, stir rod, squirt bottle, pipette, the three reagent bottles and the graduated cylinder, which is more than 8.
3. **Step 4 stirring:** the success criterion is 5 seconds of stirring, but the feedback and hint say "after stirring 2 times" and "two stirs". The text also mentions a beaker and "sugar fully dissolved", which are left over from the earlier sugar module.
4. **Step 5 wording:** the question text still says "You dissolved sugar in water. The solution is clear and transparent", and the "correct" answer is Physical Change. That is the sugar module's wording and contradicts the chemical-change topic. The correct answer for each tube is not defined ("TBD").
5. **Step 5 note:** "this is a classic pH change reaction. Upon base, the solution should change color" conflicts with the Answer section, where NaOH keeps the solution yellow.
6. **Steps 6 and 7** only say "repeat all steps above". The instructions and the popup text spell this out as the Step 3-5 sequence (4 drops, 5 s stir, record), assuming the same drop count. The expected color for each is not stated in the document. (The Answer section implies acid gives orange and base returns yellow, but does not say so.)
7. **Waste:** there is no numbered waste-disposal step. The waste reminder and the answer come after Step 7.
8. **pH / litmus paper:** it is a learning objective but no step uses it.
9. **Typos:** potassium chromate is written "K₂Cr₂O₄" (should be K₂CrO₄), and dichromate "CrO₇²⁻" (should be Cr₂O₇²⁻).
10. **Missing picture:** the document asks for a picture of the three reagent bottles, still unsupplied.

## Relation to `Chemical_Change_Overview.cs`
The popup text in `text[]` / `webGLText[]` now comes from this document. The script keeps 11 steps plus a completion message, mapped onto the document like this: 1 Background, 2 Safety + PPE, 3 Gather, 4 Measure 1 mL chromate, 5 NaOH in tube #1, 6 Stir, 7 Record, 8 H2SO4 in tube #2, 9 NaOH in tube #2, 10 Waste reminder, 11 Wrap-up (the document's Answer and safety recap). The document has no numbered waste or wrap-up step; those two slots hold its end-of-module waste reminder and Answer. The popups state no expected colors, since the document gives none for the second tube.
