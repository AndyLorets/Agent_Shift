using UnityEngine;

public class TaskTrigger : MonoBehaviour, ITaskable
{
    [SerializeField] private TagType _tagType;
    public string taskName { get; set; }

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
        if (!other.CompareTag(GetTagName())) return;

        TaskManager.Instance.CompleteTask(taskName); 
    }
}
