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
    private void Render(UnityAction action, bool state)
    {
        _interaction.SetActive(state);
        _button.onClick.RemoveAllListeners();
        if(state)
            _button.onClick.AddListener(action);
    }
}
public static class InteractionManager
{
    public static Action<UnityAction, bool> onPlayerInteraction;
    public static void Interact(UnityAction action, bool state)
    {
        onPlayerInteraction?.Invoke(action, state); 
    }
}
