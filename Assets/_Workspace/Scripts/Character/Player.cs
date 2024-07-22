using System;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private WeaponBase _weapon;
    [SerializeField] private Joystick _joystickMovement;
    [SerializeField] private Joystick _joystickAim;
    [SerializeField] private WeaponLineRender _weaponLineRender;
    [SerializeField] CharacterRigController _rig; 
    public Rigidbody rb { get; private set; }
    public string CurrentWeaponName => "Pistol";

    private MoveBehaviour _moveBehaviour;
    private WeaponBehaviour _weaponBehaviour;
    private bool _isAiming => _joystickAim.Horizontal != 0 || _joystickAim.Vertical != 0;
    private bool _aiming;

    public Action<float, float> onChangeHP;

    public WeaponBase currentWeapon => _weapon;
    private Vector3 GetMoveDirection()
    {
        float horizontal = 0;
        float vertical = 0;

        horizontal = Input.GetAxis("Horizontal") + _joystickMovement.Horizontal;
        vertical = Input.GetAxis("Vertical") + _joystickMovement.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction;
    }
    private Vector3 GetAimDirection()
    {
        float horizontal = _joystickAim.Horizontal;
        float vertical = _joystickAim.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction;
    }
    private void Update()
    {
        RunBehaviours();
        SwitchMoveBehaviour();
        AimingHandler();
        RigHandler(); 
    }
    protected override void Construct()
    {
        base.Construct();
        rb = GetComponent<Rigidbody>();

        _weaponLineRender.Construct(_viewAngle, _visibleRange);
        _rig.Construct(_weapon);
        onChangeHP?.Invoke(_currentHP, _hp);

        ConstructBehaviours();
    }
    private void ConstructBehaviours()
    {
        _moveBehaviour = new IdleMoveBehaviour(this);
        _weaponBehaviour = new WeaponBehaviour(this, _weapon, Animator);
    }
    protected override void ConstructEnemyList()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        _enemyList.AddRange(enemies);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].onDead += OnEnemyDead;
        }
    }
    private void RunBehaviours()
    {
        _moveBehaviour.Move(GetMoveDirection(), GetAimDirection());
        if (_isAiming)
        {
            _weaponBehaviour.Run();
        }
        else
        {
            _weaponBehaviour.Unrun();
        }
    }
    private void SwitchMoveBehaviour()
    {
        if (_isAiming && _moveBehaviour.GetType() == typeof(IdleMoveBehaviour))
        {
            _moveBehaviour = new AimMoveBehaviour(this);
        }
        else if (!_isAiming && _moveBehaviour.GetType() == typeof(AimMoveBehaviour))
        {
            _moveBehaviour = new IdleMoveBehaviour(this);
        }
    }
    private void AimingHandler()
    {
        if (_isAiming != _aiming)
        {
            _aiming = _isAiming;
            if (!_isAiming)
            {
                _weaponLineRender.DrawLine(false, false);
                _rig.DeactiveRig();
            }
            else
                _rig.ActiveRig();
        }

        if (_isAiming)
        {
            Character nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                _rig.SetAimTargetPos(nearestEnemy.transform.position); 
            }
            else
            {
                _rig.SetAimTargetPos(); 
            }

            _weaponLineRender.DrawLine(true, _enemyDetected);
        }
    }
    private void RigHandler()
    {
        if (!_isAiming)
        {
            if (GetMoveDirection().sqrMagnitude > 0.03f)
                _rig.ActiveRig();
            else
                _rig.DeactiveRig();
        }
    }
    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        onChangeHP?.Invoke(_currentHP, _hp);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _enemyList.Count; i++)
        {
            Enemy enemy = _enemyList[i] as Enemy;
            enemy.onDead -= OnEnemyDead;
        }
    }
    protected void OnEnemyDead(Enemy enemy) => _enemyList.Remove(enemy);
}

