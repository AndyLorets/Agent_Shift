using DG.Tweening; 
using UnityEngine;

public class Anxiety : MonoBehaviour
{
    private Light _light;
    private AudioSource _audioSource; 
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _light = transform.GetChild(0).GetComponent<Light>();
    }
    private void OnEnable()
    {
        _audioSource.Play();
        Active();
    }
    private void Active()
    {
        float value = _light.intensity < 2 ? 2 : 0; 
        _light.DOIntensity(value, 1).OnComplete(() => Active());
    }
}
