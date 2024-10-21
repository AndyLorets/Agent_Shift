using DG.Tweening;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject[] _disableObjs;  
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        PauseManager.onPause += Active;
    }
    private void Start()
    {
        Active(false); 
    }
    private void OnDestroy()
    {
        PauseManager.onPause -= Active;
    }
    private void Active(bool value)
    {
        float endValue = value ? 1 : 0;
        _canvasGroup.DOFade(endValue, .3f).SetUpdate(true);
        _canvasGroup.blocksRaycasts = value;
        _canvasGroup.interactable = value;

        for (int i = 0; i < _disableObjs.Length; i++)
        {
            _disableObjs[i].SetActive(value); 
        }
    }
}
