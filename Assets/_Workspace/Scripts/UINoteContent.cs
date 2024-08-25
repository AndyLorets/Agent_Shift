using TMPro;
using UnityEngine;

public class UINoteContent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private UIContentManager _UIContentManager;
    private void Start()
    {
        _UIContentManager = ServiceLocator.GetService<UIContentManager>();
    }
    public void Init(string value)
    {
        _text.text = value; 
    }
    public void Close() => _UIContentManager.Close();
}
