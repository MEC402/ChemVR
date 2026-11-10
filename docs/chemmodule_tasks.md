# Module 3 (Physical & Chemical Changes) - Task Instructions

**Source of truth:** `docs/VR Chemistry Module 3 - Physical and Chemical Changes UPDATED.docx` (updated 10 Aug 2026 with the USC lab coordinator). The player is walked through the exercise **one task at a time**: each task is a single goal, and a page of the document that lists several separate things becomes several tasks, not one panel. Closely linked small actions (pick up the bottle and its dropper, put the bottle down) stay together as one task so the player is not stepping through every grab.

There are **33 tasks**. Each is one popup in `Chemical_Change_Overview.cs` (`text[]` / `webGLText[]`) and one prefab `M3_Tnn_<name>` in `Resources/Tasks/Chemical_Change/Prefabs/`, listed in order in `Chemical_Change_Task.asset`. Until a task has its working piece it is a click-through placeholder (**A** in VR, **N** in WebGL).

The module mimics Reaction #1 of Lab Exercise #3, "The Law of Mass Action: Le Chatelier's Principle" (CHEM 105B manual, p. 16): the chromate-dichromate equilibrium. Estimated duration 12-15 minutes. **Linear step-gating:** each task must be completed before the next one starts. Design philosophy: minimal fine motor skill (auto-stopping measurements, button-based controls, forgiving interactions).

