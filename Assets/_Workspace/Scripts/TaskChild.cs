using UnityEngine;

public class TaskChild : MonoBehaviour, ITaskable
{
    public string taskName { get; set; }
    public bool activeTask { get; set; }

    public void CompleteTask()
    {
        ServiceLocator.GetService<TaskManager>().CompleteTask(taskName);
    }
    public void ActiveTask()
    {
        activeTask = true; 
    }
    public void DeactiveTask()
    {
        activeTask = false; 
    }
}
