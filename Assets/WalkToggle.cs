using System;
using UnityEngine;
using UnityEngine.UI;
public class WalkToggle : MonoBehaviour
{
    [SerializeField] private Image _image;
    [Space(5)]
    [SerializeField] private Sprite _walkIcon;
    [SerializeField] private Sprite _runIcon;

    private bool _walk;
    public static Action onSwitch;

    public void Switch()
    {
        _walk = !_walk;
        _image.sprite = _walk ? _walkIcon : _runIcon;  
        onSwitch?.Invoke();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Switch();
        }
    }
}
