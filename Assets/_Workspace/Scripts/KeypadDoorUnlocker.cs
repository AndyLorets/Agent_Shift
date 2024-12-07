using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using TMPro;
using UnityEngine.Events;

public class KeypadDoorUnlocker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _findCodeText;
    [SerializeField] private Button[] _btns; 

    private string _enteredCode;
    private string _correctCode;

    private UnityAction _action;
    private UIContentManager _UIContentManager;
    private AudioManager _audioManager; 

    private const string ENTER_CODE = "Enter Code"; 
    private void Start()
    {
        Clear();
        _UIContentManager = ServiceLocator.GetService<UIContentManager>();
        _audioManager = ServiceLocator.GetService<AudioManager>();
    }
    private void OnEnable()
    {

    }
    public void Init(string correctCode, UnityAction action)
    {
        _correctCode = correctCode;
        _action = action;
        _findCodeText.text = Note.CodeFounded ? $"Found code: <color=yellow>{_correctCode}</color>" : "<color=yellow>Code needs to be found</color>";
    }

    public void SetNumber(int num)
    {
        if (_enteredCode == ENTER_CODE)
        {
            _enteredCode = "";
            _text.text = _enteredCode;
        }           

        _enteredCode += num;
        _text.text = _enteredCode;
        _audioManager.PlayKeypad();
    }
    public void Clear()
    {
        _enteredCode = ENTER_CODE;
        _text.text = _enteredCode;
    }
    public void Enter() => StartCoroutine(Apply());
    private IEnumerator Apply()
    {
        WaitForSecondsRealtime waitForSeconds = new WaitForSecondsRealtime(1);

        if (_correctCode == _enteredCode)
        {
            SetInteractableBtns(false);
            _text.text = "Success!";
            _audioManager.PlaySuccess();

            yield return waitForSeconds;

            _action.Invoke();
            Close();
        }
        else
        {
            SetInteractableBtns(false);
            _text.text = "ERROR!";
            _audioManager.PlayCancel();

            yield return waitForSeconds;

            SetInteractableBtns(true);
            Clear();
        }
    }
    private void SetInteractableBtns(bool value)
    {
        for (int i = 0; i < _btns.Length; i++)
        {
            _btns[i].interactable = value;
        }
    }
    public void Close() => _UIContentManager.Close();
}
