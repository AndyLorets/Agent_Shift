using Cinemachine;
using UnityEngine;
[RequireComponent(typeof(CinemachineImpulseListener))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
    }
    public void InpulseCamera()
    {
        _impulseSource.GenerateImpulse();
    }
}
