using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _musicSource;
    [Space(5), SerializeField] private AudioClip _musicGameClip;
    [SerializeField] private AudioClip _musicWinClip;
    [SerializeField] private AudioClip _musicLoseClip;

    private float _startMusicVolume; 
    private void Awake()
    {
        GameManager.onGameStart += OnGameStart;
        GameManager.onGameWin += OnGameWin;
        GameManager.onGameLose += OnGameLose;
    }
    private void OnDestroy()
    {
        GameManager.onGameStart -= OnGameStart;
        GameManager.onGameWin -= OnGameWin;
        GameManager.onGameLose -= OnGameLose;
    }
    private void Start()
    {
        _startMusicVolume = _musicSource.volume; 
    }
    private void OnGameStart()
    {
        _musicSource.DOFade(0, .5f)
            .OnComplete(delegate ()
            {
                _musicSource.Stop();
                _musicSource.volume = _startMusicVolume;
                _musicSource.PlayOneShot(_musicGameClip);
            });
    }
    private void OnGameWin()
    {
        _musicSource.DOFade(0, .5f)
            .OnComplete(delegate ()
            {
                _musicSource.Stop();
                _musicSource.volume = _startMusicVolume;
                _musicSource.PlayOneShot(_musicWinClip);
            });
    }
    private void OnGameLose()
    {
        _musicSource.DOFade(0, .5f)
            .OnComplete(delegate ()
            {
                _musicSource.Stop();
                _musicSource.volume = _startMusicVolume; 
                _musicSource.PlayOneShot(_musicLoseClip);
            });       
    }
}
