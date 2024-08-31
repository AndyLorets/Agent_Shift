using UnityEngine;

public class TaskChild : MonoBehaviour, ITaskable
{
    public string taskName { get; set; }
    public void CompleteTask()
    {
        ServiceLocator.GetService<TaskManager>().CompleteTask(taskName);
    }
}
