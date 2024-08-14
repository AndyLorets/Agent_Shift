using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private MessageUI _messageUI; 
    private GameDataController _gameData;
    private Wallet _wallet;
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
    }
    private void Start()
    {
        _messageUI = ServiceLocator.GetService<MessageUI>();    
        _gameData = ServiceLocator.GetService<GameDataController>();
        _wallet = ServiceLocator.GetService<Wallet>();
    }

    private bool TryUpgrade(ref float currentValue, float maxValue, float increaseValue, ref int price, ref int priceIncreaseValue)
    {
        int moneyCount = _wallet.CurrentMoney;

        if(moneyCount < price)
        {
            _messageUI.ShowMessage("Not enough Money");
            return false;
        }

        if (currentValue < maxValue)
        {
            _wallet.RemoveMoney(price);
            currentValue += increaseValue;
            price += priceIncreaseValue;
            _gameData.SaveData();
            //ServiceLocator.GetService<AudioManager>().PlayUpgrade(); 
            return true;
        }

        _messageUI.ShowMessage("Value is already at maximum.");
        return false;
    }
    private bool TryUpgradeDecrease(ref float currentValue, float maxValue, float increaseValue, ref int price, ref int priceIncreaseValue)
    {
        int moneyCount = _wallet.CurrentMoney;

        if (moneyCount < price)
        {
            _messageUI.ShowMessage("Not enough Money");
            return false;
        }

        if (currentValue > maxValue && moneyCount >= price)
        {
            _wallet.RemoveMoney(price);
            currentValue -= increaseValue;
            price += priceIncreaseValue;
            _gameData.SaveData();
            //ServiceLocator.GetService<AudioManager>().PlayUpgrade();
            return true;
        }

        _messageUI.ShowMessage("Value is already at maximum.");
        return false;
    }
    public void UpgradeArmor()
    {
        ref float currentValue = ref _gameData.PlayerData.abilitiesData.armorCurrentValue;
        float maxValue = _gameData.PlayerData.abilitiesData.armorMaxValue;
        float increaseValue = _gameData.PlayerData.abilitiesData.armorIncreaseValue;
        ref int price = ref _gameData.PlayerData.abilitiesData.armorPrice;
        ref int priceIncreaseValue = ref _gameData.PlayerData.abilitiesData.armorPriceIncreaseValue;

        TryUpgrade(ref currentValue, maxValue, increaseValue, ref price, ref priceIncreaseValue);
    }

    public void UpgradeInvisibility()
    {
        ref float currentValue = ref _gameData.PlayerData.abilitiesData.invisibilityCurrentValue;
        float maxValue = _gameData.PlayerData.abilitiesData.invisibilityMaxValue;
        float increaseValue = _gameData.PlayerData.abilitiesData.invisibilityIncreaseValue;
        ref int price = ref _gameData.PlayerData.abilitiesData.invisibilityPrice;
        ref int priceIncreaseValue = ref _gameData.PlayerData.abilitiesData.invisibilityPriceIncreaseValue;

        TryUpgrade(ref currentValue, maxValue, increaseValue, ref price, ref priceIncreaseValue);
    }

    public void UpgradeHeadShotChance()
    {
        ref float currentValue = ref _gameData.PlayerData.abilitiesData.headShotChanceCurrentValue;
        float maxValue = _gameData.PlayerData.abilitiesData.headShotChanceMaxValue;
        float increaseValue = _gameData.PlayerData.abilitiesData.headShotChanceIncreaseValue;
        ref int price = ref _gameData.PlayerData.abilitiesData.headShotChancePrice;
        ref int priceIncreaseValue = ref _gameData.PlayerData.abilitiesData.headShotChancePriceIncreaseValue;

        TryUpgrade(ref currentValue, maxValue, increaseValue, ref price, ref priceIncreaseValue);
    }

    public void UpgradePistolDamage()
    {
        ref float currentValue = ref _gameData.PlayerData.weaponData.pistolCurrentDamage;
        float maxValue = _gameData.PlayerData.weaponData.pistolMaxDamage;
        float increaseValue = _gameData.PlayerData.weaponData.pistolIncreaseDamage;
        ref int price = ref _gameData.PlayerData.weaponData.pistolDamagePrice;
        ref int priceIncreaseValue = ref _gameData.PlayerData.weaponData.pistolDamagePriceIncreaseValue;

        TryUpgrade(ref currentValue, maxValue, increaseValue, ref price, ref priceIncreaseValue);
    }

    public void UpgradePistolShootDelay()
    {
        ref float currentValue = ref _gameData.PlayerData.weaponData.pistolCurrentShootDelay;
        float maxValue = _gameData.PlayerData.weaponData.pistolMinShootDelay;
        float increaseValue = _gameData.PlayerData.weaponData.pistolIncreaseShootDelay;
        ref int price = ref _gameData.PlayerData.weaponData.pistolShootDelayPrice;
        ref int priceIncreaseValue = ref _gameData.PlayerData.weaponData.pistolShootDelayPriceIncreaseValue;

        TryUpgradeDecrease(ref currentValue, maxValue, increaseValue, ref price, ref priceIncreaseValue);
    }
    public void UpgradeHP()
    {
        ref float currentValue = ref _gameData.PlayerData.currentHp;
        float maxValue = _gameData.PlayerData.maxHp;
        float increaseValue = _gameData.PlayerData.hpIncreaseValue;
        ref int price = ref _gameData.PlayerData.hpPrice;
        ref int priceIncreaseValue = ref _gameData.PlayerData.hpPriceIncreaseValue;

        TryUpgrade(ref currentValue, maxValue, increaseValue, ref price, ref priceIncreaseValue);
    }
}
