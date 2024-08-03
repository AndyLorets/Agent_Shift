using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Hostage : Character, ITaskable
{
    [SerializeField] private FollowingState _followingState;
    [SerializeField] private int _startAnimPos;

    private StateMachine _stateMachine = new StateMachine();

    [SerializeField] private CharacterDialogue[] _CharacterDialogue; 

    public NavMeshAgent agent { get; private set; }
    public string taskName { get; set; }

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
        int r = Random.Range(0, _CharacterDialogue.Length);

        Animator.SetTrigger("StandUp");
        CharacterMessanger.instance.SetDialogueMessage(icon, _CharacterDialogue[r].text, _CharacterDialogue[r].clip);
        TaskManager.Instance.CompleteTask(taskName);

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
