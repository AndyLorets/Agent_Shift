using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _tasksText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [Space(5)]
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _loseColor;
    [SerializeField] private Button _nextBtn;

    private TextMeshProUGUI _nextText; 

    private CanvasGroup _group;

    private void Awake()
    {
        GameManager.onGameLose += OnLoseGame;
        GameManager.onGameWin += OnWinGame;

        _group = GetComponent<CanvasGroup>();
        _nextText = _nextBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>(); 
    }
    private void OnDestroy()
    {
        GameManager.onGameLose -= OnLoseGame;
        GameManager.onGameWin -= OnWinGame;
        _group.DOKill();
    }
    private void Start()
    {
        _group.alpha = 0;
        _group.blocksRaycasts = false;
        _group.interactable = false;   
    }
    private void OnWinGame()
    {
        _titleText.color = _winColor; 
        _titleText.text = "Mission Complate";
        _moneyText.text = ServiceLocator.GetService<Wallet>().CurrentMoney.ToString(); 

        Invoke(nameof(ShowUI), 2f);
        RenderTasks();
    }
    private void OnLoseGame()
    {
        _nextBtn.interactable = false;
        _nextText.color = GetColorFromHex.GetColor("#6C6C6C");
        _titleText.color = _loseColor;
        _titleText.text = "Mission Failed";
        Invoke(nameof(ShowUI), 2f);
        RenderTasks(); 
    }
    private void ShowUI()
    {
        _group.DOFade(1, 1);
        _group.blocksRaycasts = true;
        _group.interactable = true;
    }
    private void RenderTasks()
    {
        List<Task> tasks = ServiceLocator.GetService<TaskManager>().TasksList;
        _tasksText.text = ""; 
        foreach (Task task in tasks) 
        {
            _tasksText.text += task.taskName + "\n"; 
        }
    }
}
