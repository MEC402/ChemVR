using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Put_On_Table : TaskStep
{    protected override void SetTaskStepState(string state)
    {
        // Not needed
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("kitchen_table_1"));

        GameEventsManager.instance.miscEvents.onItemOnTable += itemOnTable;
    }

    private void itemOnTable(GameObject item, GameObject table)
    {
        string tableName = table.name.ToLower();
        if (tableName.Contains("kitchen") && tableName.Contains("1"))
        {
            if(item.name.ToLower().Contains("beaker"))
            {
                FinishTaskStep();
            }
        }
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onItemOnTable -= itemOnTable;
    }
}
