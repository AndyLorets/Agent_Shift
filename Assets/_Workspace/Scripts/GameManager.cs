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
        BriefingManager.onEndBriefing += OnStartGame;
    }
    private void OnDisable()
    {
        BriefingManager.onEndBriefing -= OnStartGame;
    }
    private void Awake()
    {
        ServiceLocator.RegisterService(this); 
    }
    private void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
        gameState = GameState.Briefing;
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.CapsLock))
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
#endif
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
    public void WinGame()
    {
        gameState = GameState.End;
        ServiceLocator.GetService<Wallet>().AddMoney(100); 
        onGameWin?.Invoke(); 
    }
    public void LoseGame()
    {
        gameState = GameState.End;
        onGameLose?.Invoke();
    }
    public void NextLevel()
    {
        ServiceLocator.GetService<GameDataController>().PlayerData.currentLevel++;
        ServiceLocator.GetService<GameDataController>().SaveData();
        Restart();
    }
    public void Restart()
    {
        gameState = GameState.Briefing;

        CharacterDialogue.speaking = false;
        ServiceLocator.GetService<PauseManager>().Pause(false);
        ServiceLocator.GetService<SceneLoader>().LoadScene(ServiceLocator.GetService<GameDataController>().PlayerData.currentLevel);
    }
}
