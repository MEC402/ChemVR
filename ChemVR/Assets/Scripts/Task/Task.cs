using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    private string text;
    private bool isComplete;

    public Task(string text)
    {
        this.text = text;
        isComplete = false;
    }

    public string GetText() { return text; }
    public bool GetComplete() { return isComplete; }
    public void SetComplete(bool com) { isComplete = com; }
}