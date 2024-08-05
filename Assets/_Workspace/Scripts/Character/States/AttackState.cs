using UnityEngine;

public class AttackState : StateBase
{
    [SerializeField] private WeaponBase _weapon;
    [SerializeField] private float _attackRadius;

    private WeaponBehaviour _weaponBehaviour;
    private float _changeStateTimer;

    private const float CHANGE_ATTACK_TIME = 1f;
    private const float CHANGE_FOLLOW_TIME = 0.3f;
    private const float RUN_SPEED = 4f;

    private State _state = State.None;
    private enum State
    {
        None, Follow, Attack, Reload
    }

    private Enemy _enemy;

    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponent<Enemy>();

        _weapon.onStartReload += StartReload; 
        _weapon.onEndReload += EndReload;
    }

    protected override void Start()
    {
        base.Start();
        _weaponBehaviour = new WeaponBehaviour(_enemy, _weapon, _enemy.Animator);
    }

    private void LookAtPlayer()
    {
        transform.LookAt(_enemy.player.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void Update()
    {
        if (_changeStateTimer > 0)
            _changeStateTimer -= Time.deltaTime;

        Run();
    }

    private void Run()
    {
        bool isMove = _enemy.agent.velocity.sqrMagnitude > 0;
        bool detected = _enemy.IsEnemyDetected();

        _enemy.Animator.SetBool(ANIM_RUN, isMove);

        if (_changeStateTimer <= 0 && _state != State.Reload)
        {
            if (_state != State.Follow && !detected)
            {
                _state = State.Follow;
                _enemy.Animator.SetBool(ANIM_AIM, false);
                _changeStateTimer = CHANGE_FOLLOW_TIME;
                _enemy.agent.SetDestination(_enemy.player.transform.position);
                _weaponBehaviour.StopShooting();
            }
            else if (_state != State.Attack && detected)
            {
                _state = State.Attack;
                _changeStateTimer = CHANGE_ATTACK_TIME;
                _enemy.Animator.SetBool(ANIM_AIM, true);
                _enemy.agent.SetDestination(transform.position);
            }
        }

        if (_state == State.Attack)
        {
            Attack();
        }
        else if (_state == State.Follow)
        {
            Follow();
        }
    }
    private void StartReload()
    {
        _enemy.Animator.SetBool(ANIM_AIM, false);
        _state = State.Reload;
    }
    private void EndReload(int agr1, int agr2)
    {
        _state = State.None;
    }
    private void Attack()
    {
        LookAtPlayer();
        _weaponBehaviour.Run();
    }

    private void Follow()
    {
        _enemy.agent.SetDestination(_enemy.player.transform.position);
    }

    public override void EnterState()
    {
        base.EnterState();
        _enemy.agent.speed = RUN_SPEED;
        ServiceLocator.GetService<AudioManager>().PlayAlert();
    }

    public override void ExitState()
    {
        base.ExitState();
        _enemy.agent.SetDestination(transform.position);
        _enemy.Animator.SetBool(ANIM_RUN, false);
        _enemy.Animator.SetBool(ANIM_AIM, false);
        _state = State.Follow; 
    }
    private void OnDestroy()
    {
        _weapon.onStartReload -= StartReload;
        _weapon.onEndReload -= EndReload;
    }
}
