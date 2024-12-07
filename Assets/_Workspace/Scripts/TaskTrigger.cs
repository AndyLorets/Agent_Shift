using UnityEngine;

public class TaskTrigger : MonoBehaviour, ITaskable
{
    [SerializeField] private TagType _tagType;
    public string taskName { get; set; }
    public bool activeTask { get; set; }

    private enum TagType
    {
        Player, Enemy, Hostage
    }
    private string GetTagName()
    {
        switch (_tagType)
        {
            case TagType.Player:
                return TagsObj.PLAYER;
            case TagType.Enemy:
                return TagsObj.ENEMY;
            case TagType.Hostage:
                return TagsObj.HOSTAGE;
        }
        return TagsObj.PLAYER;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(GetTagName()) || !activeTask) return;

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
