using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public float invisibilityTime { get; set; }
    public float armorTime { get; set; }
    public float headShotChance { get; set; }

    public const float RESET_DURATION = 30f;

    public static System.Action<float> onChangeInvisibilitTime;
    public static System.Action<bool> onInvisibility;
    public static System.Action<float> onChangeArmorTime;
    public static System.Action<bool> onArmor;

    private AudioManager _audioManager;
    private Coroutine _armorCoroutine;
    private Coroutine _invisibilityCoroutine;

    private void Awake()
    {
        BriefingManager.onStartBriefing += Load; 
    }
    private void Start()
    {
        _audioManager = ServiceLocator.GetService<AudioManager>();
    }
    private void OnDestroy()
    {
        BriefingManager.onStartBriefing -= Load;
    }
    private void Load()
    {
        AbilitiesData abilitiesData = ServiceLocator.GetService<GameDataController>().PlayerData.abilitiesData;
        armorTime = abilitiesData.armorCurrentValue;
        invisibilityTime = abilitiesData.invisibilityCurrentValue;
        headShotChance = abilitiesData.headShotChanceCurrentValue; 
    }
    public void ActiveInvisibility()
    {
        if (_invisibilityCoroutine == null)
        {
            _invisibilityCoroutine = StartCoroutine(Invisibility());
        }        
    }
    public void ActiveArmor()
    {
        if (_armorCoroutine == null)
        {
            _armorCoroutine = StartCoroutine(Armor());
        }
    }
    private IEnumerator Invisibility()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1); 
        float t = invisibilityTime;

        onInvisibility?.Invoke(true);
        _audioManager.PlayAbilityOn();

        while (t > 0)
        {
            t--; 
            onChangeInvisibilitTime?.Invoke(t / invisibilityTime);  
            yield return waitForSeconds;
        }

        _invisibilityCoroutine = null;  
        onInvisibility?.Invoke(false);
        _audioManager.PlayAbilityOff();
    }
    private IEnumerator Armor()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        float t = armorTime;

        onArmor?.Invoke(true);
        _audioManager.PlayAbilityOn();

        while (t > 0)
        {
            t--;
            onChangeArmorTime?.Invoke(t / armorTime);
            yield return waitForSeconds;
        }

        _armorCoroutine = null; 
        onArmor?.Invoke(false);
        _audioManager.PlayAbilityOff();
    }

    public void CancelInvisibility()
    {
        if (_invisibilityCoroutine != null)
        {
            onInvisibility?.Invoke(false);
            _audioManager.PlayAbilityOff();
            StopCoroutine(_invisibilityCoroutine);
            _invisibilityCoroutine = null;
        }
    }
    public void CancelArmor()
    {
        if (_armorCoroutine != null)
        {
            onArmor?.Invoke(false);
            _audioManager.PlayAbilityOff();
            StopCoroutine(_armorCoroutine);
            _armorCoroutine = null;
        }
    }
}
