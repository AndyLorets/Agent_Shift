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
        StartCoroutine(Invisibility());
    }
    public void ActiveArmor()
    {
        StartCoroutine(Armor());
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

        onArmor?.Invoke(false);
        _audioManager.PlayAbilityOff();
    }
}
