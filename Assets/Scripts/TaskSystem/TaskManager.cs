using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool loadTaskState = false; 

    private Dictionary<string, Task> taskMap;

    // task start requirements
    private int currentPlayerExperiment; // TODO: Figure out what this is for

    private void Awake()
    {
        taskMap = CreateTaskMap();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.taskEvents.onStartTask += StartTask;
        GameEventsManager.instance.taskEvents.onAdvanceTask += AdvanceTask;
        GameEventsManager.instance.taskEvents.onStartTask += AbandonTask;
        GameEventsManager.instance.taskEvents.onFinishTask += FinishTask;
        GameEventsManager.instance.taskEvents.onTaskStepStateChange += TaskStepStateChange;

        GameEventsManager.instance.playerEvents.onPlayerExperimentChange += PlayerExperimentChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.taskEvents.onStartTask -= StartTask;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= AdvanceTask;
        GameEventsManager.instance.taskEvents.onStartTask -= AbandonTask;
        GameEventsManager.instance.taskEvents.onFinishTask -= FinishTask;
        GameEventsManager.instance.taskEvents.onTaskStepStateChange -= TaskStepStateChange;

        GameEventsManager.instance.playerEvents.onPlayerExperimentChange -= PlayerExperimentChange;
    }

    private void Start()
    {
        
        foreach (Task task in taskMap.Values)
        {
            if (loadTaskState)
            {
                // initialize any loaded task steps
                if (task.state == TaskState.IN_PROGRESS)
                {
                    task.InstantiateCurrentTaskStep(this.transform);
                }
            }
            // broadcast the initial state of all quests on startup
            GameEventsManager.instance.taskEvents.TaskStateChange(task);
            
        }
    }

    private void ChangeTaskState(string id, TaskState state)
    {
        Task task = GetTaskById(id);
        task.state = state;
        GameEventsManager.instance.taskEvents.TaskStateChange(task);
    }

    private void PlayerExperimentChange(int experimentNumber)
    {
        currentPlayerExperiment = experimentNumber;
    }

    private bool CheckRequirementsMet(Task task)
    {
        // start true and prove to be false
        bool meetsRequirements = true;

        // check player experiment requirements
        if (currentPlayerExperiment != task.info.experimentRequirement)
        {
            meetsRequirements = false;
        }

        // check task prerequisites for completion
        foreach (TaskInfoSO prerequisiteTaskInfo in task.info.taskPrerequisites)
        {
            if (GetTaskById(prerequisiteTaskInfo.id).state != TaskState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        // loop through all tasks
        foreach (Task task in taskMap.Values)
        {
            // if we now meet the requirements, switch to the CAN_START state
            if (task.state == TaskState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(task))
            {
                ChangeTaskState(task.info.id, TaskState.CAN_START);
            }
        }
    }

    private void StartTask(string id)
    {
        Task task = GetTaskById(id);
        if (!loadTaskState)
        {
            task.StartOver();
        }
        task.InstantiateCurrentTaskStep(this.transform);
        ChangeTaskState(task.info.id, TaskState.IN_PROGRESS);
    }

    private void AdvanceTask(string id)
    {
        Task task = GetTaskById(id);

        // move on to the next step
        task.MoveToNextStep();

        if (task.CurrentStepExists())
        {
            // if there are more steps, instantiate the next one
            task.InstantiateCurrentTaskStep(this.transform);
        }
        else
        {
            // if no more steps, then we've finished all of them for this task
            ChangeTaskState(task.info.id, TaskState.CAN_FINISH);
        }
    }

    private void AbandonTask(string id)
    {
        Task task = GetTaskById(id);
        task.StartOver();
        ChangeTaskState(task.info.id, TaskState.CAN_START);
    }

    private void FinishTask(string id)
    {
        Task task = GetTaskById(id);
        ClaimRewards(task);
        ChangeTaskState(task.info.id, TaskState.FINISHED);
    }

    private void ClaimRewards(Task task)
    {
        GameEventsManager.instance.playerEvents.PointsGained(task.info.pointsReward);
    }

    private void TaskStepStateChange(string id, int stepIndex, TaskStepState taskStepState)
    {
        Task task = GetTaskById(id);
        task.StoreTaskStepState(taskStepState, stepIndex);
        ChangeTaskState(id, task.state);
    }

    private Dictionary<string, Task> CreateTaskMap()
    {
        // loads all TaskInfoSO Scriptable Objects under the Assets/Resources/Tasks folder
        TaskInfoSO[] allTasks = Resources.LoadAll<TaskInfoSO>("Tasks");
        // create the task map
        Dictionary<string, Task> idToTaskMap = new Dictionary<string, Task>();
        foreach (TaskInfoSO taskInfo in allTasks)
        {
            if (idToTaskMap.ContainsKey(taskInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating task map: " + taskInfo.id);
            }
            idToTaskMap.Add(taskInfo.id, LoadTask(taskInfo));
        }
        return idToTaskMap;
    }

    private Task GetTaskById(string id)
    {
        Task task = taskMap[id];
        if (task == null)
        {
            Debug.LogError("ID not found in the Task Map: " + id);
        }
        return task;
    }

    private void OnApplicationQuit()
    {
        foreach (Task task in taskMap.Values)
        {
            SaveTask(task);
        }
    }

    private void SaveTask(Task task)
    {
        try
        {
            TaskData taskData = task.GetTaskData();
            // serialize using JsonUtility, but we can use something else if desired
            string serializedData = JsonUtility.ToJson(taskData);
            // saving to PlayerPrefs is a temporary thing just to demonstrate the concept.
            // if we want a real save and load system we'll have to write to a file or something
            PlayerPrefs.SetString(task.info.id, serializedData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save task with id " + task.info.id + ": " + e);
        }
    }

    private Task LoadTask(TaskInfoSO taskInfo)
    {
        Task task = null;
        try
        {
            if (PlayerPrefs.HasKey(taskInfo.id) && loadTaskState)
            {
                // load task from saved data
                string serializedData = PlayerPrefs.GetString(taskInfo.id);
                TaskData taskData = JsonUtility.FromJson<TaskData>(serializedData);
                task = new Task(taskInfo, taskData.state, taskData.taskStepIndex, taskData.taskStepStates);
            }
            else
            {
                // otherwise, initialize a new task
                task = new Task(taskInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load task with id " + task.info.id + ": " + e);
        }
        return task;
    }
}
