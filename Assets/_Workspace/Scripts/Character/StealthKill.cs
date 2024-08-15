using DG.Tweening;
using TMPro.Examples;
using UnityEngine;

public class StealthKill : MonoBehaviour
{
    [SerializeField] private float _sphereRadius = .3f; 
    [SerializeField] private float _detectionRange = 1f; 
    [SerializeField] private float _angle = 90f; 
    [SerializeField] private LayerMask _enemyLayer;

    private AudioManager _audioManager; 
    private CameraController _cameraController; 
    private Player _player;
    private Skin _skin;
    private ITakeDamage _targetTakeDamage;
    private bool _isKilling;

    private const string ANIM_KICK = "Kick";
    private const float FOOTSTEP_SPEED_RANGE = .8f;

    void Start()
    {
        _cameraController = ServiceLocator.GetService<CameraController>();
        _audioManager = ServiceLocator.GetService<AudioManager>();  
        _player = GetComponent<Player>();
        _skin = _player.Skin;
        _skin.onKick += Kill;
    }

    private void OnDestroy()
    {
        _skin.onKick -= Kill;
    }

    void Update()
    {
        if (!_player.Alive || _isKilling) return; 

        if (_player.Animator.GetFloat("Vertical") >= FOOTSTEP_SPEED_RANGE || _player.Animator.GetFloat("Vertical") <= -FOOTSTEP_SPEED_RANGE) return;

        PerformStealthKill();
    }

    private void PerformStealthKill()
    {
        if (IsEnemyBehind(out _targetTakeDamage))
        {
            _isKilling = true;
            _skin.animator.SetTrigger(ANIM_KICK);
        }
    }

    private bool IsEnemyBehind(out ITakeDamage targetTakeDamage)
    {
        Vector3 sphereCenter = transform.position;
        RaycastHit hit;
        targetTakeDamage = null;

        if (Physics.SphereCast(sphereCenter, _sphereRadius, transform.forward, out hit, _detectionRange, _enemyLayer, QueryTriggerInteraction.UseGlobal))
        {
            Vector3 directionToEnemy = hit.transform.position - transform.position;
            float angleToEnemy = Vector3.Angle(directionToEnemy, transform.forward);
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (angleToEnemy < _angle / 2 && !enemy.IsEnemyDetected())
            {
                targetTakeDamage = enemy.takeDamage;
                enemy.ExitAllState();

                transform.DOMove(hit.point - transform.forward * 1.35f, .2f);
                transform.LookAt(hit.point);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                _player.CanControll = false;
                return true;
            }
        }
        return false;
    }

    private void Kill()
    {
        if (_targetTakeDamage != null)
        {
            _targetTakeDamage.TakeDamage(1000, false);
            _targetTakeDamage = null;
            _cameraController.InpulseCamera();
            _audioManager.PlayKick(); 
        }
        _isKilling = false;
        _player.CanControll = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * _detectionRange);
        Gizmos.DrawWireSphere(transform.position + transform.forward * _detectionRange, _sphereRadius);
    }
}
