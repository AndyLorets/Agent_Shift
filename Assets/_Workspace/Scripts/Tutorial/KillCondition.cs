using UnityEngine;

public class KillCondition : TutorialCondition
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private CharacterDialogue _dialogueOnFail;
    [SerializeField] private Joystick _aimJoysick;
    [SerializeField] private Joystick _moveJoysick;

    private Vector3 _playerStartPos;
    private Canvas _canvas;
    private bool _ready;

    public override void EnableCondition()
    {
        base.EnableCondition();

        _enemy.onDead += EnemyDead;
        _playerStartPos = _player.transform.position;
        _player.Skin.onFootStep += Repeit;
        _enemy.enabled = false;
        _aimJoysick.gameObject.SetActive(false);

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
    }
    protected override void Ready()
    {
        base.Ready();
        _ready = true;
        _canvas.enabled = true;
    }
    private void Update()
    {
        if (!_ready) return;

        if (_moveJoysick.Vertical == 0 && _moveJoysick.Horizontal == 0)
        {
            if (!_canvas.enabled)
            {
                _canvas.enabled = true;
            }
        }
        else
        {
            if (_canvas.enabled)
            {
                _canvas.enabled = false;
            }
        }
    }
    private void EnemyDead(Character character)
    {
        _enemy.onDead -= EnemyDead;
        _player.Skin.onFootStep -= Repeit;
        CompleteStep();
        _ready = false;
        _canvas.enabled = false;
    }

    private void Repeit(Vector3 value)
    {
        _characterMessanger.SetDialogue(_icon, _dialogueOnFail);
        _gamePlayUI.Hide();
        _player.CanControll = false; 
        _player.transform.position = _playerStartPos;
        CharacterMessanger.OnResetAudioPlaying += Ready;
    }
}
