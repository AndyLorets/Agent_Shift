using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string folderPath = Path.Combine(Application.dataPath, "Saves");
    private static string filePath = Path.Combine(folderPath, "savefile.json");

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
    public int currentLevel;
    public int moneyCount = 500;
    public int hp = 10;

    public AbilitiesData abilitiesData = new AbilitiesData();
    public WeaponData weaponData = new WeaponData();    
}

[System.Serializable]
public class AbilitiesData
{
    public float armorCurrentValue = 5f;
    public float armorMaxValue = 10f;
    public float armorIncreaseValue = 1f;
    public int armorPrice = 5;
    public int armorPriceIncreaseValue = 5;

    public float invisibilityCurrentValue = 10f;
    public float invisibilityMaxValue = 20f;
    public float invisibilityIncreaseValue = 1f;
    public int invisibilityPrice = 5;
    public int invisibilityPriceIncreaseValue = 5;

    public float headShotChanceCurrentValue = 10f;
    public float headShotChanceMaxValue = 80f;
    public float headShotChanceIncreaseValue = 10f;
    public int headShotChancePrice = 10;
    public int headShotChancePriceIncreaseValue = 5;
}
[System.Serializable]
public class WeaponData
{
    public float pistolCurrentDamage = 1;
    public float pistolMaxDamage = 5;
    public float pistolIncreaseDamage = 1;
    public int pistolDamagePrice = 10;
    public int pistolDamagePriceIncreaseValue = 5;

    public float pistolCurrentShootDelay = .7f;
    public float pistolMaxShootDelay = .3f;
    public float pistolIncreaseShootDelay = -.1f;
    public int pistolShootDelayPrice = 5;
    public int pistolShootDelayPriceIncreaseValue = 2;
}