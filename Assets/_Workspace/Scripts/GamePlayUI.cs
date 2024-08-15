using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class GamePlayUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup; 
    private void OnEnable()
    {
        GameManager.onGameStart += Show;
        GameManager.onGameWin += Hide;
        GameManager.onGameLose += Hide;
    }
    private void OnDisable()
    {
        GameManager.onGameStart -= Show;
        GameManager.onGameWin -= Hide;
        GameManager.onGameLose -= Hide;
    }
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        ServiceLocator.RegisterService(this); 
    }
    private void Start()
    {
        Hide(); 
    }
    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }
    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
