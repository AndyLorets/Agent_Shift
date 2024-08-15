using UnityEngine;

public class KillCondition : TutorialCondition
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private CharacterDialogue _dialogueOnFail;

    private const float FOOTSTEP_SPEED_RANGE = .8f;
    private Vector3 _playerStartPos;
 
    public override void EnableCondition()
    {
        base.EnableCondition();

        _enemy.onDead += EnemyDead;
        _playerStartPos = _player.transform.position;
        _player.Skin.onFootStep += Repeit;
        _enemy.enabled = false; 
    }
    private void EnemyDead(Character character)
    {
        _enemy.onDead -= EnemyDead;
        _player.Skin.onFootStep -= Repeit;
        CompleteStep();
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
