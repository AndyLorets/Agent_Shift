using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private AttackState _attakState;
    [SerializeField] private PatroolState _patroolState;
    [SerializeField] private LureState _lureState;

    private Collider _collider;
    private StateMachine _stateMachine = new StateMachine();

    public System.Action<Vector3> onPlayerVisible;
    public NavMeshAgent agent { get; private set; }
    public Player player { get; private set; }

    private const float DETECTED_RADIUS = 20f; 

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
    }
    protected override void Construct()
    {
        base.Construct();

        EnterPatroolState(); 

        _patroolState.onPlayerVisible += EnterAttackState;
        _lureState.onPlayerVisible += EnterAttackState; 
        _lureState.onPlayerUnvisible += EnterPatroolState;
        InitAllLures(true);
        FindAllEnemiesSubscrip(true); 
    }
    private void FindAllEnemiesSubscrip(bool state)
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != this)
            {
                if (state)
                    enemy.onPlayerVisible += OnPlayerDetected; 
                else
                    enemy.onPlayerVisible -= OnPlayerDetected;
            }
        }
    }
    private void InitAllLures(bool value)
    {
        EnemyLureSpeaker[] lures = FindObjectsOfType<EnemyLureSpeaker>(); 
        for (int i = 0; i < lures.Length; i++)
        {
            if (value)
                lures[i].onLure += EnterLureState; 
            else
                lures[i].onLure -= EnterLureState;
        }
    }
    protected override void ConstructEnemyList()
    {
        Player[] enemies = FindObjectsOfType<Player>();
        _enemyList.AddRange(enemies);
        player = _enemyList[0] as Player;
    }
    private void OnPlayerDetected(Vector3 pos)
    {
        float dist = Vector3.Distance(transform.position, pos);
        if (dist <= DETECTED_RADIUS)
            _stateMachine.ChangeState(_attakState); ;
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
        if (_stateMachine.currentState == _attakState || _stateMachine.currentState == _lureState)
            return;

        float dist = Vector3.Distance(transform.position, pos);
        if (dist > DETECTED_RADIUS) return;

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
        _stateMachine.ExitActiveState();
        _collider.enabled = false;
    }
    private void OnDestroy()
    {
        InitAllLures(false);
        FindAllEnemiesSubscrip(false);
        _lureState.onPlayerVisible -= EnterAttackState;
        _lureState.onPlayerUnvisible -= EnterPatroolState;
        _patroolState.onPlayerVisible -= EnterAttackState;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, DETECTED_RADIUS); 
    }
}
