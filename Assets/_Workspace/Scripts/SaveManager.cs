using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
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

    // Метод для загрузки данных игрока
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
    public int moneyCount;
    public int hp = 10;

    public AbilitiesData abilitiesData = new AbilitiesData();
    public WeaponData weaponData = new WeaponData();    
}

[System.Serializable]
public class AbilitiesData
{
    public float armorTime = 5f;
    public float invisibilityTime = 10f;
    public int headShotChance = 10;

}
[System.Serializable]
public class WeaponData
{
    public int pistolDamage = 1;
    public int riffleDamage = 1;
    public float pistolShootDelay = .3f;
    public float riffleShootDelay = .3f;
}