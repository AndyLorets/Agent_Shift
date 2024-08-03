using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private float _invisibilityTime = 10f;
    [SerializeField] private float _armorTime = 5f;
    public float headShotChance = 100f;

    public static System.Action<float> onChangeInvisibilitTime;
    public static System.Action<bool> onInvisibility;
    public static System.Action<float> onChangeArmorTime;
    public static System.Action<bool> onArmor;
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
        float t = _invisibilityTime;

        onInvisibility?.Invoke(true);

        while (t > 0)
        {
            t--; 
            onChangeInvisibilitTime?.Invoke(t / _invisibilityTime);  
            yield return waitForSeconds;
        }

        onInvisibility?.Invoke(false);
    }
    private IEnumerator Armor()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        float t = _armorTime;

        onArmor?.Invoke(true);

        while (t > 0)
        {
            t--;
            onChangeArmorTime?.Invoke(t / _armorTime);
            yield return waitForSeconds;
        }

        onArmor?.Invoke(false);
    }
}
