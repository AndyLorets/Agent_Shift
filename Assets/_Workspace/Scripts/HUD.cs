using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class HUD : MonoBehaviour
{
    private CanvasGroup _canvasGroup; 
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        ServiceLocator.RegisterService(this);

        PauseManager.onPause += Deactive;
        GameManager.onGameStart += Show;
        GameManager.onGameWin += Hide;
        GameManager.onGameLose += Hide;
    }
    private void OnDestroy()
    {
        PauseManager.onPause -= Deactive;
        GameManager.onGameStart -= Show;
        GameManager.onGameWin -= Hide;
        GameManager.onGameLose -= Hide;

        _canvasGroup.DOKill();
    }
    private void Start()
    {
        Hide();
    }
    private void Deactive(bool value)
    {
        if (!value) Show();
        else Hide();
    }
    public void Show()
    {
        _canvasGroup.DOFade(1, .3f).SetUpdate(true);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, .3f).SetUpdate(true);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
