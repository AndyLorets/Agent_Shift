using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableHandler : MonoBehaviour
{
    [SerializeField] private GameObject _interaction;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    private void Awake()
    {
        InteractionManager.onPlayerInteraction += Render;
        _interaction.SetActive(false); 
    }
    private void OnDestroy()
    {
        InteractionManager.onPlayerInteraction -= Render;
    }
    private void Render(UnityAction action, bool state, Sprite sprite)
    {
        _interaction.SetActive(state);
        _image.sprite = sprite; 
        _button.onClick.RemoveAllListeners();
        if(state)
            _button.onClick.AddListener(action);
    }
}
public static class InteractionManager
{
    public static Action<UnityAction, bool, Sprite> onPlayerInteraction;
    public static void Interact(UnityAction action, bool state, Sprite sprite)
    {
        onPlayerInteraction?.Invoke(action, state, sprite); 
    }
}
