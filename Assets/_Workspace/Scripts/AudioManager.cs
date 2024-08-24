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
    [SerializeField] private AudioSource _headShot;
    [SerializeField] private AudioSource _money;
    [SerializeField] private AudioSource _kick;
    [SerializeField] private AudioSource _abilityOn;
    [SerializeField] private AudioSource _abilityOff;

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
    public void PlayHeadShot()
    {
        if (!SoundActiveState) return;

        if (!_headShot.isPlaying)
            _headShot.Play();
        else
            _headShot.PlayOneShot(_headShot.clip);
    }
    public void PlayMoney()
    {
        if (!SoundActiveState) return;

        if (!_money.isPlaying)
            _money.Play();
        else
        {
            _money.Stop();
            _money.Play();
        }
    }
    public void PlayKick()
    {
        if (!SoundActiveState) return;

        if (!_kick.isPlaying)
            _kick.Play();
        else
        {
            _kick.Stop();
            _kick.Play();
        }
    }
    public void PlayAbilityOn()
    {
        if (!SoundActiveState) return;

        if (!_abilityOn.isPlaying)
            _abilityOn.Play();
        else
            _abilityOn.PlayOneShot(_abilityOn.clip);       
    }
    public void PlayAbilityOff()
    {
        if (!SoundActiveState) return;

        if (!_abilityOff.isPlaying)
            _abilityOff.Play();
        else
            _abilityOff.PlayOneShot(_abilityOff.clip);
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
