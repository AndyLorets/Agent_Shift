using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMessanger : MonoBehaviour
{
    [SerializeField] private Image _icon; 
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private AudioSource _audioSource;

    private CanvasGroup _canvasGroup;

    public static CharacterMessanger instance;
    private const float tween_duration = .3f;

    public static Action OnResetAudioPlaying; 

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);  

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0; 
    }
    public void SetDialogueMessage(Sprite sprite, string text, AudioClip audioClip)
    {
        if (audioClip != null && !CharacterDialogue.speaking)
        {
            _audioSource.PlayOneShot(audioClip);
            _icon.sprite = sprite; 
            _text.text = text;

            _canvasGroup.DOFade(1, tween_duration); 

            CharacterDialogue.speaking = true;
            StartCoroutine(ResetAudioPlaying(audioClip));
        }
    }
    private IEnumerator ResetAudioPlaying(AudioClip audioClip)
    {
        yield return new WaitForSeconds(audioClip.length);
        CharacterDialogue.speaking = false;
        ClearText();
        OnResetAudioPlaying?.Invoke(); 
    }
    private void ClearText()
    {
        _canvasGroup.DOFade(0, tween_duration);
    }
}
