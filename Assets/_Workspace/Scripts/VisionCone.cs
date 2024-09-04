using Unity.VisualScripting;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private LayerMask _obstructingLayer;
    [SerializeField] private int _resolution = 120;
    [Space(5)]
    [SerializeField] private Color _idleColor;
    [SerializeField] private Color _visibleColor;
    [SerializeField] private Color _attackColor;

    private float _range;
    private float _angle;
    private Mesh _mesh;
    private MeshFilter _meshFilter;

    public enum ColorType
    {
        Idle, Visible, Attack
    }
    public void Init(float range, float angle)
    {
        _material = Instantiate(_material);
        transform.AddComponent<MeshRenderer>().material = _material;
        _meshFilter = transform.AddComponent<MeshFilter>();
        _mesh = new Mesh();
        _range = range;
        _angle = angle;
        _angle *= Mathf.Deg2Rad;
    }

    public void SetColor(ColorType colorType)
    {
        Color endColor = Color.black; 
        switch(colorType)
        {
            case ColorType.Idle:
                endColor = _idleColor;
                break;
            case ColorType.Visible:
                endColor = _visibleColor;
                break;
            case ColorType.Attack:
                endColor = _attackColor;
                break;
        }
        _material.color = endColor;
    }

    void Update()
    {
        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(_resolution - 1) * 3];
        Vector3[] Vertices = new Vector3[_resolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -_angle / 2;
        float angleIcrement = _angle / (_resolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < _resolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, _range, _obstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;
            }
            else
            {
                Vertices[i + 1] = VertForward * _range;
            }


            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        _mesh.Clear();
        _mesh.vertices = Vertices;
        _mesh.triangles = triangles;
        _meshFilter.mesh = _mesh;
    }
}