using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorCondition : TutorialCondition
{
    [SerializeField] private Enemy[] _enemy;
    [SerializeField] private GameObject _armorBtn;
    [SerializeField] private Joystick _moveJoysick;
    [SerializeField] private PlayerAbilities _playerAbilities;
    [SerializeField] private CameraUI _cameraUI; 

    private Canvas _canvas;
    private int _enemyKillCount; 
    public override void EnableCondition()
    {
        base.EnableCondition();

        for (int i = 0; i <_enemy.Length; i++)
        {
            _enemy[i].onDead += OnEnemyDead;
        }

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        _moveJoysick.gameObject.SetActive(true); 
        _playerAbilities.headShotChance = 100;
        _playerAbilities.invisibilityTime = 60;
        _playerAbilities.armorTime = 60;
        PlayerAbilities.onArmor += DisableUI; 
    }
    private void DisableUI(bool value)
    {
        _canvas.enabled = false;
        for (int i = 0; i < _enemy.Length; i++)
        {
            _enemy[i].EnterLureState(_player.transform.position); 
        }
        PlayerAbilities.onArmor -= DisableUI;
        _player.CanControll = true;
    }
    protected override void Ready()
    {
        base.Ready();
        _canvas.enabled = true;
        _cameraUI.Active();
        _armorBtn.gameObject.SetActive(true);
        _player.CanControll = false; 
    }
    private void OnEnemyDead(Character character)
    {
        character.onDead -= OnEnemyDead;
        _enemyKillCount++;
        if (_enemyKillCount == _enemy.Length)
            CompleteStep();
    }
}
