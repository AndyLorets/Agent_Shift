using UnityEngine;
using System.Collections;

public class PuzzleWires : MonoBehaviour
{
    [SerializeField] private EnergyReceiver _energyReceiver;
    [SerializeField] private EnergySource _energySource;
    [SerializeField] private Wire[] _wires;

    private UIContentManager _UIContentManager;
    private AudioManager _audioManager;

    private const string REWARD_NAME = "SkipPuzzleReward";

    public System.Action onComplete;  
    public Wire[] wires => _wires;

    private void Awake()
    {
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
        _audioManager.PlayItem();
        onComplete?.Invoke();
        StartCoroutine(CloseAfterRealSeconds(1));

        for (int i = 0; i < wires.Length; i++)
        {
            wires[i].SetCanRotate(false);
        }
    }
    private IEnumerator CloseAfterRealSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        _UIContentManager.Close();
    }
    public void SkipAd()
    {
        AdManager.ShowReward(REWARD_NAME);
        AdManager.OnRewardShowed += OnRewardComplate; 
    }
    private void OnRewardComplate(string name, bool compalte)
    {
        if (name == REWARD_NAME && compalte)
            Complete(); 
    }
}
