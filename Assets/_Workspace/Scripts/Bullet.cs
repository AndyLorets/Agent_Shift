using System;
using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{
    private Vector3 _lastPos;
    private float _damage;
    private bool _headShot;

    public Action<Bullet> OnHitTarget;

    private ObjectPool _bloodEffectPool;
    private ObjectPool _hitEffectPool;

    private const float SPEED = 40f;
    public void Initialize(ObjectPool bloodEffectPool, ObjectPool hitEffectPool)
    {
        _bloodEffectPool = bloodEffectPool;
        _hitEffectPool = hitEffectPool; 
    }

    public void Shoot(Vector3 pos, float damage, bool headShot)
    {
        _damage = damage;
        _headShot = headShot;
        transform.LookAt(pos);
        _lastPos = transform.position;

        Invoke(nameof(HitTarget), 3);
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * SPEED);
    }

    private void Update()
    {
        Move();
        CheckHit();
    }

    private void CheckHit()
    {
        RaycastHit hit;
        if (Physics.Linecast(_lastPos, transform.position, out hit))
        {
            ITakeDamage takeDamage = hit.collider.GetComponent<ITakeDamage>();
            if (takeDamage != null)
            {
                takeDamage.TakeDamage(_damage, _headShot);
                var bloodEffect = _bloodEffectPool.GetObject(hit.point, Quaternion.identity);
                StartCoroutine(ReturnToPool(bloodEffect, _bloodEffectPool));
            }
            else
            {
                var hitEffect = _hitEffectPool.GetObject(hit.point, Quaternion.identity); 
                StartCoroutine(ReturnToPool(hitEffect, _hitEffectPool));
            }
            print(hit.transform.name); 
            HitTarget();
        }
        _lastPos = transform.position;
    }

    private void HitTarget()
    {
        OnHitTarget?.Invoke(this);
        gameObject.SetActive(false);
        CancelInvoke(nameof(HitTarget));
    }

    private IEnumerator ReturnToPool(GameObject obj, ObjectPool pool)
    {
        yield return new WaitForSeconds(1.0f); 
        pool.ReturnObject(obj);
    }
}
