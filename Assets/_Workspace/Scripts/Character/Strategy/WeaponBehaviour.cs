using UnityEngine;

public class WeaponBehaviour
{
    private WeaponBase _weapon;
    private Character _character;
    private Animator _animator; 

    private float _shootDelay;
    private float _currentShootTime;

    private Vector3 _shootPos;

    private const string ANIM_SHOOT = "Shoot";
    private const string ANIM_RELAOD = "Reload";

    private bool headshoot; 
    private float _fireDelay;
    private int _shootCount;
    private int _currentShootCount; 
    public WeaponBehaviour(Character character, WeaponBase weapon, Animator animator)
    {
        _character = character;
        _weapon = weapon;
        _animator = animator;
        _shootDelay = weapon.shootDelay;
        _shootCount = weapon.shootCount;

        _currentShootCount = _shootCount;
        _currentShootTime = 1;
        _weapon.onStartReload += OnReloadWeapon;
    }
    public void Run()
    {
        bool canShoot = _character.IsEnemyDetected(out _shootPos, out headshoot);
        if (canShoot)
        {
            Shooting();
        }
    }
    private void Shooting()
    {
        _currentShootTime -= Time.deltaTime;
        _fireDelay -= Time.deltaTime;

        if (_currentShootTime <= 0 && !_weapon.isReloading && _fireDelay <= 0)
        {
            _animator.SetTrigger(ANIM_SHOOT);
            _weapon.Shoot(_shootPos, headshoot);

            _fireDelay = .3f; 
            _currentShootCount--;

            if (_currentShootCount <= 0)
            {
                _currentShootTime = _shootDelay;
                _currentShootCount = _shootCount; 
            }
        }
    }

    private void OnReloadWeapon()
    {
        _animator.SetTrigger(ANIM_RELAOD);
        _currentShootTime = _shootDelay;
    }
    public void StopShooting()
    {
        if (_currentShootTime < .4f)
            _currentShootTime = .4f;
    }
}