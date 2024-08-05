using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private AttackState _attakState;
    [SerializeField] private PatroolState _patroolState;
    [SerializeField] private LureState _lureState;
    [SerializeField] private UnitInventory _unitInventory;
    [Space(5)]
    [SerializeField] private bool _isRequestingAssistance = true;

    private Collider _collider;
    private EnemyLureSpeakerManager _speakersManager;
    private StateMachine _stateMachine = new StateMachine();

    public System.Action<Vector3> onPlayerVisible;
    public NavMeshAgent agent { get; private set; }
    public Player player { get; private set; }

    private const float DETECTED_RADIUS = 20f;
    public bool EnemyIsDetected => IsEnemyDetected() && !player.IsInvisibility;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _collider = agent.GetComponent<Collider>();
    }
    private void Update()
    {
        if (agent.hasPath && agent.velocity.sqrMagnitude == 0f)
        {
            Vector3 destination = agent.destination;
            agent.ResetPath();
            agent.SetDestination(destination);
        }
        if (_stateMachine.currentState == _attakState && player.IsInvisibility)
        {
            _lureState.SetLurePoint(player.transform.position);
            _stateMachine.ChangeState(_lureState);
        }
    }
    protected override void Construct()
    {
        base.Construct();

        player = ServiceLocator.GetService<Player>();
        _speakersManager = ServiceLocator.GetService<EnemyLureSpeakerManager>();

        _patroolState.onPlayerVisible += EnterAttackState;
        _lureState.onPlayerVisible += EnterAttackState;
        _lureState.onPlayerUnvisible += EnterPatroolState;
        player.onDead += OnPlayerDead;
        player.Skin.onFootStep += EnterPlayerLureState;

        GameManager.onGameWin += ExitAllState;
        InitAllLures(true);
        EnemiesSubscrip(true);

        EnterPatroolState();
    }
    protected override void ConstructTargets()
    {
        _targets.Add(player);
    }
    private void EnemiesSubscrip(bool state)
    {
        foreach (Enemy enemy in _enemyManager.enemiesList)
        {
            if (enemy != this)
            {
                if (state)
                {
                    if (_isRequestingAssistance)
                        enemy.onPlayerVisible += OnRequestingAssistance;
                }
                else if (_isRequestingAssistance)
                    enemy.onPlayerVisible -= OnRequestingAssistance;
            }
        }
    }
    private void InitAllLures(bool value)
    {
        List<EnemyLureSpeaker> lures = _speakersManager.lureSpeakerList;

        for (int i = 0; i < lures.Count; i++)
        {
            if (value)
                lures[i].onLure += EnterLureState;
            else
                lures[i].onLure -= EnterLureState;
        }
    }

    private void OnPlayerDead(Character character)
    {
        if (character != player) return;

        EnterPatroolState();
    }
    private void OnRequestingAssistance(Vector3 pos)
    {
        float dist = Vector3.Distance(transform.position, pos);
        if (dist <= DETECTED_RADIUS)
        {
            _stateMachine.ChangeState(_attakState);
        }
    }
    private void EnterAttackState()
    {
        _stateMachine.ChangeState(_attakState);
        onPlayerVisible?.Invoke(transform.position);
    }
    private void EnterPatroolState()
    {
        _stateMachine.ChangeState(_patroolState);
    }
    private void EnterLureState(Vector3 pos)
    {
        float dist = Vector3.Distance(transform.position, pos);
        if (dist > DETECTED_RADIUS || !_isRequestingAssistance) return;

        if (_stateMachine.currentState == _attakState || _stateMachine.currentState == _lureState)
            return;

        _lureState.SetLurePoint(pos);
        _stateMachine.ChangeState(_lureState);
    }
    private void EnterPlayerLureState(Vector3 pos)
    {
        float dist = Vector3.Distance(transform.position, pos);
        if (dist > _visibleRange || !_isRequestingAssistance) return;

        if (_stateMachine.currentState == _attakState || _stateMachine.currentState == _lureState)
            return;

        _lureState.SetLurePoint(pos);
        _stateMachine.ChangeState(_lureState);
    }
    public override void TakeDamage(int value, bool headShoot)
    {
        EnterAttackState();
        base.TakeDamage(value, headShoot);
    }
    public override void Dead(bool headShot)
    {
        base.Dead(headShot);
        ExitAllState();
        _collider.enabled = false;
        if (_unitInventory.gameObject.activeSelf)
            _unitInventory.SpawnItem();
    }
    private void ExitAllState()
    {
        _stateMachine.ExitActiveState();
        enabled = false;
    }
    private void OnDestroy()
    {
        InitAllLures(false);
        EnemiesSubscrip(false);
        _lureState.onPlayerVisible -= EnterAttackState;
        _lureState.onPlayerUnvisible -= EnterPatroolState;
        _patroolState.onPlayerVisible -= EnterAttackState;
        player.onDead -= OnPlayerDead;
        player.Skin.onFootStep -= EnterPlayerLureState;
        GameManager.onGameWin -= ExitAllState;
    }
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, DETECTED_RADIUS);
    }
}
