using UnityEngine;
using DG.Tweening;
public class ArmorEffect : MonoBehaviour
{
    private Transform _effect;

    private const float SCALE_DURATION = .5f; 

    private void Awake()
    {
        _effect = transform.GetChild(0);
        _effect.localScale = Vector3.zero; 
        _effect.gameObject.SetActive(false); 

        PlayerAbilities.onArmor += SetActive;
    }
    private void OnDestroy()
    {
        PlayerAbilities.onArmor -= SetActive;
    }
    private void SetActive(bool value)
    {
        if (value)
        {
            _effect.gameObject.SetActive(true);
            _effect.DOScale(Vector3.one * 2, SCALE_DURATION);
        }
        else
        {
            _effect.DOScale(Vector3.zero, SCALE_DURATION)
                .OnComplete(() => _effect.gameObject.SetActive(false));
        }
    }
}
