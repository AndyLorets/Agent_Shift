using TMPro; 
using UnityEngine;

public class WalletUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text; 

    private CanvasGroup _canvasGroup;
    private Wallet _wallet; 
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _wallet = ServiceLocator.GetService<Wallet>();

        _wallet.onMoneyChanged += UpdateUI;
        BriefingManager.onStartBriefing += Hide;
        PauseManager.onPause += SetVisible;
    }
    private void OnDestroy()
    {
        _wallet.onMoneyChanged -= UpdateUI;
        BriefingManager.onStartBriefing -= Hide;
        PauseManager.onPause -= SetVisible;
    }
    private void UpdateUI()
    {
        _text.text = _wallet.CurrentMoney.ToString(); 
    }
    private void Hide()
    {
        _canvasGroup.alpha = 0; 
    }
    private void Show()
    {
        _canvasGroup.alpha = 1;
    }
    private void SetVisible(bool value)
    {
        if (value) Show();
        else Hide(); 
    }
}
