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
    private float _lstTime; 
    public WeaponBehaviour(Character character, WeaponBase weapon, Animator animator)
    {
        _character = character;
        _weapon = weapon;
        _animator = animator;
        _shootDelay = weapon.shootDelay;
        _currentShootTime = _shootDelay;

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

        if (_currentShootTime <= 0 && !_weapon.isReloading)
        {
            _animator.SetTrigger(ANIM_SHOOT);
            _weapon.Shoot(_shootPos, headshoot);
            _currentShootTime = _shootDelay;
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