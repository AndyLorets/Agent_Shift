using UnityEngine;

public abstract class StateBase : MonoBehaviour
{
    protected Enemy _enemy;

    protected const string ANIM_MOVE = "Move";
    protected const string ANIM_RUN = "Run";
    protected const string ANIM_SHOOT = "Shoot";
    protected const string ANIM_AIM = "Aim";

    protected virtual void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    protected virtual void Start()
    {

    }
    public virtual void EnterState()
    {
        enabled = true; 
    }
    public virtual void ExitState()
    {
        enabled = false;
    }
}
