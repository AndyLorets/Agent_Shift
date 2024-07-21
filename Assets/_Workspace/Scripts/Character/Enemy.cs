using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private AttackState _attakState;
    [SerializeField] private PatroolState _patroolState;

    private NavMeshAgent _agent;
    public NavMeshAgent agent => _agent;

    public Action<Enemy> onDead;

    public Player player { get; private set; }

    private StateMachine _stateMachine = new StateMachine();
   
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (agent.hasPath && agent.velocity.sqrMagnitude == 0f)
        {
            Vector3 destination = agent.destination; 
            agent.ResetPath();
            agent.SetDestination(destination);
        }
    }
    protected override void Construct()
    {
        base.Construct();

        _stateMachine.ChangeState(_patroolState);
        _patroolState.onPlayerVisible += OnPlayerDetected; 
    }
    protected override void ConstructEnemyList()
    {
        Player[] enemies = FindObjectsOfType<Player>();
        _enemyList.AddRange(enemies);
        player = _enemyList[0] as Player;
    }
    private void OnPlayerDetected()
    {
        _stateMachine.ChangeState(_attakState);
    }
    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        OnPlayerDetected();
    }
    public override void Dead()
    {
        base.Dead();
        onDead?.Invoke(this);
    }
    private void OnDestroy()
    {
        _patroolState.onPlayerVisible -= OnPlayerDetected;
    }
}
