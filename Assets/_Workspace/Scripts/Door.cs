using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite _interactSprite;
    [SerializeField] private Sprite _playerIcon;
    [SerializeField] private CharacterDialogue _dialogue;
    [SerializeField] private bool _isOpen;
    [SerializeField] private InteractableHandler _interactHandler;

    private Animation _animation;
    private Collider _collider;

    private const string ANIM_OPEN = "Door_Open";
    private const string ANIM_CLOSE = "Door_Close";

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _collider = GetComponent<Collider>();

        _interactHandler.Init(_interactSprite, InputKey);
        _interactHandler.SetEnable(!_isOpen); 
    }
    private void Start()
    {
        if (_isOpen)
            OpenDoor(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_isOpen && other.CompareTag(TagsObj.PLAYER))
            _interactHandler.SetInteractable(true); 
    }
    private void OnTriggerExit(Collider other)
    {
        if (!_isOpen && other.CompareTag(TagsObj.PLAYER))
            _interactHandler.SetInteractable(false);
    }
    private void InputKey()
    {
        foreach (var t in Inventory.items)
        {
            if(t.itemName == "Door Key")
            {
                _isOpen = true;
                _collider.enabled = false; 
                OpenDoor();
                _interactHandler.SetEnable(false);
            }
        }
        if (!_isOpen)
            ServiceLocator.GetService<CharacterMessanger>().SetDialogueMessage(_playerIcon, _dialogue.text, _dialogue.clip);
    }
    private void OpenDoor()
    {
        _animation.PlayQueued(ANIM_OPEN);
        ServiceLocator.GetService<AudioManager>().PlayOpenDoor(); 
    }
}
