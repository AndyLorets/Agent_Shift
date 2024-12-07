public interface ITaskable
{
    public bool activeTask { get; set; }
    string taskName { get; set; }

    void InitTask(Task task)
    {
        taskName = task.taskName; 
    }
    abstract void ActiveTask();
    abstract void DeactiveTask();
}