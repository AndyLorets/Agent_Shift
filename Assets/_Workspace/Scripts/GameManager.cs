using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _menuCam;
    [SerializeField] private CinemachineVirtualCamera _gamePlayCam;
    [Space(5), Header("Dialogue")]
    [SerializeField] private Sprite _icon; 
    [SerializeField] private CharacterDialogue[] _winDialogue;
    [SerializeField] private CharacterDialogue[] _loseDialogue;

    public static System.Action onGameStart;
    public static System.Action onGameWin;
    public static System.Action onGameLose;

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

        int r = Random.Range(0, _winDialogue.Length);
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_icon, _winDialogue[r], 5);
        ServiceLocator.GetService<Wallet>().AddMoney(100);
        ServiceLocator.GetService<Player>().CanControll = false; 

        onGameWin?.Invoke(); 
    }
    public void LoseGame()
    {
        gameState = GameState.End;

        int r = Random.Range(0, _loseDialogue.Length);
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_icon, _loseDialogue[r], 5);
        ServiceLocator.GetService<Player>().CanControll = false;

        onGameLose?.Invoke();
    }
    public void NextLevel()
    {
        PlayerData playerData = ServiceLocator.GetService<GameDataController>().PlayerData;
        if (playerData.currentLevel < 3)
            playerData.currentLevel++;
        else
            playerData.currentLevel = 1;

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
