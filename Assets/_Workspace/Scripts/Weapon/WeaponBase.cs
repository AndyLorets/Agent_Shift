using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField, Range(1, 10)] protected int _damage = 1;
    [Space(5)]
    [SerializeField, Range(5, 30)] protected int _bulletOnMagazineCount = 8;
    [Space(5)]
    [SerializeField, Range(1f, 5f)] protected float _reloadTime = 1;
    [SerializeField, Range(.15f, 5f)] protected float _shootDelayTime = 1f;

    [Space(10), Header("Components")]
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected ParticleSystem _shootEffect;
    [SerializeField] protected Transform _shootPos;

    protected List<Transform> _bulletsOnMagazine;
    public Transform shootPos => _shootPos;

    protected int _bulletOnMagazine;
    protected bool _isReloading;
    public float shootDelay => _shootDelayTime;
    public bool isReloading => _isReloading;
    protected bool _hasBulletOnMagazine => _bulletOnMagazine > 0;

    protected const float BULLET_TWEEN_DURATION = .15f;

    public Action onStartReload;
    public Action<int, int> onEndReload;
    public Action<int, int> onShoot; 

    protected virtual void Start()
    {
        Construct();
    }
    protected virtual void Update()
    {
        if(!_isReloading && !_hasBulletOnMagazine)
        {
            StartCoroutine(Reload());
        }
    }
    protected virtual void Construct()
    {
        _bulletsOnMagazine = new List<Transform>();
        for (int i = 0; i < _bulletOnMagazineCount; i++)
        {
            Transform bullet = Instantiate(_bulletPrefab, _shootPos).transform;
            bullet.localPosition = Vector3.zero;
            bullet.gameObject.SetActive(false);
            _bulletsOnMagazine.Add(bullet);
        }

        _bulletOnMagazine = _bulletOnMagazineCount;
        onEndReload?.Invoke(_bulletOnMagazine, _bulletsOnMagazine.Count);
    }
    public abstract void Shoot(ITakeDamage takeDamageDamage, Vector3 pos, bool headshot);
    protected virtual void BulletHitTarget(Transform bullet, ITakeDamage takeDamageDamage, bool headshoot)
    {
        _bulletsOnMagazine.Add(bullet);
        bullet.gameObject.SetActive(false);
        bullet.transform.parent = _shootPos; 
        bullet.transform.localPosition = Vector3.zero;

        takeDamageDamage.TakeDamage(_damage, headshoot);
    }

    protected virtual IEnumerator Reload()
    {
        _isReloading = true;
        onStartReload?.Invoke(); 

        yield return new WaitForSeconds(_reloadTime);

        _bulletOnMagazine = _bulletsOnMagazine.Count; 

        _isReloading = false;
        onEndReload?.Invoke(_bulletOnMagazine, _bulletsOnMagazine.Count); 
    }
}
