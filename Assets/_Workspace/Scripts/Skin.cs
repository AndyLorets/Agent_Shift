using UnityEngine;

public class Skin : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _footStepSource;
    [SerializeField] private AudioClip[] _footStepClips; 
    public Animator animator => _animator;
    public System.Action onShoot;
    public System.Action<Vector3> onFootStep;
    public System.Action onKick;

    private const float FOOTSTEP_SPEED_RANGE = .7f; 

    public void Shoot()
    {
        onShoot?.Invoke();
    }
    public void FootStep()
    {
        if (_footStepSource == null) return; 

        if (_animator.GetFloat("Vertical") >= FOOTSTEP_SPEED_RANGE || _animator.GetFloat("Vertical") <= -FOOTSTEP_SPEED_RANGE)
        {
            onFootStep?.Invoke(transform.position);
            _footStepSource.PlayOneShot(_footStepClips[Random.Range(0, _footStepClips.Length)]);
        }
    }
    public void Kick()
    {
        onKick?.Invoke();
    }
}
