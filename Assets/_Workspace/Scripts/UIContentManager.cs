using DG.Tweening; 
using UnityEngine;
public class UIContentManager : MonoBehaviour
{
    private GameObject _content;
    private CanvasGroup _canvasGroup;
    private HUD _hud; 

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        _canvasGroup.alpha = 0; 
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _hud = ServiceLocator.GetService<HUD>();
    }
    public void Open(GameObject content)
    {
        _hud.Hide();
        _canvasGroup.DOFade(1, 1).SetUpdate(true);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true; 

        _content = content;
        _content.transform.SetParent(transform, false);
        _content.transform.localPosition = Vector3.zero;
        _content.transform.localScale = Vector3.one;
        _content.transform.localEulerAngles = Vector3.zero;

        Time.timeScale = 0;
    }
    public void Close()
    {
        _hud.Show();
        _canvasGroup.DOFade(0, 1).SetUpdate(true);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        Destroy(_content);

        Time.timeScale = 1;
    }
}
