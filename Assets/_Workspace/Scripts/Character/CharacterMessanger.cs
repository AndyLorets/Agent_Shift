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

    private const float tween_duration = .3f;

    public static Action OnResetAudioPlaying; 

    private void Awake()
    {
        ServiceLocator.RegisterService(this); 
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0; 
    }
    public void SetDialogue(Sprite sprite, CharacterDialogue characterDialogue, bool skipActiveDialogue = false)
    {
        AudioClip clip = characterDialogue.clip;
        string text = characterDialogue.text;

        if (skipActiveDialogue && CharacterDialogue.speaking)
            Skip(); 

        if (clip != null && !CharacterDialogue.speaking)
        {
            _audioSource.PlayOneShot(clip);
            _icon.sprite = sprite; 
            _text.text = text;

            _canvasGroup.DOFade(1, tween_duration); 

            CharacterDialogue.speaking = true;
            StartCoroutine(ResetAudioPlaying(clip));
        }
    }
    private IEnumerator ResetAudioPlaying(AudioClip audioClip = null)
    {
        yield return new WaitForSeconds(audioClip.length + .5f);
        CharacterDialogue.speaking = false;
        ClearText();
        OnResetAudioPlaying?.Invoke(); 
    }
    public void Skip()
    {
        _audioSource.Stop(); 
        StopCoroutine(ResetAudioPlaying());
        ClearText();
        OnResetAudioPlaying?.Invoke();
        CharacterDialogue.speaking = false;
    }
    private void ClearText()
    {
        _canvasGroup.DOFade(0, tween_duration);
    }
}
