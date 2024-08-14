using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHeadShotHandler : MonoBehaviour
{
    [SerializeField] private GameObject _panel; 

    private EnemyManager _enemyManager;
    private AudioManager _audioManager;

    private const float DURATION = 1f; 

    private void Start()
    {
        Construct(); 
    }
    private void Construct()
    {
        _panel.SetActive(false);

        _audioManager = ServiceLocator.GetService<AudioManager>(); 
        _enemyManager = ServiceLocator.GetService<EnemyManager>();  

        for (int i = 0; i < _enemyManager.enemiesList.Count; i++)
        {
            _enemyManager.enemiesList[i].onChangeHP += OnDamage; 
        }

    }
    private void OnDestroy()
    {
        for (int i = 0; i < _enemyManager.enemiesList.Count; i++)
        {
            _enemyManager.enemiesList[i].onChangeHP -= OnDamage;
        }
    }
    private void OnDamage(float currentHP, float maxHp, bool headshot)
    {
        if (!headshot) return;

        _panel.SetActive(false);
        _panel.transform.localScale = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(ShowUI()); 
    }
    private IEnumerator ShowUI()
    {
        _audioManager.PlayHeadShot();
        _panel.SetActive(true);
        _panel.transform.DOScale(Vector3.one * 1.2f, .2f)
            .OnComplete(() => _panel.transform.DOScale(Vector3.one, .1f)); 

        yield return new WaitForSeconds(DURATION);
        _panel.SetActive(false);
    }
}
