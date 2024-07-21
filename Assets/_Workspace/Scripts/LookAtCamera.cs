using UnityEngine;
public class LookAtCamera : MonoBehaviour
{
    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }
    void LateUpdate()
    {
        transform.LookAt(_camera.transform.position);
    }
}
