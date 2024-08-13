using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int CurrentMoney { get; private set; }
    public System.Action onMoneyChanged; 
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
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
    }
    public void RemoveMoney(int value)
    {
        CurrentMoney -= value;
        onMoneyChanged?.Invoke();
    }
}
