using Cinemachine;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Tutorial[] _tutorials;

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
        // Логика завершения туториала (например, разблокировка основного геймплея)
    }
}

[System.Serializable]
public class Tutorial
{
    [SerializeField] private TutorialCondition _condition; 
    public void Init()
    {
        _condition.gameObject.SetActive(false);
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

    protected Player _player;
    protected GamePlayUI _gamePlayUI;
    protected CharacterMessanger _characterMessanger;

    protected bool _isComplate; 
    public virtual void EnableCondition()
    {
        _gamePlayUI = ServiceLocator.GetService<GamePlayUI>();
        _player = ServiceLocator.GetService<Player>();  
        _characterMessanger = ServiceLocator.GetService<CharacterMessanger>();
        _characterMessanger.SetDialogue(_icon, _dialogueOnStart); 
        CharacterMessanger.OnResetAudioPlaying += Ready;

        _gamePlayUI.Hide();
        _player.CanControll = false; 
    }
    protected virtual void Ready()
    {
        _gamePlayUI.Show();
        _player.CanControll = true;
        CharacterMessanger.OnResetAudioPlaying -= Ready;
    }
    protected void CompleteStep()
    {
        _isComplate = true;
        _characterMessanger.SetDialogue(_icon, _dialogueOnEnd);
        CharacterMessanger.OnResetAudioPlaying += ConditionMet;
        _gamePlayUI.Hide();
        _player.CanControll = false;
    }
    private void ConditionMet()
    {
        if (!_isComplate) return; 

        OnConditionMet?.Invoke();
        CharacterMessanger.OnResetAudioPlaying -= ConditionMet;
        gameObject.SetActive(false);
    }
}
