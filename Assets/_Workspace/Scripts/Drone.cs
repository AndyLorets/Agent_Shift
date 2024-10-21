using UnityEngine;
using DG.Tweening; 

public class Drone : MonoBehaviour
{
    [SerializeField] Enemy _enemy; 
    [SerializeField] private Transform _gripper;

    private LineRenderer _lr;
    private AudioSource _audioSource;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _enemy.onDead += ApproachTarget;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        RenderLR();
    }
    private void ApproachTarget(Character character)
    {
        transform.parent = null; 
        gameObject.SetActive(true);
        transform.DOLocalMoveY(5, 1)
            .OnComplete(() => EngageGripper()); 
    }
    private void EngageGripper()
    {
        _audioSource.Play();
        _gripper.DOLocalMoveY(-3.5f, 1f).OnComplete(delegate() 
        {
            _audioSource.Play();
            _enemy.Animator.SetTrigger("Ragdoll");
            _enemy.transform.parent = _gripper.transform;
            _gripper.DOLocalMoveY(-1f, 1f).OnComplete(() => DepartWithTarget());
        });
    }
    private void DepartWithTarget()
    {
        transform.DOLocalMoveY(15, 1)
            .OnComplete(() => gameObject.SetActive(false));
    }
    private void RenderLR()
    {
        _lr.positionCount = 2;
        _lr.SetPosition(0, transform.position);
        _lr.SetPosition(1, _gripper.transform.position);
    }
    private void OnDestroy()
    {
        _enemy.onDead -= ApproachTarget;
    }
}
