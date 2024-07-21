using System.Collections;
using UnityEngine;

public class PatroolState : StateBase
{
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField, Range(3, 7)] private float _waitTime = 3f;
    [SerializeField, Range(1, 3)] private float _visibleReactionDuration = 2;

    [SerializeField] private bool _isMove;
    [SerializeField] private int _visibleCount; 

    public System.Action onPlayerVisible;

    private const float visible_reaction_duration = 1f;
    private const float AGENT_MOVE_SPEED = 1.2f;

    private Vector3 _startPos;
    protected override void Start()
    {
        base.Start(); 
        _startPos = transform.position;
    }
    private void Update()
    {
        _isMove = _enemy.agent.velocity.sqrMagnitude > 0;
        _enemy.Animator.SetBool(ANIM_MOVE, _isMove);

        if (_enemy.agent.remainingDistance <= .5f && _isMove)
            StopMove();
    }
    public override void EnterState()
    {
        base.EnterState();

        _visibleCount = 0;
        _enemy.agent.speed = AGENT_MOVE_SPEED;

        SetMove();
        StartCoroutine(CheckPlayer());
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();

        _isMove = false;
        _enemy.Animator.SetBool(ANIM_MOVE, _isMove);
    }
    private void SetMove()
    {
        if (_visibleCount > 0) return; 

        _enemy.agent.SetDestination(GetRandomPointInsideCube());
    }
    private void StopMove()
    {
        if (_visibleCount > 0) return;
 
        _enemy.agent.SetDestination(transform.position);

        StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(_waitTime);

        while (_visibleCount > 0)
            yield return null;

        SetMove();
    }
    private IEnumerator CheckPlayer()
    {
        while (enabled)
        {
            if (_enemy.IsEnemyDetected())
            {
                StopMove();
                _visibleCount++;

                if (_visibleCount == 1)
                    _enemy.onSendMessag?.Invoke("?"); 
            }
            else
                _visibleCount = 0;

            if (_visibleCount >= _visibleReactionDuration)
            {
                onPlayerVisible?.Invoke();
            }

            yield return new WaitForSeconds(visible_reaction_duration);

        }
    }
    private Vector3 GetRandomPointInsideCube()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-_cubeSize.x / 2, _cubeSize.x / 2),
            Random.Range(-_cubeSize.y / 2, _cubeSize.y / 2),
            Random.Range(-_cubeSize.z / 2, _cubeSize.z / 2)
        );

        return _startPos + randomPoint;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _cubeSize);
    }
}
