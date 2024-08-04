using Cinemachine;
using System;
using UnityEngine;

public class BriefingManager : MonoBehaviour
{
    [SerializeField] private Briefing[] _briefings;
    [SerializeField] private Briefing _loseBriefings;
    [SerializeField] private Briefing _winBriefings;

    public static Action onEndBriefing;
    [SerializeField] private int _currentBriefing;

    private void Awake()
    {
        CharacterMessanger.OnResetAudioPlaying += PlayBriefing;
        GameManager.onGameWin += OnGameWin;
        GameManager.onGameLose += OnGameLose;
    }
    private void Start()
    {
        Invoke(nameof(PlayBriefing), 1f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkipBriefing(); 
        }
    }
    private void PlayBriefing()
    {
        if (_currentBriefing < _briefings.Length)
        {
            _briefings[_currentBriefing].PlayBriefing();
        }
        _currentBriefing++;
        EndBriefing();
    }
    private void SkipBriefing()
    {
        ServiceLocator.GetService<CharacterMessanger>().Skip();
        _briefings[_currentBriefing - 1].StopBriefing();
        _currentBriefing = _briefings.Length + 1;
        EndBriefing();
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
    [SerializeField] private string _text;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private CinemachineVirtualCamera _cam;

    public void PlayBriefing()
    {
        CharacterMessanger.OnResetAudioPlaying += StopBriefing;
        ServiceLocator.GetService<CharacterMessanger>().SetDialogueMessage(_icon, _text, _audioClip);
        _cam.Priority = 11;
    }
    public void StopBriefing()
    {
        CharacterMessanger.OnResetAudioPlaying -= StopBriefing;
        _cam.Priority = 0;
        _cam.gameObject.SetActive(false);
    }
}

