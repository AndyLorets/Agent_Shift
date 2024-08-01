using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonArmor : MonoBehaviour
{
    [SerializeField] private Image _image;
    private Button _button;

    public Action<Image, Button> onClick;
    private void Awake()
    {
        _button = GetComponent<Button>();

        PlayerAbilities.onChangeArmorTime += ChangeImageValue;
    }
    private void OnDestroy()
    {
        PlayerAbilities.onChangeArmorTime -= ChangeImageValue;
    }
    private void ChangeImageValue(float value)
    {
        _button.interactable = value == 1; 
        _image.DOFillAmount(value, 1)
            .SetEase(Ease.Linear); 
    }
}
