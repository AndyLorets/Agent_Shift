using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class CharacterMessanger : MonoBehaviour
{
    [SerializeField] private Character _character;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.enabled = false;  
    }

    private void OnEnable()
    {
        _character.onSendMessag += OnSendMessage; 
    }
    private void OnDisable()
    {
        _character.onSendMessag -= OnSendMessage;
    }
    private void OnSendMessage(string text)
    {
        _text.enabled = true;
        _text.text = text;

        CancelInvoke(nameof(ClearText)); 
        Invoke(nameof(ClearText), 3f); 
    }

    private void ClearText()
    {
        _text.text = "";
        _text.enabled = false;   
    }
}
