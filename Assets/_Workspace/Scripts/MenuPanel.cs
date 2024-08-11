using DG.Tweening;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        BriefingManager.onStartBriefing += Hide;
    }
    private void OnDestroy()
    {
        BriefingManager.onStartBriefing -= Hide;
    }
    private void Hide()
    {
        _canvasGroup.DOFade(0, .3f).SetUpdate(true);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}
