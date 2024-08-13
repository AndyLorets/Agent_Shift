using UnityEngine;

public class GameDataController : MonoBehaviour 
{
    public PlayerData PlayerData { get; private set; }
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        PlayerData = SaveManager.LoadPlayerData();
    }
    public void SaveData()
    {
        SaveManager.SavePlayerData(PlayerData); 
    }
}
