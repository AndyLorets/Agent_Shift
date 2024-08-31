using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Tutorial[] _tutorials;
    [SerializeField] protected CharacterDialogue _dialogueOnEnd;
    [SerializeField] private Sprite _icon; 
    private int _currentStep;

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
    }

    private void Start()
    {
        InitSteps(); 
        StartTutorial();
    }

    public void StartTutorial()
    {
        Invoke(nameof(ShowNextStep), 1f);
    }
    private void InitSteps()
    {
        for (int i = 0; i < _tutorials.Length; i++)
        {
            _tutorials[i].Init(); 
        }
    }
    private void ShowNextStep()
    {
        if (_currentStep < _tutorials.Length)
        {
            _tutorials[_currentStep].ShowStep(OnStepCompleted);
        }
        else
        {
            EndTutorial();
        }
    }

    private void OnStepCompleted()
    {
        _currentStep++;
        ShowNextStep();
    }

    private void EndTutorial()
    {
        CharacterMessanger characterMessanger = ServiceLocator.GetService<CharacterMessanger>();
        characterMessanger.SetDialogue(_icon, _dialogueOnEnd);
        CharacterMessanger.OnResetAudioPlaying += StartGameLevel;
        PlayerPrefs.SetInt("Tutorial", 1);
    }
    private void StartGameLevel()
    {
        CharacterMessanger.OnResetAudioPlaying -= StartGameLevel;
        ServiceLocator.GetService<SceneLoader>().LoadScene(1); 
    }
}

[System.Serializable]
public class Tutorial
{
    [SerializeField] private TutorialCondition _condition; 
    public void Init()
    {
        _condition.Initialize();
    }
    public void ShowStep(System.Action onStepCompleted)
    { 
        _condition.OnConditionMet += onStepCompleted;
        _condition.gameObject.SetActive(true); 
        _condition.EnableCondition();
    }
}

public abstract class TutorialCondition : MonoBehaviour
{
    public event System.Action OnConditionMet;

    [SerializeField] protected Sprite _icon;
    [SerializeField] protected CharacterDialogue _dialogueOnStart;
    [SerializeField] protected CharacterDialogue _dialogueOnEnd;
    [SerializeField] private Transform _playerPos;

    private Vector3 _playerPosition;
    private Quaternion _playerRotation; 

    protected Player _player;
    protected HUD _hud;
    protected CharacterMessanger _characterMessanger;

    protected bool _isComplate;
    public void Initialize()
    {
        _playerPosition = _playerPos.transform.position;
        _playerRotation = _playerPos.transform.rotation; 
        gameObject.SetActive(false);
    }
    public virtual void EnableCondition()
    {
        _hud = ServiceLocator.GetService<HUD>();
        _player = ServiceLocator.GetService<Player>();  
        _characterMessanger = ServiceLocator.GetService<CharacterMessanger>();
        _characterMessanger.SetDialogue(_icon, _dialogueOnStart); 
        CharacterMessanger.OnResetAudioPlaying += Ready;

        _hud.Hide();
        _player.CanControll = false;
        _player.transform.position = _playerPosition;
        _player.transform.rotation = _playerRotation;
    }
    protected virtual void Ready()
    {
        _hud.Show();
        _player.CanControll = true;
        CharacterMessanger.OnResetAudioPlaying -= Ready;
    }
    protected void CompleteStep()
    {
        if (_isComplate) return; 

        _isComplate = true;
        _characterMessanger.SetDialogue(_icon, _dialogueOnEnd);
        CharacterMessanger.OnResetAudioPlaying += ConditionMet;
        _hud.Hide();
        _player.CanControll = false;
    }
    private void ConditionMet()
    {
        OnConditionMet?.Invoke();
        CharacterMessanger.OnResetAudioPlaying -= ConditionMet;
        gameObject.SetActive(false);
    }
}
