using System;
using UnityEngine;

[Serializable]
public class Task
{
    public string taskName;
    public bool complate; 
    public GameObject taskableSource; 
    public ITaskable taskable;
    public void Init()
    {
        taskable = taskableSource.GetComponent<ITaskable>();

        if (taskable != null)
            taskable.InitTask(this);
        else
            Debug.LogWarning("taskable == null");
    }
}
