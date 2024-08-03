using Cinemachine;
using System;
using UnityEngine;

public class BriefingManager : MonoBehaviour
{
    [SerializeField] private Briefing[] _briefings;
    public static Action onEndBriefing;
    [SerializeField] private int _currentBriefing; 

    private void Awake()
    {
        CharacterMessanger.OnResetAudioPlaying += PlayBriefing; 
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
    }
    private void EndBriefing()
    {
        if (_currentBriefing != _briefings.Length + 1) return;

        onEndBriefing?.Invoke(); 
        gameObject.SetActive(false);
        CharacterMessanger.OnResetAudioPlaying -= PlayBriefing;
    }
    private void OnDestroy()
    {
        CharacterMessanger.OnResetAudioPlaying -= PlayBriefing;
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
        CharacterMessanger.instance.SetDialogueMessage(_icon, _text, _audioClip);
        _cam.Priority = 11; 
    }
    private void StopBriefing()
    {
        CharacterMessanger.OnResetAudioPlaying -= StopBriefing;
        _cam.Priority = 0;
    }
}

