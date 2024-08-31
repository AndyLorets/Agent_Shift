using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskable
{
    string taskName { get; set; }
    void InitTask(Task task)
    {
        taskName = task.taskName; 
    }
}
