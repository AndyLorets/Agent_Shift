using System;
using UnityEngine;

public class GameDataController : MonoBehaviour
{
    public static GameDataController Instance { get; private set; }
    public PlayerData PlayerData { get; private set; }

    public static Action onDataSave; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ServiceLocator.RegisterService(this);
        PlayerData = SaveManager.LoadPlayerData();
    }

    public void SaveData()
    {
        onDataSave?.Invoke(); 
        SaveManager.SavePlayerData(PlayerData);
    }
}
