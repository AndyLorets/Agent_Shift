using UnityEngine;
using TMPro; 
public class Note : MonoBehaviour
{
    [SerializeField] private UINoteContent _noteContent;
    [Space(5)]
    [SerializeField] private TextMeshPro _text;
    [SerializeField, TextArea(4, 4)] private string _value;
    [Space(5)]
    [SerializeField] private Sprite _interactSprite;
    [SerializeField] private InteractableHandler _interactHandler;
    [Space(5)]
    [SerializeField] private Sprite _icon;
    [SerializeField] private CharacterDialogue _dialogue;

    private UINoteContent _currenUIContent;

    private void Start()
    {
        _interactHandler.Init(_interactSprite, Action);
        _text.text = _value; 
    }
    private void Action()
    {
        _currenUIContent = Instantiate(_noteContent);
        _currenUIContent.Init(_value);
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_icon, _dialogue, 5);
        ServiceLocator.GetService<UIContentManager>().Open(_currenUIContent.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsObj.PLAYER))
        {
            _interactHandler.SetInteractable(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagsObj.PLAYER))
            _interactHandler.SetInteractable(false);
    }
}
