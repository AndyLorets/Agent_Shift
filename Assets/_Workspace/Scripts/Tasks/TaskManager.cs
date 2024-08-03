using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance; 
    public List<Task> tasks = new List<Task>();

    private int _currentTask;

    public Action<string> onTaskUpdate;
    public Action onTasksComplate;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); 
    }

    void Start()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Init(); 
        }
        ActiveTask();
    }
    private void ActiveTask()
    {
        onTaskUpdate?.Invoke(tasks[_currentTask].taskName); 
    }
    public void CompleteTask(string taskName)
    {
        Task task = tasks.Find(t => t.taskName == taskName);
        if (task != null)
        {
            _currentTask++;
            if (_currentTask < tasks.Count)
            {
                onTaskUpdate?.Invoke(tasks[_currentTask].taskName);
            }
            else
                onTasksComplate?.Invoke(); 
        }       
    }

}
