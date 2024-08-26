using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    [SerializeField] private string _taskNameToActive;
    [SerializeField] private Sprite _icon;
    [SerializeField] private CharacterDialogue _dialogue;

    private const float FLY_OFFSET = 30; 
    private AudioSource _audioSource;
    private Animator _animator;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        ServiceLocator.GetService<TaskManager>().onTaskComplete += TaskComplate;
        _animator.enabled = false;
        _audioSource.Stop(); 
        transform.position += Vector3.up * FLY_OFFSET; 
    }
    private void OnDestroy()
    {
        ServiceLocator.GetService<TaskManager>().onTaskComplete -= TaskComplate;
    }
    private void TaskComplate(string taskName)
    {
        if (_taskNameToActive == taskName)
            Active();
    }
    private void Active()
    {
        _audioSource.Play();
        _animator.enabled = true; 
        Vector3 endPos = transform.position += Vector3.down * FLY_OFFSET;
        transform.DOMove(endPos, 10f);
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_icon, _dialogue, 5);
    }
}
