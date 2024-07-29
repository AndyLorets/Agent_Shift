using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonArmor : MonoBehaviour
{
    private Image _image;
    private Button _button;

    public Action<Image, Button> onClick;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        PlayerAbilities.onChangeArmor += ChangeButtonValue;
        PlayerAbilities.onChangeArmorTime += ChangeImageValue;
    }
    private void OnDestroy()
    {
        PlayerAbilities.onChangeArmor -= ChangeButtonValue;
        PlayerAbilities.onChangeArmorTime -= ChangeImageValue;
    }
    private void ChangeButtonValue(bool value)
    {
        _button.interactable = !value;
    }

    private void ChangeImageValue(float value)
    {
        _image.DOFillAmount(value, 1).SetEase(Ease.Linear);

    }
}
