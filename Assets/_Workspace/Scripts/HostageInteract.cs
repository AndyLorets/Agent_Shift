using UnityEngine;

public class HostageInteract : MonoBehaviour
{
    [SerializeField] private Sprite _interactSprite; 
    [SerializeField] private Hostage[] _hostages;
    [SerializeField] private InteractableHandler _interactHandler;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _interactHandler.Init(_interactSprite, Released);
    }

    private void Released()
    {
        _boxCollider.enabled = false;

        for (int i = 0; i < _hostages.Length; i++)
        {
            _hostages[i].Released(); 
        }
        _interactHandler.SetEnable(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        _interactHandler.SetInteractable(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        _interactHandler.SetInteractable(false);
    }
}
