using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected int _hp;
    [SerializeField] protected int _currentHP;
    [Space(5), Header("Visible")]
    [SerializeField, Range(3f, 10f)] protected float _visibleRange = 3f;
    [SerializeField, Range(25f, 250f)] protected float _viewAngle = 45f;
    [SerializeField] protected LayerMask _detectionLayer;
    [Space(5), Header("Components")]
    [SerializeField] private Sprite _icon;
    [SerializeField] private Skin _skin;
    [SerializeField] protected AudioClip[] _damageClips;
    [SerializeField] protected AudioClip[] _deathClips;

    protected AudioSource _audioSource; 
    protected EnemyManager _enemyManager;
    protected List<Character> _targets = new List<Character>(); 
    public Sprite icon => _icon;
    public ITakeDamage takeDamage => this;
    public Skin Skin => _skin;
    public Animator Animator => _skin.animator;
    public bool Alive { get; private set; } = true;
    protected bool _enemyDetected;

    public System.Action<float, float, bool> onChangeHP;
    public System.Action<Character> onDead;

    protected const string ANIM_DAMAGE = "Damage";
    protected const string ANIM_DAMAGE_HEADSHOT = "Damage_HeadShot";
    protected const string ANIM_DEATH = "Death";
    protected const string ANIM_DEATH_HEADSHOT = "Death_HeadShot";

    private void Start()
    {
        Construct();
        ConstructTargets(); 
    }
    protected abstract void ConstructTargets(); 
    protected virtual void Construct()
    {
        _enemyManager = ServiceLocator.GetService<EnemyManager>();
        _audioSource = GetComponent<AudioSource>();
        _currentHP = _hp;
    }
    private bool CheckForEnemyInRange() => Physics.CheckSphere(transform.position, _visibleRange, _detectionLayer);
    protected Character FindNearestEnemy()
    {
        Character nearestCharacter = null;
        float minDistance = Mathf.Infinity;

        foreach (Character character in _targets)
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
    public virtual bool IsEnemyDetected(out Vector3 pos, out bool headshoot)
    {
        _enemyDetected = false;

        pos = Vector3.zero;
        headshoot = false;

        if (!CheckForEnemyInRange()) return false;

        Character nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return false;

        if(!IsLineClear(transform.position, nearestEnemy.transform.position) || !nearestEnemy.Alive) return false;

        pos = nearestEnemy.transform.position + Vector3.up;

        _enemyDetected = true; 

        return true;
    }
    public virtual bool IsEnemyDetected(out Vector3 pos, out ITakeDamage takeDamage)
    {
        _enemyDetected = false;

        pos = Vector3.zero;
        takeDamage = null;

        if (!CheckForEnemyInRange()) return false;

        Character nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return false;

        takeDamage = nearestEnemy.takeDamage;

        if (!IsLineClear(transform.position, nearestEnemy.transform.position) || !nearestEnemy.Alive) return false;

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

        int r = Random.Range(0, _damageClips.Length);
        _audioSource.PlayOneShot(_damageClips[r]); 
    }
    public virtual void Dead(bool headShot)
    {
        string anim = headShot ? ANIM_DEATH_HEADSHOT : ANIM_DEATH;
        Alive = false;
        enabled = false;
        Animator.SetTrigger(anim);
        onDead?.Invoke(this);

        int r = Random.Range(0, _deathClips.Length);
        _audioSource.PlayOneShot(_deathClips[r]);
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
