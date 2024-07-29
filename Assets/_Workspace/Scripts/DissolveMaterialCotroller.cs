using DG.Tweening;
using UnityEngine;

public class DissolveMaterialCotroller : MonoBehaviour
{
    private Material _material;
    private float _dissolveValue;
    private float _startDissolveValue;

    private const string DissolveProperty = "_DissolveValue";

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            _material = renderer.material;
            _material = Instantiate(_material);
            renderer.material = _material;
        }
        else
        {
            Debug.LogError("Renderer not found on the object.");
        }

        // Подписка на событие
        PlayerAbilities.onChangeInvisibility += ChangeDissolveValue;
    }

    private void Start()
    {
        if (_material != null)
        {
            _startDissolveValue = _material.GetFloat(DissolveProperty);
            _dissolveValue = _startDissolveValue;
        }
    }

    private void OnDestroy()
    {
        PlayerAbilities.onChangeInvisibility -= ChangeDissolveValue;
    }

    private void ChangeDissolveValue(bool value)
    {
        float targetValue = value ? 0.7f : _startDissolveValue;
        StartDissolve(targetValue, 0.7f);
    }

    public void StartDissolve(float targetValue, float duration)
    {
        DOTween.To(() => _dissolveValue, x => _dissolveValue = x, targetValue, duration)
            .OnUpdate(() => _material.SetFloat(DissolveProperty, _dissolveValue)); 
    }
}
