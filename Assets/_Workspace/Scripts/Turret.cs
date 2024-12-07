using UnityEngine;

public class Turret : WeaponBase
{
    [SerializeField] private VisionCone _visionCone;  
    [SerializeField] private Transform _head;
    [Space(5), Header("Visible")]
    [SerializeField, Range(3f, 15f)] protected float _visibleRange = 3f;
    [SerializeField, Range(25f, 250f)] protected float _viewAngle = 45f;
    [SerializeField] protected LayerMask _detectionLayer;

    private Player _target;
    private bool _isActive = true;
    private float _fireDelay;
    private float _currentShootTime;
    private int _currentShootCount;
    private bool EnemyIsDetected => IsTargetDetected() && !_target.IsInvisibility;
    private void Awake()
    {
        _visionCone.Init(_visibleRange, _viewAngle);
        _target = FindObjectOfType<Player>();
    }

    protected override void Update()
    {
        if (!_isActive) return;

        base.Update();

        if (EnemyIsDetected)
            Shooting();
    }
    private void Deactive()
    {
        _isActive = false;
        _currentShootTime = 0;
    }
    private void HeadRotation(Vector3 pos)
    {
        float rotationSpeed = 10.0f;
        Vector3 targetDirection = (pos - _head.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        _head.rotation = Quaternion.Slerp(_head.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        _head.eulerAngles = new Vector3(0, _head.eulerAngles.y, 0);
    }
    private bool CheckForTargetInRange() => Physics.CheckSphere(_head.position, _visibleRange, _detectionLayer);
    private bool IsTargetDetected()
    {
        if (!_target.Alive)
            return false;
        if (!CheckForTargetInRange()) 
            return false;

        float distance = Vector3.Distance(_head.position, _target.transform.position);
        Vector3 directionToTarget = (_target.transform.position - _head.position).normalized;
        float angleToTarget = Vector3.Angle(_head.forward, directionToTarget);
        if (angleToTarget > _viewAngle / 2)
            return false;

        if (!IsLineClear(_head.position, _target.transform.position))
            return false;

        return true;
    }
    private bool IsLineClear(Vector3 pointA, Vector3 pointB)
    {
        pointA += Vector3.up;
        pointB += Vector3.up;
#if UNITY_EDITOR
        Debug.DrawLine(pointA, pointB, Color.red);
#endif
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
    private void Shooting()
    {
        _currentShootTime -= Time.deltaTime;
        _fireDelay -= Time.deltaTime;
        HeadRotation(_target.transform.position);

        if (_currentShootTime <= 0 && !isReloading && _fireDelay <= 0)
        {
            Shoot(_target.transform.position, false);
            _fireDelay = .3f;
            _currentShootCount--;

            if (_currentShootCount <= 0)
            {
                _currentShootTime = _shootDelay;
                _currentShootCount = _shootCount;
            }
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_head.position, _visibleRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -_viewAngle / 2, 0) * _head.forward * _visibleRange;
        Vector3 rightBoundary = Quaternion.Euler(0, _viewAngle / 2, 0) * _head.forward * _visibleRange;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_head.position, leftBoundary);
        Gizmos.DrawRay(_head.position, rightBoundary);
    }
}
