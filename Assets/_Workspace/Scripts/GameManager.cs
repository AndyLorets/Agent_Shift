using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static Action onGameStart;
    public static Action onGameWin;
    public static Action onGameLose;

    [SerializeField] private CinemachineVirtualCamera _menuCam;
    [SerializeField] private CinemachineVirtualCamera _gamePlayCam;
    public enum GameState
    {
        Briefing, GamePlay, End
    }
    public static GameState gameState { get; private set; }

    private void OnEnable()
    {
        onGameWin += OnEndGame;
        onGameLose += OnEndGame;
        BriefingManager.onEndBriefing += OnStartGame;
    }
    private void OnDisable()
    {
        onGameWin -= OnEndGame;
        onGameLose -= OnEndGame;
        BriefingManager.onEndBriefing -= OnStartGame;
    }
    private void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
        gameState = GameState.Briefing;
    }
    public void StartBriefing()
    {
        ServiceLocator.GetService<BriefingManager>().StartBriefing();
        _menuCam.Priority = 0;
    }
    private void OnStartGame()
    {
        gameState = GameState.GamePlay;
        onGameStart?.Invoke();
        _gamePlayCam.Priority = 10;
    }
    private void OnEndGame()
    {
        gameState = GameState.End;
    }
    public void Restart()
    {
        ServiceLocator.GetService<PauseManager>().Pause(false);
        ServiceLocator.ClearAllServices();
        gameState = GameState.Briefing;
        CharacterDialogue.speaking = false;
        SceneManager.LoadScene(1);
    }
}
