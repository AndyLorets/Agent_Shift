using System;
using UnityEngine;
using TMPro; 

public class Bullet : MonoBehaviour
{
    private Vector3 _lastPos;
    private float _damage;
    private bool _headShot;

    public Action<Bullet> OnHitTarget; 

    private const float SPEED = 40f;

    public void Shoot(Vector3 pos, float damgae, bool headShot)
    {
        _damage = damgae;
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
        if(Physics.Linecast(_lastPos, transform.position, out hit))
        {
            ITakeDamage takeDamage = hit.collider.GetComponent<ITakeDamage>();
            if (takeDamage != null)
            {
                takeDamage.TakeDamage(_damage, _headShot); 
            }

            HitTarget();
        }
    }

    private void HitTarget()
    {
        OnHitTarget?.Invoke(this);
        gameObject.SetActive(false);
        CancelInvoke(nameof(HitTarget));
    }
}
