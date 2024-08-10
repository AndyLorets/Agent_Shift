using UnityEngine.UI;
using UnityEngine;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _deactieSprite; 

    private bool _active = true;
    private void Start()
    {
        SetCurrentSprite(); 
    }
    public void Toggle()
    {
        _active = !_active;
        SetCurrentSprite(); 
    }

    private void SetCurrentSprite()
    {
        _image.sprite = _active ? _activeSprite : _deactieSprite;
    }
}
