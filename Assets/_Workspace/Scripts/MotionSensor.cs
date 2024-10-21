using System;
using UnityEngine;

public class MotionSensor : MonoBehaviour
{
    public Action<Character> onSensorEnter;
    public Action onSensorExit;
    private ParticleSystem _vfx;
    private ParticleSystem.MainModule _mainModule;
    private TaskManager _taskManager;

    [SerializeField] private string _taskName;

    private void Awake()
    {
        _vfx = transform.GetChild(0).GetComponent<ParticleSystem>();
        _mainModule = _vfx.main;
        _mainModule.startColor = Color.green;
    }
    private void Start()
    {
        _taskManager = ServiceLocator.GetService<TaskManager>();    
        _taskManager.onTaskComplete += TaskComplate;
    }
    private void OnDestroy()
    {
        _taskManager.onTaskComplete -= TaskComplate;
        _taskManager = null;    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        onSensorEnter?.Invoke(other.GetComponent<Character>());
        ChangeParticleColor(Color.red);  
    }

    private void OnTriggerExit(Collider other)
    {
        onSensorExit?.Invoke();
        ChangeParticleColor(Color.green);  
    }

    private void ChangeParticleColor(Color color)
    {
        _mainModule.startColor = color;  
        _vfx.Clear();
        _vfx.Play();   
    }
    private void TaskComplate(string taskTame)
    {
        if (_taskName == taskTame)
        {
            onSensorExit?.Invoke();
            gameObject.SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.8f);
    }
}
