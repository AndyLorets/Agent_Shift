using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> _tasks = new List<Task>();

    public List<Task> TasksList => _tasks;
    private int _currentTask;

    public Action<string> onTaskUpdate;

    private void Awake()
    {
        ServiceLocator.RegisterService(this);   
        GameManager.onGameStart += ActiveTask; 
    }

    void Start()
    {
        for (int i = 0; i < _tasks.Count; i++)
        {
            _tasks[i].Init(); 
        }
    }
    private void ActiveTask()
    {
        onTaskUpdate?.Invoke(_tasks[_currentTask].taskName); 
    }
    public void CompleteTask(string taskName)
    {
        Task task = _tasks.Find(t => t.taskName == taskName);
        if (task != null)
        {
            _currentTask++;
            if (_currentTask < _tasks.Count)
            {
                task.complate = true;
                onTaskUpdate?.Invoke(_tasks[_currentTask].taskName);
            }
            else
                ServiceLocator.GetService<GameManager>().WinGame(); 
        }       
    }
    private void OnDestroy()
    {
        GameManager.onGameStart -= ActiveTask;
    }
}
