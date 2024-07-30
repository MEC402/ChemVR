using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glove_Hygiene_Task_2_5 : TaskStep
{
    HashSet<string> redTable1 = new HashSet<string>();
    HashSet<string> redTable2 = new HashSet<string>();
    HashSet<string> redTable3 = new HashSet<string>();
    HashSet<string> redTable4 = new HashSet<string>();
    HashSet<HashSet<string>> redTables = new HashSet<HashSet<string>>();

    protected override void SetTaskStepState(string state)
    {
        // Not needed
    }
    private void Update()
    {
        foreach(HashSet<string> table in redTables)
        {
            tableHasAll(table);
        }
    }
    void OnEnable()
    {
        //point at the burette
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Burette"));

        redTables.Add(redTable1);
        redTables.Add(redTable2);
        redTables.Add(redTable3);
        redTables.Add(redTable4);

        GameEventsManager.instance.miscEvents.onItemOnTable += itemOnTable;
        GameEventsManager.instance.miscEvents.onItemOffTable += itemOffTable;
    }

    private void itemOnTable(GameObject item, GameObject table)
    {
        if(table.name.Contains("C"))
        {
            if(table.name.Contains("1"))
            {
                putOnTable(redTable1, item);
            } else if (table.name.Contains("2"))
            {
                putOnTable(redTable2, item);
            }
            else if (table.name.Contains("3"))
            {
                putOnTable(redTable3, item);
            }
            else if (table.name.Contains("4"))
            {
                putOnTable(redTable4, item);
            }
        }
    }

    private void itemOffTable(GameObject item, GameObject table)
    {
        if (table.name.Contains("C"))
        {
            if (table.name.Contains("1"))
            {
                takeOffTable(redTable1, item);
            }
            else if (table.name.Contains("2"))
            {
                takeOffTable(redTable2, item);
            }
            else if (table.name.Contains("3"))
            {
                takeOffTable(redTable3, item);
            }
            else if (table.name.Contains("4"))
            {
                takeOffTable(redTable4, item);
            }
        }
    }

    private void putOnTable(HashSet<string> table, GameObject item)
    {
        if (item.name.ToLower().Contains("rette"))
        {
            table.Add("burette");
        }
        else if (item.name.ToLower().Contains("unnel"))
        {
            table.Add("funnel");
        }
    }

    private void takeOffTable(HashSet<string> table, GameObject item)
    {
        if (item.name.ToLower().Contains("rette"))
        {
            table.Remove("burette");
        }
        else if (item.name.ToLower().Contains("unnel"))
        {
            table.Remove("funnel");
        }
    }

    private void tableHasAll(HashSet<string> table)
    {
        if(table.Contains("burette") && table.Contains("funnel"))
        {
            FinishTaskStep();
        }

        //If any table has these contents point to the next item needed
        if (!table.Contains("burette") && !table.Contains("funnel"))
        {
            GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Burette"));
        }

        if (table.Contains("burette"))
        {
            GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Funnel"));
        }
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onItemOnTable -= itemOnTable;
        GameEventsManager.instance.miscEvents.onItemOffTable -= itemOffTable;
    }
}
