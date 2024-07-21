using UnityEngine;

public class WeaponBehaviour
{
    private Weapon _weapon;
    private Character _character;
    private Animator _animator; 

    private float _shootDelay;
    private float _currentShootTime;

    private ITakeDamage _takeDamage;
    private Vector3 _shootPos;

    private const string ANIM_SHOOT = "Shoot";
    private const string ANIM_RELAOD = "Reload";

    private float _lstTime; 
    public WeaponBehaviour(Character character, Weapon weapon, Animator animator)
    {
        _character = character;
        _weapon = weapon;
        _animator = animator;
        _shootDelay = weapon.shootDelay;

        weapon.onStartReload += OnReloadWeapon; 
    }
    public void Run()
    {
        bool canShoot = _character.IsEnemyDetected(out _takeDamage, out _shootPos);
        if (canShoot)
        {
            Shooting();
        }
    }
    public void Unrun()
    {
        if (_currentShootTime != 0)
            _currentShootTime = 0; 
    }
    private void Shooting()
    {
        _currentShootTime -= Time.deltaTime;

        if (_currentShootTime <= 0 && !_weapon.isReloading)
        {
            _animator.SetTrigger(ANIM_SHOOT);
            _weapon.Shoot(_takeDamage, _shootPos);
            _currentShootTime = _shootDelay;
        }
    }

    private void OnReloadWeapon()
    {
        _animator.SetTrigger(ANIM_RELAOD);
    }
}