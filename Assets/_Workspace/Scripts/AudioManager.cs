using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _musicSource;
    [Space(5), SerializeField] private AudioClip _musicMenuClip;
    [SerializeField] private AudioClip _musicBriefingClip;
    [SerializeField] private AudioClip _musicGameClip;
    [SerializeField] private AudioClip _musicWinClip;
    [SerializeField] private AudioClip _musicLoseClip;
    [Header("Fx")]
    [SerializeField] private AudioSource _door;
    [SerializeField] private AudioSource _taskWrite;
    [SerializeField] private AudioSource _item;
    [SerializeField] private AudioSource _alert;

    private AudioListener audioListener; 

    private float _startMusicVolume;
    public static bool SoundActiveState
    {
        get => PlayerPrefs.GetInt(nameof(SoundActiveState), 1) == 1;
        set => PlayerPrefs.SetInt(nameof(SoundActiveState), value ? 1 : 0);
    }
    public static bool MusicActiveState
    {
        get => PlayerPrefs.GetInt(nameof(MusicActiveState), 1) == 1;
        set => PlayerPrefs.SetInt(nameof(MusicActiveState), value ? 1 : 0);
    }

    private void Awake()
    {
        ServiceLocator.RegisterService(this);

        BriefingManager.onStartBriefing += PlayBriefingMusic;
        GameManager.onGameWin += PlayWinMusic;
        GameManager.onGameLose += PlayLoseMusic;
        GameManager.onGameStart += PlayGameMusic; 
    }
    private void OnDestroy()
    {
        BriefingManager.onStartBriefing -= PlayBriefingMusic;
        GameManager.onGameWin -= PlayWinMusic;
        GameManager.onGameLose -= PlayLoseMusic;
        GameManager.onGameStart -= PlayGameMusic;
    }
    private void Start()
    {
        _startMusicVolume = _musicSource.volume;
        SetMusicMute();
        SetSoundMute();
    }
    private void PlayMenuMusic() => ChangeMusic(_musicMenuClip);
    private void PlayBriefingMusic() => ChangeMusic(_musicBriefingClip);
    private void PlayGameMusic() => ChangeMusic(_musicGameClip);
    private void PlayWinMusic() => ChangeMusic(_musicWinClip);
    private void PlayLoseMusic() => ChangeMusic(_musicLoseClip);
    private void ChangeMusic(AudioClip clip)
    {
        if (_musicSource.clip == null)
        {
            _musicSource.clip = clip;
            _musicSource.volume = _startMusicVolume;
            _musicSource.Play(); 
            return; 
        }
        _musicSource.DOFade(0, .5f)
            .OnComplete(delegate ()
            {
                _musicSource.clip = clip;
                _musicSource.Play(); 
                _musicSource.DOFade(_startMusicVolume, .15f);
            });
    }

    public void PlayOpenDoor()
    {
        if (!SoundActiveState) return;

        _door.Play(); 
    }
    public void PlayTaskWrite()
    {
        if (!SoundActiveState) return;

        _taskWrite.Play();
    }
    public void PlayItem()
    {
        if (!SoundActiveState) return;

        _item.Play();
    }
    public void PlayAlert()
    {
        if (!SoundActiveState) return;

        if (!_alert.isPlaying)
            _alert.Play(); 
    }
    public void SetMusicMute()
    {
        _musicSource.mute = !MusicActiveState;
    }
    public void SetSoundMute()
    {
        AudioListener.pause = !SoundActiveState;
    }
}
