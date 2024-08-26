using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableHandler : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _button.interactable = false;
    }
    public void Init(Sprite sprite, UnityAction action)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(action);
        _image.sprite = sprite;
    }
    public void SetInteractable(bool state)
    {
        _button.interactable = state;
    }
    public void SetEnable(bool state)
    {
        _canvasGroup.alpha = state ? 1 : 0;
        _canvasGroup.interactable = state;
        _canvasGroup.blocksRaycasts = state; 
    }
}
