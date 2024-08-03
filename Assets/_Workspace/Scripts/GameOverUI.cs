using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _tasksText;
    [Space(5)]
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _loseColor;

    private CanvasGroup _group;

    private void Awake()
    {
        GameManager.onGameLose += OnLoseGame;
        GameManager.onGameWin += OnWinGame;

        _group = GetComponent<CanvasGroup>();   
    }
    private void OnDestroy()
    {
        GameManager.onGameLose -= OnLoseGame;
        GameManager.onGameWin -= OnWinGame;
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
        Invoke(nameof(ShowUI), 2f);
        RenderTasks();
    }
    private void OnLoseGame()
    {
        _titleText.color = _loseColor;
        _titleText.text = "Mission Filed";
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
        List<Task> tasks = TaskManager.Instance.TasksList;
        _tasksText.text = ""; 
        foreach (Task task in tasks) 
        {
            _tasksText.text += task.taskName + "\n"; 
        }
    }
}