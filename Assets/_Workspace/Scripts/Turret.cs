using System.Threading;
using UnityEngine;

public class Turret : WeaponBase
{
    [SerializeField] private MotionSensor[] _motionSensors; 
    [SerializeField] private Transform _head;

    private Character _target;
    private bool _isActive;
    private float _fireDelay;
    private float _currentShootTime;
    private int _currentShootCount;

    private void Awake()
    {
        for (int i = 0; i < _motionSensors.Length; i++)
        {
            _motionSensors[i].onSensorEnter += Active;
            _motionSensors[i].onSensorExit += Deactive; 
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < _motionSensors.Length; i++)
        {
            _motionSensors[i].onSensorEnter -= Active;
            _motionSensors[i].onSensorExit -= Deactive;
        }
    }
    private void Shooting()
    {
        _currentShootTime -= Time.deltaTime;
        _fireDelay -= Time.deltaTime;
        HeadRotation();

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
    private void HeadRotation()
    {
        float rotationSpeed = 10.0f;
        Vector3 targetDirection = (_target.transform.position - _head.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        _head.rotation = Quaternion.Slerp(_head.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        _head.eulerAngles = new Vector3(0, _head.eulerAngles.y, 0);
    }
    private void Active(Character character)
    {
        _isActive = true;
        _target = character;
    }
    private void Deactive()
    {
        _isActive = false;
        _target = null;
        _currentShootTime = _shootDelay;
    }

    protected override void Update()
    {
        if (!_isActive) return;

        base.Update();
        Shooting(); 
    }
}
