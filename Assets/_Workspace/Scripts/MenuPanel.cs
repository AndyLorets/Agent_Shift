using DG.Tweening;
using TMPro;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _missionText; 
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        BriefingManager.onStartBriefing += Hide;
    }
    private void Start()
    {
        string currentLevel = $"Mission: {ServiceLocator.GetService<GameDataController>().PlayerData.currentLevel}"; 
        _missionText.text = currentLevel;
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
