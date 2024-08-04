using System;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class EnemyLureSpeaker : MonoBehaviour
{
    [SerializeField] private Sprite _interactSprite; 
    [SerializeField] private Transform _point;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private InteractableHandler _interactHandler;

    public Action<Vector3> onLure;
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _interactHandler.Init(_interactSprite, ActiveLure); 
    }

    private void ActiveLure()
    {
        onLure?.Invoke(_point.position);
        _audio.Play();
        _effect.Play();
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