**Station labels** (the numbered signs in the scene; the document itself does not name stations, so this mapping is the scene's):

| Station | Name | Used for |
|---|---|---|
| 1 | PPE | Background and PPE |
| 2 | MIXING | Working bench: squirt bottle, pipette, Sharpie, pen, notebook, graduated cylinder; all mixing |
| 3 | HOOD | Reagent bottles in the Acids / Bases trays; chemical waste |
| 4 | SINK | Glass waste |
| 5 | TUBE | Test tubes, rack, stir rod |

---


## Step 0: Background

1. **Background: reaction #1** (station 1 PPE): In this experiment, 9 reactions at equilibrium will be perturbed, and the resulting chemical and physical changes will be observed. A physical change occurs when matter changes form. A chemical change occurs when new products are formed. For this simulation, you will do reaction #1 from Lab 3: The Law of Mass Action: Le Chatelier's Principle.
2. **Background: Le Chatelier's Principle** (station 1 PPE): Le Chatelier's Principle allows us to predict the effect of change upon a chemical system in equilibrium. It says that if stress is imposed on a system at equilibrium, the equilibrium concentrations of reactants and products will shift in such a manner as to relieve the stress.

## Step 0: Safety first - put on PPE

3. **Safety precaution** (station 1 PPE): SAFETY PRECAUTION: Several of the reactions in this experiment use acids and bases at concentrations of 1 M or more. Be extremely careful when using high concentrations of corrosive chemicals such as acids and bases. All spills or drips should be cleaned up immediately as directed by your TA. Be particularly careful not to leave drops of acid or base on the sides of the bottles.
4. **Put on goggles and lab coat** (station 1 PPE): Before starting any chemistry experiment, you must wear proper Personal Protective Equipment (PPE). At the PPE station, press the goggles button and then the lab coat button. Each item equips automatically.
5. **Put on gloves** (station 1 PPE): Put on your gloves. Your hands turn blue to show that you are wearing gloves.
6. **Before you start: lab notes** (station 1 PPE): Goggles protect your eyes from splashes; lab coats protect your skin and clothing. In this lab you will generate metal ion waste, disposed of in the fume hood, and glass waste (the test tubes). This is a qualitative experiment: measurements do not have to be exact, detailed observations of color, precipitation and temperature changes are what matter most.

## Step 1: Gather equipment

7. **Set up the test tubes** (station 5 TUBE): Gather your equipment. At the TUBE station, put the test tube rack in your work area, place two test tubes inside it, and pick up the glass stir rod.
8. **Estimate 1 mL** (station 2 MIXING): This lab uses approximately 1 mL volumes, which do not have to be exact. Take the squirt bottle of water and a disposable pipette from the bench and measure about 1 mL of water into one test tube to learn the amount. Also place the 10 mL graduated cylinder in your work area.
9. **Label the test tubes** (station 2 MIXING): Label one empty test tube #1 and the other #2 with the black Sharpie.
10. **Collect the reagents** (station 3 HOOD): Go to the fume hood. The reagents are stored in two trays labeled acids and bases, and each bottle has a disposable pipette taped to it. Bring the potassium chromate (K2CrO4), sodium hydroxide (NaOH) and sulfuric acid (H2SO4) bottles to your work area.

## Step 2: Measure 1 mL of 0.1 M potassium chromate

11. **Measure 1 mL of chromate** (station 2 MIXING): Pick up the graduated cylinder and hold it under the potassium chromate reagent bottle. Pour: the yellow solution stops flowing automatically at exactly 1 mL.
12. **Pour into test tube #1** (station 2 MIXING): Pour the yellow potassium chromate from the cylinder into test tube #1.
13. **Repeat for test tube #2** (station 2 MIXING): Measure another 1 mL of potassium chromate in the cylinder and pour it into test tube #2.

## Steps 3-5: Add 1 M sodium hydroxide to test tube #1, stir, record

14. **NaOH #1: get the bottle and dropper** (station 2 MIXING): Open the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.
15. **NaOH #1: add 4 drops** (station 2 MIXING): Hold the dropper over test tube #1 and push the button to add drops, drop by drop. The system stops automatically after four drops.
16. **NaOH #1: stir** (station 2 MIXING): Place the glass stir rod in test tube #1 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.
17. **NaOH #1: physical or chemical?** (station 2 MIXING): Is this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.
18. **NaOH #1: record the observation** (station 2 MIXING): Select the sentence that matches what you see in test tube #1, then pick up the pen and write it in the lab notebook.

## Step 6: Add 3 M sulfuric acid to test tube #2 (the document says to repeat steps 3-5)

19. **H2SO4 #2: get the bottle and dropper** (station 2 MIXING): Open the reagent bottle that says 3 M sulfuric acid: pick it up in one hand and its dropper in the other, then put the bottle down.
20. **H2SO4 #2: add 4 drops** (station 2 MIXING): Hold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.
21. **H2SO4 #2: stir** (station 2 MIXING): Place the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.
22. **H2SO4 #2: physical or chemical?** (station 2 MIXING): Is this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.
23. **H2SO4 #2: record the observation** (station 2 MIXING): Select the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.

## Step 7: Add 1 M sodium hydroxide to test tube #2 (the document says to repeat steps 3-5)

24. **NaOH #2: get the bottle and dropper** (station 2 MIXING): Open the reagent bottle that says 1 M sodium hydroxide: pick it up in one hand and its dropper in the other, then put the bottle down.
25. **NaOH #2: add 4 drops** (station 2 MIXING): Hold the dropper over test tube #2 and push the button to add drops, drop by drop. The system stops automatically after four drops.
26. **NaOH #2: stir** (station 2 MIXING): Place the glass stir rod in test tube #2 and stir for at least 5 seconds (move the rod in a circle, or hold the button for automatic stirring). Then take the rod out and put it on the counter.
27. **NaOH #2: physical or chemical?** (station 2 MIXING): Is this a physical change or a chemical change? Pick one of the two buttons. If it is not right, think about whether a new substance was created and try again.
28. **NaOH #2: record the observation** (station 2 MIXING): Select the sentence that matches what you see in test tube #2, then pick up the pen and write it in the lab notebook.

## Waste disposal

29. **Waste reminder** (station 3 HOOD): WASTE REMINDER: Dispose of waste in the designated waste containers. Metal ion waste should NEVER be poured down the drain. You are not yet qualified to distinguish when acids and bases may be poured down the drain, so you should always dispose of acid and base waste in the designated containers.
30. **Empty the test tubes** (station 3 HOOD): Take test tubes #1 and #2 to the fume hood and empty both into the chemical waste container.
31. **Test tubes to glass waste** (station 4 SINK): Put both empty test tubes in the glass waste.

## Answer

32. **Answer: what happened** (station 2 MIXING): Mixing chromate ions (CrO4 2-) with sodium hydroxide (NaOH) keeps the chromium in its yellow chromate form. Because NaOH is a base, it prevents the solution from becoming acidic, which stops yellow chromate from turning into orange dichromate (Cr2O7 2-). NaOH adds hydroxide ions (OH-), which remove free hydrogen ions (H+), so the equilibrium shifts toward chromate. No precipitate forms; the solution stays a clear yellow liquid.
33. **Safety and handling** (station 2 MIXING): Sodium hydroxide is a strong alkali and causes severe skin burns. Chromate compounds contain hexavalent chromium, which is toxic and harmful to the environment. Always wear gloves and eye protection.

---

## How the document's steps map onto tasks

| Document step | Tasks |
|---|---|
| Step 0: Background | 1-2 |
| Step 0: Safety first - put on PPE | 3-6 |
| Step 1: Gather equipment | 7-10 |
| Step 2: Measure 1 mL of 0.1 M potassium chromate | 11-13 |
| Steps 3-5: Add 1 M sodium hydroxide to test tube #1, stir, record | 14-18 |
| Step 6: Add 3 M sulfuric acid to test tube #2 (the document says to repeat steps 3-5) | 19-23 |
| Step 7: Add 1 M sodium hydroxide to test tube #2 (the document says to repeat steps 3-5) | 24-28 |
| Waste disposal | 29-31 |
| Answer | 32-33 |

## Where the document is unclear or inconsistent (needs a decision)
1. **Stir rod location (task 7):** the document says it is "inside your lab drawer, below the bench". The scene has no drawer; the stir rod sits on the TUBE bench (station 5).
2. **"All 8 items":** the document never lists the 8. Task 7-10 together collect the rack, two tubes, stir rod, squirt bottle, pipette, graduated cylinder and three reagent bottles, which is more than 8.
3. **Stirring (tasks 16, 21, 26):** the success criterion is 5 seconds of stirring, but the feedback and hint say "after stirring 2 times" and "two stirs". The text also mentions a beaker and "sugar fully dissolved", left over from the earlier sugar module. The tasks use 5 seconds.
4. **Physical or chemical question (tasks 17, 22, 27):** the document's question text still says "You dissolved sugar in water. The solution is clear and transparent", with Physical Change as the correct answer. That is the sugar module's wording; the correct answer for each tube is not defined ("TBD"). The tasks use a neutral question.
5. **Observation statements (tasks 18, 23, 28):** the document says the student picks from several pre-written sentences, but does not supply them. The note "this is a classic pH change reaction. Upon base, the solution should change color" conflicts with the Answer section, where NaOH keeps the solution yellow.
6. **Steps 6 and 7 (tasks 19-28)** only say "repeat all steps above". The tasks repeat the step 3-5 sequence with tube #2 (4 drops, 5 s stir, record), assuming the same drop count. The expected color for each is not stated in the document.
7. **Waste (tasks 29-31):** the document has only the waste reminder, "metal ion waste ... disposed of in the fume hood" and "test tubes are thrown out in glass waste". Emptying the tubes and putting them in glass waste are derived from those lines; the document gives no explicit actions.
8. **Reagent trays (task 10):** the document says the trays are sorted by acid/base but does not say which tray holds potassium chromate.
9. **pH / litmus paper:** it is a learning objective but no step uses it.
10. **Typos in the document:** potassium chromate is written "K2Cr2O4" (should be K2CrO4), and dichromate "CrO7 2-" (should be Cr2O7 2-). The tasks use the correct formulas.
11. **Missing picture:** the document asks for a picture of the three reagent bottles, still unsupplied.

## Keeping things in sync
Adding, removing or reordering a task means changing three things together: this document, `text[]` and `webGLText[]` in `Chemical_Change_Overview.cs` (exactly one more entry than the number of tasks, for the completion message), and `taskStepPrefabs` in `Chemical_Change_Task.asset`. Do not put `_0` in a task prefab name: `StartTasks.RemoveUnstarted()` destroys children whose name contains it.

