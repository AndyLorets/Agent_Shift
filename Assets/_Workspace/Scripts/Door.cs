using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite _interactSprite;
    [SerializeField] private Sprite _playerIcon;
    [SerializeField] private CharacterDialogue _dialogue;
    [SerializeField] private bool isOpen = false;

    private Animation _animation;
    private Collider _collider;

    private const string ANIM_OPEN = "Door_Open";
    private const string ANIM_CLOSE = "Door_Close";
    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;
        if (!isOpen)
            InteractionManager.Interact(InputKey, true, _interactSprite);
        else OpenDoor(); 
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(TagsObj.PLAYER)) return;
        if (!isOpen)
            InteractionManager.Interact(InputKey, false, _interactSprite);
        else
            CloseDoor();
    }

    private void InputKey()
    {
        foreach (var t in Inventory.items)
        {
            if(t.itemName == "Door Key")
            {
                isOpen = true;
                OpenDoor(); 
                InteractionManager.Interact(InputKey, false, _interactSprite);
            }
        }
        if (!isOpen)
            CharacterMessanger.instance.SetDialogueMessage(_playerIcon, _dialogue.text, _dialogue.clip);
    }
    private void OpenDoor()
    {
        _animation.Play(ANIM_OPEN);
        AudioManager.Instance.PlayOpenDoor(); 
    }
    private void CloseDoor()
    {
        _animation.Play(ANIM_CLOSE);
        AudioManager.Instance.PlayOpenDoor();
    }
}
