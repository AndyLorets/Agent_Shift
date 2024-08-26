using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class Computer : MonoBehaviour, ITaskable
{
    [SerializeField] private Sprite _interactSprite;
    [SerializeField] private InteractableHandler _interactHandler;
    [Space(5)]
    [SerializeField] private GameObject _loading;
    [SerializeField] private Image _loadingFillAmount;
    [SerializeField, Range(5, 20)] private float _loadingDuration = 5f;

    public string taskName { get; set; }

    void Start()
    {
        _interactHandler.Init(_interactSprite, StartAction);
        _loading.SetActive(false);
        _loadingFillAmount.fillAmount = 0; 
    }
    private void StartAction()
    {
        _interactHandler.SetEnable(false);
        _loading.SetActive(true); 
        _loadingFillAmount.DOFillAmount(1, _loadingDuration)
            .OnComplete(() => EndAction());

        ServiceLocator.GetService<AudioManager>().PlayItem();
    }
    private void EndAction()
    {
        ServiceLocator.GetService<TaskManager>().CompleteTask(taskName);
        _loading.SetActive(false);
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
