using UnityEngine;

public class AttackState : StateBase
{
    [SerializeField] private Weapon _weapon;

    private WeaponBehaviour _weaponBehaviour;

    private float _changeStateTimer;
    private bool _shooting;
    bool _canChangeState => _changeStateTimer <= 0;

    private const float CHANGE_STATE_TIME = .3f;
    private const float RUN_SPEED = 4f;

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
        Run();

        if (_changeStateTimer >= 0)
            _changeStateTimer -= Time.deltaTime;
    }

    private void Run()
    {
        bool isMove = _enemy.agent.velocity.sqrMagnitude > 0;
        bool detected = _enemy.IsEnemyDetected(); 

        if (detected)
            Attack();
        else 
        {
            _weaponBehaviour.Unrun();
            Follow();
        }

        _enemy.Animator.SetBool(ANIM_RUN, isMove);
    }
    private void Attack()
    {
        if (!_canChangeState) return;

        if (!_shooting)
        {
            _shooting = true;
            _changeStateTimer = CHANGE_STATE_TIME;

            _enemy.agent.SetDestination(transform.position);
            _enemy.Animator.SetTrigger(ANIM_AIM);
        }

        LookAtPlayer();
        _weaponBehaviour.Run(); 
    }
    private void Follow()
    {
        if (!_canChangeState) return; 

        if (_shooting)
        {
            _shooting = false;
            _changeStateTimer = CHANGE_STATE_TIME;
        }

        _enemy.agent.SetDestination(_enemy.player.transform.position);
    }
    public override void EnterState()
    {
        base.EnterState();
        _enemy.agent.speed = RUN_SPEED;
        _enemy.onSendMessag?.Invoke("!!!");
    }
    public override void ExitState()
    {
        base.ExitState();

        _enemy.Animator.SetBool(ANIM_RUN, false); 
    }
}
