using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    private TextMesh text;
    private List<Task> tasks;
    private int curTask;

    
    void Start()
    {
        tasks = new List<Task>();
        curTask = 0;

        Task zero = new Task("Welcome to the VR\nChemistry lab. Press B\nto continue to the next step");
        tasks.Add(zero);
        Task one = new Task("To toggle the info-page press\nthe left thumbstick.Try disabling\nthen enabling the info-page.");
        tasks.Add(one);
        Task two = new Task("Time to get to work. Use the two\nanalog sticks to move and look\naround. Go to the glove box and\nput on some gloves.");
        tasks.Add(two);
        Task three = new Task("Lets being the first expirement.");
        StartCoroutine(TaskZero());
    }
    // Update is called once per frame
    private IEnumerator TaskZero()
    {
        text.text = tasks[curTask].GetText();
        while (tasks[curTask].GetComplete() == false)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.B))
            {
                tasks[curTask].SetComplete(true);
            }
            yield return null;
        }
        curTask++;
        StartCoroutine(TaskOne());
    }
    private IEnumerator TaskOne()
    {
        text.text = tasks[curTask].GetText();
        int counter = 0;
        while(tasks[curTask].GetComplete() == false)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch))
            {
                counter++;
            }
            if(counter == 2)
            {
                tasks[curTask].SetComplete(true);
            }
            yield return null;
        }
        curTask++;
        StartCoroutine(TaskTwo());
    }
    private IEnumerator TaskTwo()
    {
        text.text = tasks[curTask].GetText();
        while (tasks[curTask].GetComplete() == false)
        {
            
            yield return null;
        }

        yield return null;
    }
}
