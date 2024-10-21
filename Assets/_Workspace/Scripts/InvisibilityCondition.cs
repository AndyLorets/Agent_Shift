using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityCondition : TutorialCondition
{
    [SerializeField] private GameObject _invisibilityBtn;
    [SerializeField] private CharacterDialogue _onActiveInvisibilityDialogue; 
    [SerializeField] private GameObject _point;
    [SerializeField] private PlayerAbilities _playerAbilities;

    private Canvas _canvas;
    public override void EnableCondition()
    {
        base.EnableCondition();

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        PlayerAbilities.onInvisibility += DisableUI;
        _playerAbilities.CancelArmor();
    }
    private void DisableUI(bool value)
    {
        _canvas.enabled = false;
        PlayerAbilities.onInvisibility -= DisableUI;
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_icon, _onActiveInvisibilityDialogue);
        CharacterMessanger.OnResetAudioPlaying += ActivePlayerControll;
        _point.SetActive(true); 
    }
    private void ActivePlayerControll()
    {
        CharacterMessanger.OnResetAudioPlaying -= ActivePlayerControll;
        _player.CanControll = true;
    }
    protected override void Ready()
    {
        base.Ready();
        _canvas.enabled = true;
        _player.CanControll = false;
        _invisibilityBtn.gameObject.SetActive(true);
    }

    private void Update()
    {
        float dist = Vector3.Distance(_player.transform.position, _point.transform.position);
        if (dist <= 2.5f && _point.activeSelf)
        {
            CompleteStep();
            _point.SetActive(false);
        }
    }
}
