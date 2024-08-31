using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PatroolState : StateBase
{
    [SerializeField] private Transform _patroolTransform; 
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField, Range(3, 7)] private float _waitTime = 3f;
    
    private bool _isMove;

    private const float AGENT_MOVE_SPEED = 1.2f;

    private Enemy _enemy;

    protected override void Awake()
    {
        base.Awake();
        _patroolTransform.parent = transform.parent;
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

        _enemy.agent.speed = AGENT_MOVE_SPEED;

        SetMove();
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();

        _isMove = false;
        _enemy.agent.SetDestination(transform.position);
        _enemy.Animator.SetBool(ANIM_WALK, _isMove);
    }
    private void LookAtPlayer()
    {
        transform.DOLookAt(_enemy.player.transform.position, .5f);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    private void SetMove()
    {
        _enemy.agent.SetDestination(GetRandomPointInsideCube());
    }
    private void StopMove()
    {
        _enemy.agent.SetDestination(transform.position);

        StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(_waitTime);

        SetMove();
    }
 private Vector3 GetRandomPointInsideCube()
    {
        Vector3 randomPointLocal = new Vector3(
            Random.Range(-_cubeSize.x / 2, _cubeSize.x / 2),
            Random.Range(-_cubeSize.y / 2, _cubeSize.y / 2),
            Random.Range(-_cubeSize.z / 2, _cubeSize.z / 2)
        );
        Vector3 randomPointWorld = _patroolTransform.TransformPoint(randomPointLocal);
        return randomPointWorld;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawWireCube(Vector3.zero, _cubeSize);
    }
}