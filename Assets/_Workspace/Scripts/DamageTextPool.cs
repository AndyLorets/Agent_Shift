using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : MonoBehaviour
{
    [SerializeField] private Character _character; 
    private Queue<DamageText> _damageTextPool;

    private void Awake()
    {
        _damageTextPool = new Queue<DamageText>();

        for (int i = 0; i < transform.childCount; i++)
        {
            DamageText damageTextInstance = transform.GetChild(i).GetComponent<DamageText>();
            damageTextInstance.gameObject.SetActive(false);
            damageTextInstance.Init(this);
            _damageTextPool.Enqueue(damageTextInstance);
        }

        _character.onTakeDamage += ShowDamageText; 
    }
    private void OnDestroy()
    {
        _character.onTakeDamage -= ShowDamageText;
    }
    private void ShowDamageText(float damage)
    {
        if (_damageTextPool.Count == 0 || damage > 999) return;

        DamageText damageTextInstance = _damageTextPool.Dequeue();
        damageTextInstance.gameObject.SetActive(true);
        damageTextInstance.SetDamageAmount(damage);
    }

    public void ReturnDamageText(DamageText damageTextInstance)
    {
        damageTextInstance.gameObject.SetActive(false);
        _damageTextPool.Enqueue(damageTextInstance);
    }
}
