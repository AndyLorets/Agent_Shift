using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Hostage : Character
{
    [SerializeField] private FollowingState _followingState;
    [SerializeField] private int _startAnimPos;

    private StateMachine _stateMachine = new StateMachine();
    public NavMeshAgent agent { get; private set; }

    private const string ANIM_HOSTAGE = "HostagePos";
    private const string ANIM_HOSTAGE_NUM = "HostagePosNum";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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

        Animator.SetTrigger(ANIM_HOSTAGE);
        Animator.SetInteger(ANIM_HOSTAGE_NUM, _startAnimPos);
    }
    public void Released()
    {
        Animator.SetTrigger("StandUp");
        onSendMessag?.Invoke("Thank You!");

        Invoke(nameof(EnterFollowingState), 2f);
    }
    private void EnterFollowingState()
    {
        _stateMachine.ChangeState(_followingState);
    }
    protected override void ConstructEnemyList()
    {
    }
    public override void Dead(bool headShot)
    {
        base.Dead(headShot);
        _stateMachine.ExitActiveState();
    }
}
