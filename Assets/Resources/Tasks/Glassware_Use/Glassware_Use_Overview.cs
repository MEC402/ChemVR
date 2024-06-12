using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glassware_Use_Overview : MonoBehaviour
{
    public TextMeshProUGUI guText;
    int curStep;

    void OnEnable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        curStep = 0;
        guText.text = "Not reading events.";
        if (GameEventsManager.instance == null)
        {
            guText.text = "There is no game events manager";
        }
        GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task0Complete;
        GameEventsManager.instance.taskEvents.onAbandonTask += gu_abandonMe;
    }

    void OnDisable()
    {
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
        //Remove any listeners
        disableAll();
    }
    public void restart()
    {
        gu_abandonMe("Glassware_Use_Overview");
        this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
    }

    void disableAll()
    {
        if (curStep == 0)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task0Complete;
        }
        else if (curStep == 1)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task1Complete;
        }
        else if (curStep == 2)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task2Complete;
        }
        else if (curStep == 3)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task3Complete;
        }
        else if (curStep == 4)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task4Complete;
        }
        else if (curStep == 5)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task5Complete;
        }
        else if (curStep == 6)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task6Complete;
        }
        else if (curStep == 7)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task7Complete;
        }
        else if (curStep == 8)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task8Complete;
        }
        else if (curStep == 9)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task9Complete;
        }
        else if (curStep == 10)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task10Complete;
        }
        else if (curStep == 11)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task11Complete;
        }
        else if (curStep == 12)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task12Complete;
        }
        else if (curStep == 13)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task13Complete;
        }
        else if (curStep == 14)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task14Complete;
        }
        else if (curStep == 15)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task15Complete;
        }
        else if (curStep == 16)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task16Complete;
        }
        else if (curStep == 17)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task17Complete;
        }
        else if (curStep == 18)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task18Complete;
        }
        else if (curStep == 19)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task19Complete;
        }
        else if (curStep == 20)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task20Complete;
        }
        else if (curStep == 21)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task21Complete;
        }
        else if (curStep == 22)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task22Complete;
        }
        else if (curStep == 23)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task23Complete;
        }
        else if (curStep == 24)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task24Complete;
        }
        else if (curStep == 25)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task25Complete;
        }
        else if (curStep == 26)
        {
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task26Complete;
        }
    }

    public void gu_abandonMe(string context)
    {
        if (context.Contains("Glass"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = false;
            GameObject guPop = GameObject.Find("Glassware Use PopUp");
            guText.text = "Don't look at me I'm inactive.";
            if (guPop != null)
            {
                //Debug.LogWarning("Disabling: " + context);
                guPop.SetActive(false);
                disableAll();
                GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task0Complete;
            }
        }
    }

    void gu_task0Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            this.gameObject.GetComponent<ToggleTextSimple>().enabled = true;
            curStep += 1;
            guText.text = "Welcome!\nBefore we begin the lab, you need to put on your lab gear.\n\nYou should be wearing closed toed shoes and have your hair tied back.\n\nFind gloves, goggles, and a lab coat and put them on with (B).";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task1Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task0Complete;
        }
    }

    void gu_task1Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "For this lab, you will need to bring the required materials to a table in the blue zone:\nNote: Inspect all glassware for cracks or chips.\n\nStart with:\n3 250mL Volumetric Flasks.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task2Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task1Complete;
        }
    }

    void gu_task2Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Next bring:\n1 100mL Graduated Cylinder\n1 Glass Stir Rod\n1 10mL Volumetric Pipette\n to the table.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task3Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task2Complete;
        }
    }

    void gu_task3Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Finally you will need:\nSmall Weigh Boat With Paper\nCopper Sulfate\nPaper Towels\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task4Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task3Complete;
        }
    }

    void gu_task4Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Locate these tools that you will need in the lab:\nAnalytical Balance\nBalance Brush\nDI water\nWhen you are ready to continue, press (A).\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task5Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task4Complete;
        }
    }

    void gu_task5Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Turn on the analytical balance, and allow for it to stabilize.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task6Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task5Complete;
        }
    }

    void gu_task6Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Place a weigh boat on the scale, then put the weigh paper inside the boat.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task7Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task6Complete;
        }
    }

    void gu_task7Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Set the scale units to grams.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task8Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task7Complete;
        }
    }

    void gu_task8Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Press the tare button on the scale to reset the weight to zero.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task9Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task8Complete;
        }
    }

    void gu_task9Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Use the scoopula to transfer 1.500g copper sulfate to the boat.\n\nTransfer small amounts at a time to avoid transferring too much.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task10Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task9Complete;
        }
    }

    void gu_task10Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Remember to close the copper sulfate jar.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task11Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task10Complete;
        }
    }

    void gu_task11Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Pour the contents of the weigh paper into a 250ml flask.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task12Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task11Complete;
        }
    }

    void gu_task12Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Clean the balance with a brush, and turn it off.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task13Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task12Complete;
        }
    }

    void gu_task13Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Bring a graduated cylinder over to the DI water.\n\nFill it until the meniscus is at 50mL.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task14Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task13Complete;
        }
    }

    void gu_task14Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Bring the copper sulfate and DI water back to your hood, and then open the hood and lower it to a working height.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task15Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task14Complete;
        }
    }

    void gu_task15Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Set up a volumetric pipette with a bulb top.\n\nDraw up water to more than 50mL\nPush excess water out until 50mL\nTouch excess drip to side of the glass\nWipe the tip with folded paper towel.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task16Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task15Complete;
        }
    }
    void gu_task16Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Mix copper sulfate and water with the glass stir rod and write down any chemical changes in your notebook.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task17Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task16Complete;
        }
    }
    void gu_task17Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Line up 3 volumetric flasks.\n\nFill each flask with 15mL of the blue fluid using the volumetric pipette.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task18Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task17Complete;
        }
    }
    void gu_task18Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Fill flasks with 25mL, 35mL, 45mL water respectively and observe and record changes.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task19Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task18Complete;
        }
    }
    void gu_task19Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Time to clean up! Pre-rinse your glasses with tap water.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task20Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task19Complete;
        }
    }
    void gu_task20Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Wash your glassware with soapy water to remove any residues.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task21Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task20Complete;
        }
    }
    void gu_task21Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Scrub the inside of the glassware with a brush.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task22Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task21Complete;
        }
    }
    void gu_task22Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Rinse the glassware with the DI bottle.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task23Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task22Complete;
        }
    }
    void gu_task23Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Invert the glassware to allow excess water to drain.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task24Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task23Complete;
        }
    }
    void gu_task24Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Return materials to their original places, and close the glass doors.\n\nThis will protect them from dust and contaminants.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task25Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task24Complete;
        }
    }
    void gu_task25Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "Go to the trash to remove your gloves, and then wash your hands in the sink with soap.\n\n\nSkip with A";
            GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task26Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task25Complete;
        }
    }
    void gu_task26Complete(string context)
    {
        if (context.Contains("Glassware_Use_Task"))
        {
            curStep += 1;
            guText.text = "This concludes the glassware use module. Thank you for your time.\n(X) Hides Popup\n(Y) Exits Module";
            //GameEventsManager.instance.taskEvents.onAdvanceTask += gu_task27Complete;
            GameEventsManager.instance.taskEvents.onAdvanceTask -= gu_task26Complete;
        }
    }
}
