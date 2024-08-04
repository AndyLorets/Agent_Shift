using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class Character : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected int _hp;
    [SerializeField] protected int _currentHP;
    [Space(5), Header("Visible")]
    [SerializeField, Range(3f, 10f)] protected float _visibleRange = 3f;
    [SerializeField, Range(25f, 180f)] protected float _viewAngle = 45f;
    [SerializeField] protected LayerMask _detectionLayer;
    [Space(5), Header("Components")]
    [SerializeField] private Skin _skin;
    [SerializeField] private Sprite _icon; 
    public Sprite icon => _icon;
    protected List <Character> _enemyList = new List<Character>();
    public ITakeDamage takeDamage => this;
    public Skin Skin => _skin;
    public Animator Animator => _skin.animator;
    [field : SerializeField] public bool Alive { get; private set; } = true; 
    protected bool _enemyDetected;

    public Action<float, float, bool> onChangeHP;
    public Action<Character> onDead;

    protected const string ANIM_DAMAGE = "Damage";
    protected const string ANIM_DAMAGE_HEADSHOT = "Damage_HeadShot";
    protected const string ANIM_DEATH = "Death";
    protected const string ANIM_DEATH_HEADSHOT = "Death_HeadShot";

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
        if (nearestEnemy == null || !nearestEnemy.Alive) return false;
        if (!IsLineClear(transform.position, nearestEnemy.transform.position)) return false;

        return true;
    }
    public virtual bool IsEnemyDetected(out ITakeDamage takeDamage, out Vector3 pos, out bool headshoot)
    {
        _enemyDetected = false;

        takeDamage = null;
        pos = Vector3.zero;
        headshoot = false;

        if (!CheckForEnemyInRange()) return false;

        Character nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return false;

        if(!IsLineClear(transform.position, nearestEnemy.transform.position) || !nearestEnemy.Alive) return false;

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
    public virtual void TakeDamage(int value, bool headShot)
    {
        string anim = headShot ? ANIM_DAMAGE_HEADSHOT : ANIM_DAMAGE;
        value = headShot ? value * 3 : value;

        onChangeHP?.Invoke(_currentHP, _hp, headShot);

        _currentHP -= value;
        if (_currentHP <= 0)
            Dead(headShot);
        else
            Animator.SetTrigger(anim);
    }
    public virtual void Dead(bool headShot)
    {
        string anim = headShot ? ANIM_DEATH_HEADSHOT : ANIM_DEATH;
        Alive = false;
        enabled = false;
        Animator.SetTrigger(anim);
        onDead?.Invoke(this);
    }
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _visibleRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -_viewAngle / 2, 0) * transform.forward * _visibleRange;
        Vector3 rightBoundary = Quaternion.Euler(0, _viewAngle / 2, 0) * transform.forward * _visibleRange;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}
