using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static Action onGameStart;
    public static Action onGameWin;
    public static Action onGameLose;

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
    private void OnStartGame()
    {
        gameState = GameState.GamePlay;
        onGameStart?.Invoke();
    }
    private void OnEndGame()
    {
        gameState = GameState.End;
    }
    public void Restart()
    {
        gameState = GameState.Briefing;
        ServiceLocator.ClearAllServices();
        CharacterDialogue.speaking = false;
        SceneManager.LoadScene(0);
    }
}
