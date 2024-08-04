using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static Action onGameStart;
    public static Action onGameWin;
    public static Action onGameLose; 
    private void OnEnable()
    {
        BriefingManager.onEndBriefing += OnStartGame;
    }
    private void OnDisable()
    {
        BriefingManager.onEndBriefing -= OnStartGame;
    }
    private void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
    }
    private void OnStartGame()
    {
        onGameStart?.Invoke();
    }
    public void Restart()
    {
        ServiceLocator.ClearAllServices();
        CharacterDialogue.speaking = false;
        SceneManager.LoadScene(0);
    }
}
