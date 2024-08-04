using System;
using UnityEngine;

public class Skin : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _footStepSource;
    public Animator animator => _animator;
    public Action onShoot;
    public Action onFootStep;
    public void Shoot()
    {
        onShoot?.Invoke();
    }
    public void FootStep()
    {
        onFootStep?.Invoke();
        _footStepSource.Play(); 
    }
}
