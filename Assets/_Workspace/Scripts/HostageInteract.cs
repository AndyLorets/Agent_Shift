using UnityEngine;

public class HostageInteract : MonoBehaviour
{
    [SerializeField] private Sprite _interactSprite; 
    [SerializeField] private Hostage[] _hostages;

    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Released()
    {
        _boxCollider.enabled = false;

        for (int i = 0; i < _hostages.Length; i++)
        {
            _hostages[i].Released(); 
        }
        InteractionManager.Interact(Released, false, _interactSprite);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        InteractionManager.Interact(Released, true, _interactSprite);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;

        InteractionManager.Interact(Released, false, _interactSprite);
    }
}
