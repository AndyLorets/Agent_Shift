using System.IO;
using UnityEngine;

public static class SaveManager
{
#if UNITY_EDITOR
    private static string folderPath = Path.Combine(Application.dataPath, "Saves");
    private static string filePath = Path.Combine(folderPath, "savefile.json");
#else
    private static string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
    private static string filePath = Path.Combine(folderPath, "savefile.json");
#endif
    static SaveManager()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }
    public static void SavePlayerData(PlayerData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
        }
        catch (IOException e)
        {
            Debug.LogError($"Error saving player data: {e.Message}");
        }
    }
    public static PlayerData LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<PlayerData>(json);
            }
            catch (IOException e)
            {
                Debug.LogError($"Error loading player data: {e.Message}");
                return new PlayerData(); 
            }
        }
        else
        {
            Debug.Log($"Loading new player data");
            return new PlayerData(); 
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public int currentLevel = 1;
    public int openLevel = 2;
    public int moneyCount = 0;

    public float startHp = 10f;
    public float currentHp;
    public float maxHp = 50f;
    public float hpIncreaseValue = 10f;
    public int hpPrice = 5;
    public int hpPriceIncreaseValue = 5;

    public AbilitiesData abilitiesData = new AbilitiesData();
    public WeaponData weaponData = new WeaponData();   
    
    public PlayerData()
    {
        currentHp = startHp; 
    }
}

[System.Serializable]
public class AbilitiesData
{
    public float armorStartValue = 5f;
    public float armorCurrentValue;
    public float armorMaxValue = 15f;
    public float armorIncreaseValue = 1f;
    public int armorPrice = 5;
    public int armorPriceIncreaseValue = 5;

    public float invisibilityStartValue = 5f;
    public float invisibilityCurrentValue;
    public float invisibilityMaxValue = 15f;
    public float invisibilityIncreaseValue = 1f;
    public int invisibilityPrice = 5;
    public int invisibilityPriceIncreaseValue = 5;

    public float headShotChanceStartValue = 30f;
    public float headShotChanceCurrentValue;
    public float headShotChanceMaxValue = 90f;
    public float headShotChanceIncreaseValue = 10f;
    public int headShotChancePrice = 10;
    public int headShotChancePriceIncreaseValue = 5;

    public AbilitiesData()
    {
        armorCurrentValue = armorStartValue;
        invisibilityCurrentValue = invisibilityStartValue;
        headShotChanceCurrentValue = headShotChanceStartValue;
    }
}
[System.Serializable]
public class WeaponData
{
    public float pistolStartDamage = 1;
    public float pistolCurrentDamage;
    public float pistolMaxDamage = 5;
    public float pistolIncreaseDamage = 1;
    public int pistolDamagePrice = 10;
    public int pistolDamagePriceIncreaseValue = 5;

    public float pistolStartShootDelay = .7f;
    public float pistolCurrentShootDelay;
    public float pistolMinShootDelay = .3f;
    public float pistolIncreaseShootDelay = .1f;
    public int pistolShootDelayPrice = 5;
    public int pistolShootDelayPriceIncreaseValue = 2;

    public WeaponData()
    {
        pistolCurrentDamage = pistolStartDamage;
        pistolCurrentShootDelay = pistolStartShootDelay; 
    }
}