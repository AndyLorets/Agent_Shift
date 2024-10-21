using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int CurrentMoney { get; private set; }
    public System.Action onMoneyChanged; 
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        GameDataController.onDataSave += SaveData; 
    }
    private void OnDestroy()
    {
        GameDataController.onDataSave -= SaveData;
    }
    private void Start()
    {
        Invoke(nameof(SetStartMoney), 0.5f); 
    }
    private void SetStartMoney()
    {
        CurrentMoney = ServiceLocator.GetService<GameDataController>().PlayerData.moneyCount;
        onMoneyChanged?.Invoke();
    }
    public void AddMoney(int value)
    {
        CurrentMoney += value;
        onMoneyChanged?.Invoke();
    }
    public void RemoveMoney(int value)
    {
        CurrentMoney -= value;
        onMoneyChanged?.Invoke();
        ServiceLocator.GetService<AudioManager>().PlayMoney(); 
    }
    private void SaveData()
    {
        ServiceLocator.GetService<GameDataController>().PlayerData.moneyCount = CurrentMoney; 
    }
}
