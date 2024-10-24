using DG.Tweening;
using UnityEngine;

public class UpgradeCondition : TutorialCondition
{
    [SerializeField] protected CharacterDialogue _dialogueOnOpenUpgrades;
    [SerializeField] private CanvasGroup _menuCanvas;
    [SerializeField] private CanvasGroup _upgradesCanvas;
    [SerializeField] private CanvasGroup _walletCanvas;
    [SerializeField] private Animation _animBtnContinue;
    [SerializeField] private PlayerAbilities _playerAbilities;

    private Canvas _canvas;

    public override void EnableCondition()
    {
        base.EnableCondition();

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        _menuCanvas.alpha = 1;
        _playerAbilities.CancelInvisibility();
        ServiceLocator.GetService<Wallet>().AddMoney(10); 
    }
    protected override void Ready()
    {
        base.Ready();
        _canvas.enabled = true;
        _menuCanvas.blocksRaycasts = true;
        _menuCanvas.interactable = true;
        _hud.Hide();
    }
    public void OpenUpgrades()
    {
        _canvas.enabled = false;
        _upgradesCanvas.interactable = false; 
        ServiceLocator.GetService<CharacterMessanger>().SetDialogue(_icon, _dialogueOnOpenUpgrades);
        CharacterMessanger.OnResetAudioPlaying += EnableContinueBtn; 
    }
    private void EnableContinueBtn()
    {
        _walletCanvas.alpha = 1; 
        _upgradesCanvas.interactable = true;
        _animBtnContinue.Play();
        CharacterMessanger.OnResetAudioPlaying -= EnableContinueBtn;
    }
    public void EndTutorial()
    {
        _upgradesCanvas.alpha = 0;
        _upgradesCanvas.interactable = false;
        _upgradesCanvas.blocksRaycasts = false; 
        CompleteStep(); 
    }
}
