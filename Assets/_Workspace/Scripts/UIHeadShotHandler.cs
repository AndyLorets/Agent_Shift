using System.Collections;
using UnityEngine;

public class UIHeadShotHandler : MonoBehaviour
{
    [SerializeField] private GameObject _panel; 
    private EnemyManager _enemyManager;

    private void Start()
    {
        Construct(); 
    }
    private void Construct()
    {
        _panel.SetActive(false);

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
        StopAllCoroutines();
        StartCoroutine(ShowUI()); 
    }
    private IEnumerator ShowUI()
    {
        _panel.SetActive(true);
        yield return new WaitForSeconds(3f);
        _panel.SetActive(false);
    }
}
