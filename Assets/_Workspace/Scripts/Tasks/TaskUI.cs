using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    private TaskManager _taskManager;
    private AudioManager _audioManager; 

    private void Start()
    {
        _audioManager = ServiceLocator.GetService<AudioManager>();
        _taskManager = ServiceLocator.GetService<TaskManager>();
        _taskManager.onTaskUpdate += UpdateTask;
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
           _audioManager.PlayTaskWrite();
            yield return new WaitForSeconds(0.1f); 
        }
    }
    private void OnDestroy()
    {
        _taskManager.onTaskUpdate -= UpdateTask;
    }
}
