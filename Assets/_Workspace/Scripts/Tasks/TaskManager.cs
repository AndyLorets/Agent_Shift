using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> _tasks = new List<Task>();
    [SerializeField] private GameObject _minimapPoint;
    public List<Task> TasksList => _tasks;
    private int _currentTask;

    public Action<string> onTaskUpdate;
    public Action<string> onTaskComplete;

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
        if (_tasks.Count == 0) return; 
        
        onTaskUpdate?.Invoke(_tasks[_currentTask].taskName);
        SetMinimapPointPosition();
    }
    private void SetMinimapPointPosition()
    {
        float x = _tasks[_currentTask].taskableSource.transform.position.x;
        float z = _tasks[_currentTask].taskableSource.transform.position.z;
        _minimapPoint.transform.position = new Vector3(x, 1, z);
    }
    public void CompleteTask(string taskName)
    {
        Task task = _tasks.Find(t => t.taskName == taskName);
        if (task != null)
        {
            onTaskComplete?.Invoke(taskName);
            _currentTask++;
            if (_currentTask < _tasks.Count)
            {
                task.complate = true;
                onTaskUpdate?.Invoke(_tasks[_currentTask].taskName);
                SetMinimapPointPosition();
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
