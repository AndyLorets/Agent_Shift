using UnityEngine.UI;
using UnityEngine;
using System;
using DG.Tweening;

public class ButtonInvisibility : MonoBehaviour
{
    [SerializeField] private Image _btnFillAmount;
    [SerializeField] private Image _icon;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        PlayerAbilities.onChangeInvisibilitTime += HandleChangeInvisibilityTime;
        PlayerAbilities.onInvisibility += HandleInvisibility;
    }

    private void OnDestroy()
    {
        PlayerAbilities.onChangeInvisibilitTime -= HandleChangeInvisibilityTime;
        PlayerAbilities.onInvisibility -= HandleInvisibility;
    }

    private void HandleInvisibility(bool isActive)
    {
        SetButtonInteractable(isActive);
        AnimateButtonFillAmount(isActive);
    }

    private void HandleChangeInvisibilityTime(float fillAmount)
    {
        AnimateIconFillAmount(fillAmount);
    }

    private void AnimateIconFillAmount(float fillAmount, float duration = 1f)
    {
        _icon.DOFillAmount(fillAmount, duration)
             .SetEase(Ease.Linear);
    }

    private void AnimateButtonFillAmount(bool isActive)
    {
        float endValue = isActive ? 0f : 1f;
        float duration = isActive ? 0.3f : 5f;

        _btnFillAmount.DOFillAmount(endValue, duration)
                      .SetEase(Ease.Linear)
                      .OnComplete(() =>
                      {
                          if (!isActive)
                          {
                              AnimateIconFillAmount(1f, 0.1f);
                              SetButtonInteractable(true);
                          }
                      });
    }

    private void SetButtonInteractable(bool isActive)
    {
        _button.interactable = isActive;
    }
}
