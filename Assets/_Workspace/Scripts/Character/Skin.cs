using System;
using UnityEngine;

public class Skin : MonoBehaviour
{
    [SerializeField] private Animator _animator; 
    public Animator animator => _animator;
    public Action onShoot;
    public void Shoot()
    {
        onShoot?.Invoke();
    }
}
