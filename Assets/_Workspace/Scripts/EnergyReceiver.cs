using UnityEngine;
using UnityEngine.UI;
public class EnergyReceiver : MonoBehaviour
{
    [SerializeField] private Transform _inputPoint;
    [SerializeField] private bool _isActivated = false;
    [Space(5)]
    [SerializeField] private Image[] _imageEnergy;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _deactiveColor;

    public System.Action onActive; 
    public Transform inputPoint => _inputPoint;
    private void Start()
    {
        Deactive();
    }
    public void Activate()
    {
        if (!_isActivated)
        {
            _isActivated = true;
            for (int i = 0; i < _imageEnergy.Length; i++)
            {
                _imageEnergy[i].color = _activeColor; 
            }

            onActive?.Invoke();
        }
    }
    public void Deactive()
    {
        _isActivated = false;
        for (int i = 0; i < _imageEnergy.Length; i++)
        {
            _imageEnergy[i].color = _deactiveColor;
        }
    }
}
