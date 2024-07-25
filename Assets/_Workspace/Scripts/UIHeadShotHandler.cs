using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UIHeadShotHandler : MonoBehaviour
{
    [SerializeField] private GameObject _panel; 
    private List <Enemy> enemyList = new List <Enemy>();

    private void Awake()
    {
        Construct(); 
    }
    private void Construct()
    {
        _panel.SetActive(false);
        enemyList.AddRange(FindObjectsOfType<Enemy>()); 

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].onChangeHP += OnDamage; 
        }

    }
    private void OnDestroy()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].onChangeHP -= OnDamage;
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
