using UnityEngine;
using System.Collections;

public class PuzzleWires : MonoBehaviour
{
    [SerializeField] private EnergyReceiver _energyReceiver;
    [SerializeField] private EnergySource _energySource;
    [SerializeField] private Wire[] _wires;

    private UIContentManager _UIContentManager;
    private AudioManager _audioManager;
    private CanvasGroup _canvasGroup; 

    public System.Action onComplete;  
    public Wire[] wires => _wires;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _UIContentManager = ServiceLocator.GetService<UIContentManager>();
        _audioManager = ServiceLocator.GetService<AudioManager>();
    }
    private void Start()
    {
        for (int i = 0; i < _wires.Length; i++)
        {
            _wires[i].Initialize(this, _energyReceiver, _energySource);
        }

        _energyReceiver.onActive += Complete; 
    }
    private void OnDestroy()
    {
        _energyReceiver.onActive -= Complete;
    }
    private void Complete()
    {
        _canvasGroup.interactable = false;
        _audioManager.PlayItem();
        onComplete?.Invoke();
        StartCoroutine(CloseAfterRealSeconds(1));
    }
    private IEnumerator CloseAfterRealSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        _UIContentManager.Close();
    }
}
