using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class Computer : MonoBehaviour, ITaskable
{
    [SerializeField] private Sprite _interactSprite;
    [SerializeField] private InteractableHandler _interactHandler;
    [Space(5)]
    [SerializeField] private PuzzleWires _puzzleWires;
    [Space(5)]
    [SerializeField] private Sprite _icon;
    [SerializeField] private CharacterDialogue _dialogue;

    private PuzzleWires _currentPuzzleWires;
    public string taskName { get; set; }

    void Start()
    {
        _interactHandler.Init(_interactSprite, StartAction);
    }
    private void StartAction()
    {
        _currentPuzzleWires = Instantiate(_puzzleWires);
        _currentPuzzleWires.onComplete += EndAction; 
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_icon, _dialogue, 5);
        ServiceLocator.GetService<UIContentManager>().Open(_currentPuzzleWires.gameObject);
    }
    private void EndAction()
    {
        ServiceLocator.GetService<TaskManager>().CompleteTask(taskName);
        _currentPuzzleWires.onComplete -= EndAction;
        _interactHandler.SetEnable(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsObj.PLAYER))
            _interactHandler.SetInteractable(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagsObj.PLAYER))
            _interactHandler.SetInteractable(false);
    }
}
