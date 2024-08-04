using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _musicSource;
    [Space(5), SerializeField] private AudioClip _musicGameClip;
    [SerializeField] private AudioClip _musicWinClip;
    [SerializeField] private AudioClip _musicLoseClip; 
    [Header("Fx")]
    [SerializeField] private AudioSource _door;
    [SerializeField] private AudioSource _taskWrite;

    public static AudioManager Instance { get; private set; }

    private float _startMusicVolume; 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); 

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
    public void PlayOpenDoor()
    {
        _door.Play(); 
    }
    public void PlayTaskWrite()
    {
        _taskWrite.Play();
    }
}
