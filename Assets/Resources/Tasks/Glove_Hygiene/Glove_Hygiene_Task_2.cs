using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glove_Hygiene_Task_2 : TaskStep
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
        //point at the beaker
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Beaker"));

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
        if (item.name.ToLower().Contains("beaker"))
        {
            table.Add("beaker");
        }
        else if (item.name.ToLower().Contains("flask"))
        {
            if (item.GetComponent<ChemContainer>() != null)
            {
                string contents = item.GetComponent<ChemContainer>().getContents();
                string[] sepContents = contents.Split('\n');
                foreach(string s in sepContents)
                {
                    if(!s.Contains(": 0") && !s.Contains("TOTAL"))
                    {
                        table.Add(s.Split(':')[0]);
                    }
                }
            }
        }
    }

    private void takeOffTable(HashSet<string> table, GameObject item)
    {
        if (item.name.ToLower().Contains("beaker"))
        {
            table.Remove("beaker");
        }
        else if (item.name.ToLower().Contains("flask"))
        {
            if (item.GetComponent<ChemContainer>() != null)
            {
                string contents = item.GetComponent<ChemContainer>().getContents();
                string[] sepContents = contents.Split('\n');
                foreach (string s in sepContents)
                {
                    if (!s.Contains(": 0") && !s.Contains("TOTAL"))
                    {
                        table.Remove(s.Split(':')[0]);
                    }
                }
            }
        }
    }

    private void tableHasAll(HashSet<string> table)
    {
        if(table.Contains("beaker") && table.Contains("WATER") && table.Contains("HYDROCHLORIC_ACID"))
        {
            FinishTaskStep();
        }

        //If any table has these contents point to the next item needed
        if(table.Contains("beaker"))
        {
            if (table.Contains("WATER"))
            {
                GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Flask (1)"));
            } else
            {
                GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("Flask"));
            }
        }
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onItemOnTable -= itemOnTable;
        GameEventsManager.instance.miscEvents.onItemOffTable -= itemOffTable;
    }
}
