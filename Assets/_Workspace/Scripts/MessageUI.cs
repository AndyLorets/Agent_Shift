using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private CanvasGroup _canvas;
    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
        ServiceLocator.RegisterService(this);
    }
    private void Start()
    {
        _canvas.DOFade(0, .3f);
    }
    public void ShowMessage(string value)
    {
        _canvas.DOFade(1, .3f);
        _text.text = value;

        CancelInvoke(nameof(Hide)); 
        Invoke(nameof(Hide), 2f); 
    }

    private void Hide()
    {
        _canvas.DOFade(0, .3f);
    }
}
