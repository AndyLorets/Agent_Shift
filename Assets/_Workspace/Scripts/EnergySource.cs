using UnityEngine;

public class EnergySource : MonoBehaviour
{
    [SerializeField] private Transform _outputPoint;
    public Transform outputPoint => _outputPoint;

    public const float DIST = 50f;

}
