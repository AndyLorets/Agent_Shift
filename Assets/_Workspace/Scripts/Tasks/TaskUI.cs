using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TaskManager _taskManager;
    [SerializeField] AudioSource _audioSource; 
    private void OnEnable()
    {
        _taskManager.onTaskUpdate += UpdateTask; 
    }
    private void OnDisable()
    {
        _taskManager.onTaskUpdate -= UpdateTask;
    }
    private void UpdateTask(string text)
    {
        string txt = $"• {text}";
        StartCoroutine(TypeText(txt));
    }
    private IEnumerator TypeText(string text)
    {
        _text.text = ""; 
        foreach (char letter in text)
        {
            _text.text += letter;
            _audioSource.Play(); 
            yield return new WaitForSeconds(0.1f); 
        }
    }
}
