using UnityEngine.UI;
using UnityEngine;
using System;
using DG.Tweening;

public class ButtonInvisibility : MonoBehaviour
{
    [SerializeField] private Image _image;
    private Button _button;

    public Action<Image, Button> onClick;
    private void Awake()
    {
        _button = GetComponent<Button>();   

        PlayerAbilities.onChangeInvisibility += ChangeButtonValue;
        PlayerAbilities.onChangeInvisibilitTime += ChangeImageValue; 
    }
    private void OnDestroy()
    {
        PlayerAbilities.onChangeInvisibility -= ChangeButtonValue;
        PlayerAbilities.onChangeInvisibilitTime -= ChangeImageValue;
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
