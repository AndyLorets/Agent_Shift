using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PatroolState : StateBase
{
    [SerializeField] private PatroolType _patroolType;
    [SerializeField, Range(3, 7)] private float _waitTime = 3f;
    [Space(5)]
    [SerializeField] private Transform _patroolTransform; 
    [SerializeField] private Vector3 _cubeSize;
    [SerializeField] private Vector3[] _points;
    [Space(5)]
    [SerializeField] private bool _drawGizmos = true; 
    private int _currentPoint; 
    public enum PatroolType
    {
        Random, Points
    }

    private bool _isMove;

    private const float AGENT_MOVE_SPEED = 1.2f;

    private Enemy _enemy;

    protected override void Awake()
    {
        base.Awake();

        _enemy = GetComponent<Enemy>();

        if (_patroolType == PatroolType.Random)
            _patroolTransform.parent = transform.parent;
        if (_patroolType == PatroolType.Points)
            ConstructPoints();

        _drawGizmos = false;
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

        _enemy.agent.SetDestination(transform.position);
        _isMove = false;
        _enemy.Animator.SetBool(ANIM_WALK, _isMove);
    }
    private void SetMove()
    {
        if (_patroolType == PatroolType.Random)
        {
            _enemy.agent.SetDestination(GetRandomPointInsideCube());
        }
        if (_patroolType == PatroolType.Points)
        {
            if (_currentPoint < _points.Length - 1)
                _currentPoint++;
            else
                _currentPoint = 0;

            _enemy.agent.SetDestination(_points[_currentPoint]);
        }
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
    private void ConstructPoints()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] += transform.position;  
        }
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
        if (!_drawGizmos) return; 

        Gizmos.color = Color.cyan;

        if (_patroolType == PatroolType.Random)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.DrawWireCube(Vector3.zero, _cubeSize);
        }
        if (_patroolType == PatroolType.Points)
        {
            for (int i = 0; i < _points.Length; i++)
            {
                if (i < _points.Length - 1)
                    Gizmos.DrawLine(transform.position + _points[i], transform.position + _points[i + 1]); 
                Gizmos.DrawSphere(transform.position + _points[i], .1f); 
            }
        }
    }
}
