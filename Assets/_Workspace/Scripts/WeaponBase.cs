using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField, Range(1, 10)] protected float _damage = 1;
    [Space(5)]
    [SerializeField, Range(5, 30)] protected int _bulletOnMagazineCount = 8;
    [Space(5)]
    [SerializeField, Range(1, 5)] protected int _shootCount = 1;
    [SerializeField, Range(1f, 5f)] protected float _reloadTime = 1;
    [SerializeField, Range(.15f, 5f)] protected float _shootDelay = 1f;

    [Space(10), Header("Components")]
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] private ObjectPool _bloodEffectPool;
    [SerializeField] private ObjectPool _hitEffectPool;
    [SerializeField] protected ParticleSystem _shootEffect;
    [SerializeField] protected Transform _shootPos;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _shootClip;
    [SerializeField] protected AudioClip _reloadClip;

    protected List<Bullet> _bulletsOnMagazine;
    public Transform shootPos => _shootPos;

    protected int _bulletOnMagazine;
    protected bool _isReloading;
    public float shootDelay => _shootDelay;
    public int shootCount => _shootCount;
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
        if (!_isReloading && !_hasBulletOnMagazine)
        {
            StartCoroutine(Reload());
        }
    }
    protected virtual void Construct()
    {
        _bulletsOnMagazine = new List<Bullet>();
        for (int i = 0; i < _bulletOnMagazineCount; i++)
        {
            Bullet bullet = Instantiate(_bulletPrefab, _shootPos);
            bullet.Initialize(_bloodEffectPool, _hitEffectPool);
            bullet.transform.localPosition = Vector3.zero;
            bullet.gameObject.SetActive(false);
            _bulletsOnMagazine.Add(bullet);
        }

        _bulletOnMagazine = _bulletOnMagazineCount;
        onEndReload?.Invoke(_bulletOnMagazine, _bulletsOnMagazine.Count);
    }
    public void SetParameters(float damage, float delay)
    {
        _damage = damage; 
        _shootDelay = delay;
    }
    public void Shoot(Vector3 pos, bool headshot)
    {
        if (_bulletsOnMagazine.Count <= 0 || _isReloading) return; 

        Bullet bullet = _bulletsOnMagazine[0];
        bullet.gameObject.SetActive(true);
        bullet.transform.parent = null;
        bullet.Shoot(pos, _damage, headshot);
        bullet.OnHitTarget += BulletHitTarget;

        _audioSource.PlayOneShot(_shootClip);
        _bulletsOnMagazine.Remove(bullet);
        _bulletOnMagazine--;
        _shootEffect.Play();
        onShoot?.Invoke(_bulletOnMagazine, _bulletOnMagazineCount);
    }
    protected virtual void BulletHitTarget(Bullet bullet)
    {
        bullet.transform.parent = _shootPos; 
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.identity; 
        bullet.OnHitTarget -= BulletHitTarget;

        _bulletsOnMagazine.Add(bullet);
    }

    protected virtual IEnumerator Reload()
    {
        _isReloading = true;
        onStartReload?.Invoke();
        _audioSource.PlayOneShot(_reloadClip);

        yield return new WaitForSeconds(_reloadTime);

        _bulletOnMagazine = _bulletsOnMagazine.Count; 

        _isReloading = false;
        onEndReload?.Invoke(_bulletOnMagazine, _bulletsOnMagazine.Count); 
    }
}
