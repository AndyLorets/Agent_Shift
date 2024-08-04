using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableHandler : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    public void Init(Sprite sprite, UnityAction action)
    {
        _button.onClick.AddListener(action);
        _button.interactable = false; 
        _image.sprite = sprite;
    }
    public void SetInteractable(bool state)
    {
        _button.interactable = state;
    }
    public void SetEnable(bool state)
    {
        _button.gameObject.SetActive(state);
    }
}
