using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected int _hp;
    [SerializeField] protected int _currentHP;
    [Space(5), Header("Visible")]
    [SerializeField, Range(3f, 10f)] protected float _visibleRange = 3f;
    [SerializeField, Range(25f, 120f)] protected float _viewAngle = 45f;
    [SerializeField] protected LayerMask _detectionLayer;
    [Space(5), Header("Components")]
    [SerializeField] private Skin _skin;

    protected List <Character> _enemyList = new List<Character>();
    public ITakeDamage takeDamage => this;
    public Skin Skin => _skin;
    public Animator Animator => _skin.animator;

    protected bool _enemyDetected;

    public Action<string> onSendMessag; 

    private void Start()
    {
        Construct();
        ConstructEnemyList();
    }
    protected virtual void Construct()
    {
        _currentHP = _hp;
    }
    protected abstract void ConstructEnemyList(); 

    private bool CheckForEnemyInRange() => Physics.CheckSphere(transform.position, _visibleRange, _detectionLayer);
    protected Character FindNearestEnemy()
    {
        Character nearestCharacter = null;
        float minDistance = Mathf.Infinity;

        foreach (Character character in _enemyList)
        {
            float distance = Vector3.Distance(transform.position, character.transform.position);
            Vector3 directionToTarget = (character.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < _viewAngle / 2)
            {
                if (distance < minDistance && distance <= _visibleRange)
                {
                    minDistance = distance;
                    nearestCharacter = character;
                }
            }
        }

        return nearestCharacter;
    }
    public bool IsEnemyDetected()
    {
        if (!CheckForEnemyInRange()) return false;

        Character nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return false;
        if (!IsLineClear(transform.position, nearestEnemy.transform.position)) return false;

        return true;
    }
    public virtual bool IsEnemyDetected(out ITakeDamage takeDamage, out Vector3 pos)
    {
        _enemyDetected = false;

        takeDamage = null;
        pos = Vector3.zero;

        if (!CheckForEnemyInRange()) return false;

        Character nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return false;

        if(!IsLineClear(transform.position, nearestEnemy.transform.position)) return false;

        takeDamage = nearestEnemy.takeDamage;
        pos = nearestEnemy.transform.position + Vector3.up;

        _enemyDetected = true; 

        return true;
    }
    private bool IsLineClear (Vector3 pointA, Vector3 pointB)
    {
        pointA += Vector3.up;
        pointB += Vector3.up;

        RaycastHit hit;
        if (Physics.Linecast(pointA, pointB, out hit))
        {
            if (((1 << hit.collider.gameObject.layer) & _detectionLayer) != 0)
            {
                return true;
            }
        }
        return false;
    }
    public virtual void TakeDamage(int value)
    {
        _currentHP -= value;
        if (_currentHP <= 0)
            Dead();
    }
    public virtual void Dead()
    {
        gameObject.SetActive(false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _visibleRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -_viewAngle / 2, 0) * transform.forward * _visibleRange;
        Vector3 rightBoundary = Quaternion.Euler(0, _viewAngle / 2, 0) * transform.forward * _visibleRange;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}
