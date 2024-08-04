using UnityEngine;

public class AttackState : StateBase
{
    [SerializeField] private WeaponBase _weapon;
    private WeaponBehaviour _weaponBehaviour;

    private float _changeStateTimer;
    private bool _shooting;
    bool _canChangeState => _changeStateTimer <= 0;
    bool canChangeState;

    private const float CHANGE_STATE_TIME = .3f;
    private const float RUN_SPEED = 4f;

    private Enemy _enemy;
    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponent<Enemy>();
    }

    protected override void Start()
    {
        base.Start();
        _weaponBehaviour = new WeaponBehaviour(_enemy, _weapon, _enemy.Animator);

        enabled = false; 
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
        bool detected = _enemy.IsEnemyDetected();
        bool isMove = _enemy.agent.velocity.sqrMagnitude > 0;

        _enemy.Animator.SetBool(ANIM_RUN, isMove);
        _enemy.Animator.SetBool(ANIM_AIM, detected);

        if (detected)
            Attack();
        else
            Follow();
    }
    private void Attack()
    {
        if (!_canChangeState) return;

        if (!_shooting)
        {
            _shooting = true;
            _changeStateTimer = CHANGE_STATE_TIME;

            _enemy.agent.SetDestination(transform.position);
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
        //CharacterMessanger.instance.SetDialogueMessage(_enemy.icon, _CharacterDialogue[r].text, _CharacterDialogue[r].clip);
    }
    public override void ExitState()
    {
        base.ExitState();
        _enemy.agent.SetDestination(transform.position);
        _enemy.Animator.SetBool(ANIM_RUN, false);
        _enemy.Animator.SetBool(ANIM_AIM, false);
    }
}
