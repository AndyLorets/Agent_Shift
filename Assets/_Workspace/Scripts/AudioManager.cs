using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _musicSource;
    [Space(5), SerializeField] private AudioClip _musicGameClip;
    [SerializeField] private AudioClip _musicAlertClip;
    [SerializeField] private AudioClip _musicWinClip;
    [SerializeField] private AudioClip _musicLoseClip; 
    [Header("Fx")]
    [SerializeField] private AudioSource _door;
    [SerializeField] private AudioSource _taskWrite;
    [SerializeField] private AudioSource _item;
    [SerializeField] private AudioSource _alert;

    private EnemyManager _enemyManager;
    private bool _isAlert;
    private float _startMusicVolume; 
    private void Awake()
    {
        ServiceLocator.RegisterService(this);

        GameManager.onGameStart += PlayIdleMusic;
        GameManager.onGameWin += PlayWinMusic;
        GameManager.onGameLose += PlayLoseMusic;
        GameManager.onGameStart += StartCheckAlertState; 
    }
    private void OnDestroy()
    {
        GameManager.onGameStart -= PlayIdleMusic;
        GameManager.onGameWin -= PlayWinMusic;
        GameManager.onGameLose -= PlayLoseMusic;
        GameManager.onGameStart -= StartCheckAlertState;
    }
    private void Start()
    {
        _enemyManager = ServiceLocator.GetService<EnemyManager>();
        _startMusicVolume = _musicSource.volume; 
    }
    private void StartCheckAlertState() => StartCoroutine(CheckAlertState());
    private void PlayIdleMusic() => ChangeMusic(_musicGameClip);
    private void PlayWinMusic() => ChangeMusic(_musicWinClip);
    private void PlayLoseMusic() => ChangeMusic(_musicLoseClip);
    private void ChangeMusic(AudioClip clip)
    {
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
        _door.Play(); 
    }
    public void PlayTaskWrite()
    {
        _taskWrite.Play();
    }
    public void PlayItem()
    {
        _item.Play();
    }
    public void PlayAlert()
    {
        if (!_alert.isPlaying)
            _alert.Play(); 
    }

    private IEnumerator CheckAlertState()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(3); 

        while (GameManager.gameState == GameManager.GameState.GamePlay)
        {
            bool shouldPlayAlertMusic = false;

            if (_enemyManager.enemiesList.Count > 0)
            {
                foreach (var enemy in _enemyManager.enemiesList)
                {
                    if (enemy.onAttack)
                    {
                        shouldPlayAlertMusic = true;
                        break;
                    }
                }
            }

            if (shouldPlayAlertMusic != _isAlert)
            {
                _isAlert = shouldPlayAlertMusic;
                if (_isAlert)
                {
                    ChangeMusic(_musicAlertClip);
                }
                else
                {
                    ChangeMusic(_musicGameClip); 
                }
            }

            yield return waitForSeconds;
        }
    }

}
