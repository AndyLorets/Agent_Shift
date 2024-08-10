using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BriefingManager : MonoBehaviour
{
    [SerializeField] private Briefing[] _briefings;
    [SerializeField] private Briefing _loseBriefings;
    [SerializeField] private Briefing _winBriefings;
    [Space(5)]
    [SerializeField] private Button _skipBtn; 

    private int _currentBriefing;

    public static Action onStartBriefing;
    public static Action onEndBriefing;

    private void Awake()
    {
        CharacterMessanger.OnResetAudioPlaying += PlayBriefing;
        GameManager.onGameWin += OnGameWin;
        GameManager.onGameLose += OnGameLose;

        _skipBtn.onClick.AddListener(SkipBriefing);
        _skipBtn.gameObject.SetActive(false);
    }
    private void Start()
    {
        Invoke(nameof(PlayBriefing), 1f);
    }
    private void PlayBriefing()
    {
        if (_currentBriefing < _briefings.Length)
        {
            _briefings[_currentBriefing].PlayBriefing();
        }
        _currentBriefing++;
        EndBriefing();

        if (_currentBriefing == 1)
            _skipBtn.gameObject.SetActive(true);
    }
    private void SkipBriefing()
    {
        ServiceLocator.GetService<CharacterMessanger>().Skip();
        _briefings[_currentBriefing - 1].StopBriefing();
        _currentBriefing = _briefings.Length + 1;
        EndBriefing();
        _skipBtn.gameObject.SetActive(false);
    }
    private void EndBriefing()
    {
        if (_currentBriefing == _briefings.Length + 1)
        {
            onEndBriefing?.Invoke();
            CharacterMessanger.OnResetAudioPlaying -= PlayBriefing;
        }
    }
    private void OnDestroy()
    {
        CharacterMessanger.OnResetAudioPlaying -= PlayBriefing;
        GameManager.onGameWin -= OnGameWin;
        GameManager.onGameLose -= OnGameLose;
    }
    private void OnGameLose()
    {
        _loseBriefings.PlayBriefing();
    }
    private void OnGameWin()
    {
        _winBriefings.PlayBriefing();
    }

}

[System.Serializable]
public class Briefing
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private CharacterDialogue _dialogue;

    public void PlayBriefing()
    {
        CharacterMessanger.OnResetAudioPlaying += StopBriefing;
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_icon, _dialogue);
        _cam.Priority = 11;
    }
    public void StopBriefing()
    {
        CharacterMessanger.OnResetAudioPlaying -= StopBriefing;
        _cam.Priority = 0;
        _cam.gameObject.SetActive(false);
    }
}

