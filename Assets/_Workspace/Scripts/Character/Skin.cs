using UnityEngine;

public class Skin : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _footStepSource;
    [SerializeField] private AudioClip[] _footStepClips; 
    public Animator animator => _animator;
    public System.Action onShoot;
    public System.Action<Vector3> onFootStep;
    public void Shoot()
    {
        onShoot?.Invoke();
    }
    public void FootStep()
    {
        if (_animator.GetFloat("Vertical") >= .9f || _animator.GetFloat("Vertical") <= -.9f)
        {
            onFootStep?.Invoke(transform.position);
            _footStepSource.PlayOneShot(_footStepClips[Random.Range(0, _footStepClips.Length)]);
        }
    }
}
