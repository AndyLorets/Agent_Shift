using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class WeaponLineRender : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private float _visibleRange;
    private float _viewAngle;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false; 
    }
    public void Construct(float viewAngle, float visibleRange)
    {
        _viewAngle = viewAngle;
        _visibleRange = visibleRange;
    }
    public void DrawLine(bool render, bool isDetected)
    {
        if (!render)
        {
            _lineRenderer.positionCount = 0;
            _lineRenderer.enabled = false; 
            return;
        }

        if (!_lineRenderer.enabled)
            _lineRenderer.enabled = true; 

        _lineRenderer.positionCount = 4;

        Vector3 leftBoundary = Quaternion.Euler(0, -_viewAngle / 2, 0) * transform.forward * _visibleRange;
        Vector3 rightBoundary = Quaternion.Euler(0, _viewAngle / 2, 0) * transform.forward * _visibleRange;

        _lineRenderer.startColor = isDetected ? Color.red : Color.yellow;
        _lineRenderer.endColor = isDetected ? Color.red : Color.yellow;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + leftBoundary);
        _lineRenderer.SetPosition(2, transform.position + rightBoundary);
        _lineRenderer.SetPosition(3, transform.position); 
    }
}
