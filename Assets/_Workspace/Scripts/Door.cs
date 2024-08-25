using UnityEngine;
using UnityEngine.Events; 

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite _interactSprite;
    [SerializeField] private Sprite _playerIcon;
    [SerializeField] private CharacterDialogue _dialogue;
    [SerializeField] private InteractableHandler _interactHandler;
    [Space(5)]
    [SerializeField] private UnlockType _unlockType;
    [SerializeField] private string _doorCode;
    [SerializeField] private KeypadDoorUnlocker _doorUnlocker;

    private KeypadDoorUnlocker _currentDoorUnlocker; 

    private bool _isOpen;
    public enum UnlockType
    {
       None, Key, Code
    }

    private Animation _animation;
    private Collider _collider;

    private const string ANIM_OPEN = "Door_Open";
    private const string ANIM_CLOSE = "Door_Close";

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _collider = GetComponent<Collider>();
    }
    private void Start()
    {
        if (_unlockType == UnlockType.None)
            OpenDoor();
        else
            _interactHandler.Init(_interactSprite, Action());

        _interactHandler.SetEnable(!_isOpen);
    }
    private UnityAction Action()
    {
        switch (_unlockType)
        {
            case UnlockType.Code: return OpenDoorUnlocker;
            default: return InputKey;
        }
    }
    private void InputKey()
    {
        foreach (var t in Inventory.items)
        {
            if(t.itemName == "Door Key")
            {
                _collider.enabled = false; 
                OpenDoor();
                _interactHandler.SetEnable(false);
            }
        }
        if (!_isOpen)
            ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_playerIcon, _dialogue);
    }
    
    private void OpenDoorUnlocker()
    {
        _currentDoorUnlocker = Instantiate(_doorUnlocker);
        _currentDoorUnlocker.Init(_doorCode, OpenDoor);
        ServiceLocator.GetService<UIContentManager>().Open(_currentDoorUnlocker.gameObject); 
    }

    private void OpenDoor()
    {
        _isOpen = true;
        _animation.PlayQueued(ANIM_OPEN);
        ServiceLocator.GetService<AudioManager>().PlayOpenDoor(); 
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
}
