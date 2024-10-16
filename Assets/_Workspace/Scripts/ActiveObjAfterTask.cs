using UnityEngine;

public class ActiveObjAfterTask : MonoBehaviour
{
    [SerializeField] private GameObject[] _obj;
    [SerializeField] private string _taskName;

    private TaskManager _taskManager;
    void Start()
    {
        _taskManager = ServiceLocator.GetService<TaskManager>();
        _taskManager.onTaskComplete += TaskComplate;
        for (int i = 0; i < _obj.Length; i++)
        {
            _obj[i].SetActive(false);
        }

    }
    private void TaskComplate(string taskTame)
    {
        if (_taskName == taskTame)
            for (int i = 0; i < _obj.Length; i++)
            {
                _obj[i].SetActive(true);
            }
    }
    private void OnDestroy()
    {
        _taskManager.onTaskComplete -= TaskComplate;
    }
}
