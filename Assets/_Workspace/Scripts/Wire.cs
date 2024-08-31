using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; 
public class Wire : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool _canRotate = true;
    [Space(5)]
    [SerializeField] private Transform _inputPoint;
    [SerializeField] private Transform _outputPoint;
    [Space(5)]
    [SerializeField] private Sprite _cantRotateSprite;
    [SerializeField] private Image[] _imageEnergy;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _deActiveColor;

    private bool _hasEnergy = false;
    private bool _isRotating; 

    private PuzzleWires _puzzleWires;
    private EnergyReceiver _energyReceiver;
    private EnergySource _energySource;
    private AudioManager _audioManager; 
    public Transform inputPoint => _inputPoint;
    public Transform outputPoint => _outputPoint;


    private void Start()
    {
        SetCanRotate(_canRotate);
        Deactive();
        _audioManager = ServiceLocator.GetService<AudioManager>();
    }

    public void Initialize(PuzzleWires puzzleWires, EnergyReceiver energyReceiver, EnergySource energySource)
    {
        _puzzleWires = puzzleWires;
        _energyReceiver = energyReceiver;
        _energySource = energySource; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RotateWire();
    }
    public void SetCanRotate(bool value)
    {
        _canRotate = value;
        if (!_canRotate)
            GetComponent<Image>().sprite = _cantRotateSprite;
    }
    private void RotateWire()
    {
        if (!_canRotate || _isRotating) return;

        _isRotating = true; 
        transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z - 90), .2f)
            .SetUpdate(true)
            .OnComplete(delegate ()
            {
                foreach (Wire wire in _puzzleWires.wires)
                {
                    wire.CheckEnergySource();
                }
                _isRotating = false; 
            });

        _audioManager.PlayKeypad();
    }

    private void CheckEnergySource()
    {
        if (FindEnergySourceAtPosition() || FindEnergyWireAtPosition())
        {
            Active();
        }
        else
        {
            Deactive();
        }

        SendEnergy();
    }

    private void Active()
    {
        _hasEnergy = true;
        for (int i = 0; i < _imageEnergy.Length; i++)
        {
            _imageEnergy[i].color = _activeColor;
        }

    }

    private void Deactive()
    {
        _hasEnergy = false;
        for (int i = 0; i < _imageEnergy.Length; i++)
        {
            _imageEnergy[i].color = _deActiveColor;
        }
    }

    private void SendEnergy()
    {
        if (!_hasEnergy) return;

        if (FindReceiverAtPosition())
        {
            _energyReceiver.Activate();
        }
        else
        {
            _energyReceiver.Deactive();
        }
    }

    private bool FindEnergySourceAtPosition()
    {
        if (_puzzleWires == null) return false; 

        if (Vector3.Distance(_energySource.outputPoint.position, _inputPoint.position) < EnergySource.DIST)
        {
            return true;
        }
        return false;
    }

    private bool FindEnergyWireAtPosition()
    {
        foreach (Wire wire in _puzzleWires.wires)
        {
            if (Vector3.Distance(wire.outputPoint.position, _inputPoint.position) < EnergySource.DIST)
            {
                return wire._hasEnergy;
            }
        }
        return false;
    }

    private bool FindReceiverAtPosition()
    {
        if (Vector3.Distance(_energyReceiver.inputPoint.position, _outputPoint.position) < EnergySource.DIST)
        {
            return true;
        }
        return false;
    }
}
