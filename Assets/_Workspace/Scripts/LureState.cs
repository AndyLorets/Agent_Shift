using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LureState : StateBase
{
    [SerializeField, Range(3, 7)] private float _waitTime = 3f;

    private bool _isMove;

    public System.Action onPlayerUnvisible;

    private const float AGENT_MOVE_SPEED = 1.2f;

    private Enemy _enemy;
    private Vector3 _lurePoint;

    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        _isMove = _enemy.agent.velocity.sqrMagnitude > 0;
        _enemy.Animator.SetBool(ANIM_WALK, _isMove);

        if (_enemy.agent.remainingDistance <= .5f && _isMove)
            StopMove();
    }
    public override void EnterState()
    {
        base.EnterState();

        _enemy.agent.SetDestination(transform.position);
        _enemy.agent.speed = AGENT_MOVE_SPEED;

        Invoke(nameof(SetMove), 1f);
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();

        _isMove = false;
        _enemy.Animator.SetBool(ANIM_WALK, _isMove);
        _enemy.agent.SetDestination(transform.position);
    }
    private void SetMove()
    {
        if (_enemy.Alive)
            _enemy.agent.SetDestination(_lurePoint);
    }
    private void StopMove()
    {
        _enemy.agent.SetDestination(transform.position);

        StartCoroutine(Waiting());
    }
    private void LookAtPlayer()
    {
        transform.DOLookAt(_enemy.player.transform.position, .5f);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(_waitTime);

        onPlayerUnvisible?.Invoke();  
    }
 
    public void SetLurePoint(Vector3 pos) => _lurePoint = pos;
}
  

