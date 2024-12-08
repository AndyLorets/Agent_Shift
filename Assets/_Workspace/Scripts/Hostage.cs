using UnityEngine;
using UnityEngine.AI;

public class Hostage : Character, ITaskable 
{
    [SerializeField] private FollowingState _followingState;
    [SerializeField] private int _startAnimPos;
    [SerializeField] private Outline _outline;
    private readonly StateMachine _stateMachine = new StateMachine();
    [SerializeField] private CharacterDialogue[] _dialogue;
    public NavMeshAgent agent { get; private set; }
    public string taskName { get; set; }
    public bool activeTask { get; set; }

    private Collider _collider;
    private Rigidbody _rb;


    private const string AnimHostageTrigger = "HostagePos";
    private const string AnimHostageParam = "HostagePosNum";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _outline.enabled = false;
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
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
        Animator.SetTrigger(AnimHostageTrigger);
        Animator.SetInteger(AnimHostageParam, _startAnimPos);
    }

    public void Released()
    {
        int randomIndex = Random.Range(0, _dialogue.Length);

        Animator.SetTrigger("StandUp");
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(icon, _dialogue[randomIndex]);
        ServiceLocator.GetService<TaskManager>().CompleteTask(taskName);
        Invoke(nameof(EnterFollowingState), 2f);
    }

    private void EnterFollowingState()
    {
        _stateMachine.ChangeState(_followingState);
    }

    //public override void TakeDamage(float value, bool headShot)
    //{
    //    // Implementation for damage handling (if required)
    //}

    public override void Dead(bool headShot)
    {
        Alive = false;
        enabled = false;
        Animator.SetTrigger(ANIM_DEATH);
        onDead?.Invoke(this);

        int r = Random.Range(0, _deathClips.Length);
        _audioSource.PlayOneShot(_deathClips[r]);

        _collider.enabled = false;
        _rb.isKinematic = true;
        ServiceLocator.GetService<GameManager>().LoseGame();
    }

    protected override void ConstructTargets()
    {
        // Implementation for target construction (if required)
    }   

    public void ActiveTask()
    {
        activeTask = true;
        _outline.enabled = true; 
    }

    public void DeactiveTask()
    {
        activeTask = false;
        _outline.enabled = false;
    }
}
