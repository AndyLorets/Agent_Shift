using UnityEngine;

public class Player : Character
{
    [SerializeField] private WeaponBase _weapon;
    [SerializeField] private Joystick _joystickMovement;
    [SerializeField] private Joystick _joystickAim;
    [SerializeField] private WeaponLineRender _weaponLineRender;
    [SerializeField] CharacterRigController _rig;
    [SerializeField] private PlayerAbilities _playerAbilities;

    public bool IsInvisibility { get; private set; }
    public bool IsArmom { get; private set; }
    public Rigidbody rb { get; private set; }
    public string CurrentWeaponName => "Pistol";

    private MoveBehaviour _moveBehaviour;
    private WeaponBehaviour _weaponBehaviour;
    private bool _isAiming => _joystickAim.Horizontal != 0 || _joystickAim.Vertical != 0;
    private bool _aiming;
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
    private void SetInvisibility(bool value) => IsInvisibility = value;
    private void SetArmor(bool value) => IsArmom = value;
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
        onChangeHP?.Invoke(_currentHP, _hp, false);

        ConstructBehaviours();

        PlayerAbilities.onChangeInvisibility += SetInvisibility;
        PlayerAbilities.onChangeArmor += SetArmor;
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
    public override bool IsEnemyDetected(out ITakeDamage takeDamage, out Vector3 pos, out bool headshoot)
    {
        bool isDetected = base.IsEnemyDetected(out takeDamage, out pos, out headshoot);

        if (isDetected)
        {
            float randomValue = Random.Range(0f, 100f);
            headshoot = randomValue <= _playerAbilities.headShotChance;

            if (headshoot)
            {
                pos.y += 0.8f;
            }
        }

        return isDetected;
    }


    private void OnDestroy()
    {
        for (int i = 0; i < _enemyList.Count; i++)
        {
            Enemy enemy = _enemyList[i] as Enemy;
            enemy.onDead -= OnEnemyDead;
        }

        PlayerAbilities.onChangeInvisibility -= SetInvisibility;
        PlayerAbilities.onChangeArmor -= SetArmor;
    }
    public override void TakeDamage(int value, bool headShot)
    {
        value = IsArmom ? 0 : value; 
        base.TakeDamage(value, headShot);
    }
    protected void OnEnemyDead(Character enemy) => _enemyList.Remove(enemy);
}

