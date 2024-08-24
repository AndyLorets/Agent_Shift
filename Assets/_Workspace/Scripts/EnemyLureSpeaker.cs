using System;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class EnemyLureSpeaker : MonoBehaviour
{ 
    [SerializeField] private Sprite _interactSprite; 
    [SerializeField] private Transform _point;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private InteractableHandler _interactHandler;
    [SerializeField] private Enemy[] _enemies;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _interactHandler.Init(_interactSprite, ActiveLure); 
    }
    private void ActiveLure()
    {
        if(_audio.isPlaying)
        {
            DeactiveLure();
            return;
        }

        _audio.Play();
        _effect.Play();

        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].EnterLureState(_point.position); 
        }

        Invoke(nameof(DeactiveLure), _audio.clip.length);
    }
    private void DeactiveLure()
    {
        _audio.Stop();
        _effect.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        _interactHandler.SetInteractable(true); 
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        _interactHandler.SetInteractable(false);
    }
}
