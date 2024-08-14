using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int CurrentMoney { get; private set; }
    public System.Action onMoneyChanged; 
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        GameManager.onGameWin += SaveData; 
    }
    private void OnDestroy()
    {
        GameManager.onGameWin -= SaveData;
    }
    private void Start()
    {
        CurrentMoney = ServiceLocator.GetService<GameDataController>().PlayerData.moneyCount;
        onMoneyChanged?.Invoke();
    }
    public void AddMoney(int value)
    {
        CurrentMoney += value;
        onMoneyChanged?.Invoke();
        //ServiceLocator.GetService<AudioManager>().PlayMoney();
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
