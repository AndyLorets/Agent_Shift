using UnityEngine.UI;
using UnityEngine;

public abstract class ToggleButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;

    protected abstract bool _activeState { get; set; }

    private void OnEnable()
    {
        SetCurrentSprite();
    }
    public virtual void Toggle()
    {
        _activeState = !_activeState;
        SetCurrentSprite();
    }

    private void SetCurrentSprite()
    {
        _image.sprite = _activeState ? _activeSprite : _inactiveSprite;
    }
}
