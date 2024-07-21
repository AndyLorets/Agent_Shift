using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Parameters")]
    public int damage = 1;
    [Space(5)]
    [SerializeField, Range(5, 30)] private int _bulletOnMagazineCount = 8;
    [SerializeField, Range(10, 100)] private int _bulletCount = 30;
    [Space(5)]
    [SerializeField, Range(1f, 5f)] private float _reloadTime = 1;
    [SerializeField, Range(.15f, 5f)] private float _shootDelayTime = 1f;

    [Space(10), Header("Components")]
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private Transform _shootPos;

    private List<Transform> _bulletsOnMagazine;
    public Transform shootPos => _shootPos;

    private int _bulletOnMagazine;
    private bool _isReloading;
    public float shootDelay => _shootDelayTime;
    public bool isReloading => _isReloading;
    private bool _hasBulletOnMagazine => _bulletOnMagazine > 0;

    private const float BULLET_TWEEN_DURATION = .15f;

    public Action onStartReload;
    public Action<int, int> onEndReload;
    public Action<int, int> onShoot;

    private void Start()
    {
        Construct();
    }
    private void Update()
    {
        if(!_isReloading && !_hasBulletOnMagazine)
        {
            StartCoroutine(Reload());
        }
    }
    private void Construct()
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
    public virtual void Shoot(ITakeDamage takeDamageDamage, Vector3 pos)
    {
        _bulletOnMagazine--;
        _shootEffect.Play();
        onShoot?.Invoke(_bulletOnMagazine, _bulletsOnMagazine.Count);

        Transform bullet = _bulletsOnMagazine[0];
        bullet.transform.parent = null;
        bullet.gameObject.SetActive(true);
        bullet.DOMove(pos, BULLET_TWEEN_DURATION)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnBulletHitTarget(bullet, takeDamageDamage));

        _bulletsOnMagazine.RemoveAt(0);
    }
    private void OnBulletHitTarget(Transform bullet, ITakeDamage takeDamageDamage)
    {
        _bulletsOnMagazine.Add(bullet);
        bullet.gameObject.SetActive(false);
        bullet.transform.parent = _shootPos; 
        bullet.transform.localPosition = Vector3.zero;
        takeDamageDamage.TakeDamage(damage);
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        onStartReload?.Invoke(); 

        yield return new WaitForSeconds(_reloadTime);

        _bulletOnMagazine = _bulletsOnMagazine.Count; 

        _isReloading = false;
        onEndReload?.Invoke(_bulletOnMagazine, _bulletsOnMagazine.Count); 
    }
}
