using UnityEngine;

public class Player : Character
{
    [SerializeField] private WeaponBase _weapon;
    [SerializeField] private Joystick _joystickMovement;
    [SerializeField] private Joystick _joystickAim;
    [SerializeField] private WeaponLineRender _weaponLineRender;
    [SerializeField] private CharacterRigController _rig;

    public bool IsInvisibility { get; private set; }
    public bool IsArmom { get; private set; }
    public Rigidbody rb { get; private set; }
    public Joystick joystickMove => _joystickMovement;
    public Joystick joystickAim => _joystickAim;
    public string CurrentWeaponName => "Pistol";

    private Collider _collider;
    private MoveBehaviour _moveBehaviour;
    private WeaponBehaviour _weaponBehaviour;
    private PlayerAbilities _playerAbilities; 
    private bool _isAiming => _joystickAim.Horizontal != 0 || _joystickAim.Vertical != 0;
    private bool _aiming;
    public bool CanControll { get; set; } = true; 
    public WeaponBase currentWeapon => _weapon;
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
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
        _collider = GetComponent<Collider>();
        _playerAbilities = GetComponent<PlayerAbilities>(); 

        _weaponLineRender.Construct(_viewAngle, _visibleRange);
        _rig.Construct(_weapon);
        onChangeHP?.Invoke(_currentHP, _hp, false);

        ConstructBehaviours();

        PlayerAbilities.onInvisibility += SetInvisibility;
        PlayerAbilities.onArmor += SetArmor;
        BriefingManager.onStartBriefing += LoadData;
    }
    private void LoadData()
    {
        GameDataController gameDataController = ServiceLocator.GetService<GameDataController>();

        _weapon.SetParameters(gameDataController.PlayerData.weaponData.pistolCurrentDamage, gameDataController.PlayerData.weaponData.pistolCurrentShootDelay);
        _hp = gameDataController.PlayerData.currentHp;
        _currentHP = _hp;
    }
    private void ConstructBehaviours()
    {
        _moveBehaviour = new IdleMoveBehaviour(this);
        _weaponBehaviour = new WeaponBehaviour(this, _weapon, Animator);
    }
    protected override void ConstructTargets()
    {
        for (int i = 0; i < _enemyManager.enemiesList.Count; i++)
        {
            _targets = _enemyManager.enemiesList;
        }
    }
    private Vector3 GetMoveDirection()
    {
        float horizontal = CanControll ? Input.GetAxis("Horizontal") + _joystickMovement.Horizontal : 0;
        float vertical = CanControll ? Input.GetAxis("Vertical") + _joystickMovement.Vertical : 0;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction;
    }
    private Vector3 GetAimDirection()
    {
        float horizontal = CanControll ? _joystickAim.Horizontal : 0;
        float vertical = CanControll ? _joystickAim.Vertical : 0;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction;
    }
    private void SetInvisibility(bool value) => IsInvisibility = value;
    private void SetArmor(bool value) => IsArmom = value;
    private void RunBehaviours()
    {
        _moveBehaviour.Move(GetMoveDirection(), GetAimDirection());
        if (_isAiming && !IsInvisibility)
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
    public override bool IsEnemyDetected(out Vector3 pos, out bool headshoot)
    {
        bool isDetected = base.IsEnemyDetected(out pos, out headshoot);

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
        PlayerAbilities.onInvisibility -= SetInvisibility;
        PlayerAbilities.onArmor -= SetArmor;
        BriefingManager.onStartBriefing -= LoadData;
    }
    public override void TakeDamage(float value, bool headShot)
    {
        value = IsArmom ? 0 : value;

        string anim = headShot ? ANIM_DAMAGE_HEADSHOT : ANIM_DAMAGE;
        value = headShot ? value * 3 : value;

        _currentHP -= value;

        onTakeDamage?.Invoke(value);
        onChangeHP?.Invoke(_currentHP, _hp, headShot);

        if (_currentHP <= 0)
            Dead(headShot);
        else if (!IsArmom)
            Animator.SetTrigger(anim);

        int r = Random.Range(0, _damageClips.Length);

        if (!IsArmom)
            _audioSource.PlayOneShot(_damageClips[r]); 
    }
    public override void Dead(bool headShot)
    {
        base.Dead(headShot);
        _collider.enabled = false;
        rb.isKinematic = true;

        ServiceLocator.GetService<GameManager>().LoseGame(); 
    }
}

