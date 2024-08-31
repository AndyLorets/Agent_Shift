using UnityEngine;
public class IdleState : StateBase
{
    private enum AnimType 
    {
        Idle, Dance
    }
    [SerializeField] private AnimType _animType; 

    private int _idleAnimHash;
    private Animator _animator;

    private const string ANIM_IDLE = "Idle";
    private const string ANIM_DANCE = "Dance";

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Enemy>().Animator;
    }
    public override void EnterState()
    {
        base.EnterState();
        _idleAnimHash = Animator.StringToHash(GetAnimName());
        _animator.Play(_idleAnimHash);
    }
    private string GetAnimName()
    {
        switch (_animType)
        {
            case AnimType.Dance: return ANIM_DANCE;
            default: return ANIM_IDLE; 
        }
    }
}
