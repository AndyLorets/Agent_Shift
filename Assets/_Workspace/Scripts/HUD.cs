using DG.Tweening;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private CanvasGroup _canvasGroup; 
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        PauseManager.onPause += Active; 
    }
    private void OnDestroy()
    {
        PauseManager.onPause -= Active;
        _canvasGroup.DOKill();
    }
    private void Active(bool value)
    {
        float endValue = !value ? 1 : 0; 
        _canvasGroup.DOFade(endValue, .3f).SetUpdate(true);
        _canvasGroup.blocksRaycasts = !value;
        _canvasGroup.interactable = !value;
    }
   
}
