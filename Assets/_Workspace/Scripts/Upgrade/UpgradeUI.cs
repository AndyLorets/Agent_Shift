using UnityEngine;
using UnityEngine.UI;
using TMPro; 

[RequireComponent(typeof(Button))]
public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private UpgradeType _upgradeType;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI _currentValueText;
    [SerializeField] private TextMeshProUGUI _priceText;

    private UpgradeManager _upgrade;
    private GameDataController _gameData;
    private Button _upgradeButton;
    public enum UpgradeType
    {
        Armor,
        Invisibility,
        HeadShotChance,
        PistolDamage,
        PistolShootDelay,
        HP
    }

    private void Awake()
    {
        _upgradeButton = GetComponent<Button>();
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    private void Start()
    {
        _upgrade = ServiceLocator.GetService<UpgradeManager>();
        _gameData = ServiceLocator.GetService<GameDataController>();

        UpdateUI();
    }
    int GetUpgradesMade(float startValue, float currentValue, float increaseValue)
    {
        return Mathf.Max(0, Mathf.FloorToInt((currentValue - startValue) / increaseValue));
    }
    int GetMaxUpgrades(float startValue, float maxValue, float increaseValue)
    {
        return Mathf.Max(0, Mathf.FloorToInt((maxValue - startValue) / increaseValue));
    }
    int GetUpgradesMadeDecrease(float startValue, float currentValue, float decreaseValue)
    {
        if (decreaseValue <= 0) return 0;
        return Mathf.Max(0, Mathf.FloorToInt((startValue - currentValue) / decreaseValue));
    }
    int GetMaxUpgradesDecrease(float startValue, float minValue, float decreaseValue)
    {
        if (decreaseValue <= 0) return 0;
        return Mathf.RoundToInt((startValue - minValue) / decreaseValue);
    }
    private void UpdateUI()
    {
        float startValue = 0; 
        float currentValue = 0;
        float maxValue = 0;
        float increaseValue = 0;
        int upgradesMade = 0;
        int maxUpgrades = 0; 
        int price = 0;

        switch (_upgradeType)
        {
            case UpgradeType.Armor:
                startValue = _gameData.PlayerData.abilitiesData.armorStartValue; 
                currentValue = _gameData.PlayerData.abilitiesData.armorCurrentValue;
                maxValue = _gameData.PlayerData.abilitiesData.armorMaxValue;
                increaseValue = _gameData.PlayerData.abilitiesData.armorIncreaseValue;
                price = _gameData.PlayerData.abilitiesData.armorPrice;
                upgradesMade = GetUpgradesMade(startValue, currentValue, increaseValue);
                maxUpgrades = GetMaxUpgrades(startValue, maxValue, increaseValue);
                _currentValueText.text = $"Armor Duration: {currentValue} \n {upgradesMade} / {maxUpgrades}";
                break;

            case UpgradeType.Invisibility:
                startValue = _gameData.PlayerData.abilitiesData.invisibilityStartValue; 
                currentValue = _gameData.PlayerData.abilitiesData.invisibilityCurrentValue;
                maxValue = _gameData.PlayerData.abilitiesData.invisibilityMaxValue;
                increaseValue = _gameData.PlayerData.abilitiesData.invisibilityIncreaseValue;
                price = _gameData.PlayerData.abilitiesData.invisibilityPrice;
                upgradesMade = GetUpgradesMade(startValue, currentValue, increaseValue);
                maxUpgrades = GetMaxUpgrades(startValue, maxValue, increaseValue);
                _currentValueText.text = $"Invisibility Duration: {currentValue} \n {upgradesMade} / {maxUpgrades}";
                break;

            case UpgradeType.HeadShotChance:
                startValue = _gameData.PlayerData.abilitiesData.headShotChanceStartValue; 
                currentValue = _gameData.PlayerData.abilitiesData.headShotChanceCurrentValue;
                maxValue = _gameData.PlayerData.abilitiesData.headShotChanceMaxValue;
                increaseValue = _gameData.PlayerData.abilitiesData.headShotChanceIncreaseValue;
                price = _gameData.PlayerData.abilitiesData.headShotChancePrice;
                upgradesMade = GetUpgradesMade(startValue, currentValue, increaseValue);
                maxUpgrades = GetMaxUpgrades(startValue, maxValue, increaseValue);
                _currentValueText.text = $"HeadShot Chance: {currentValue} \n {upgradesMade} / {maxUpgrades}";
                break;

            case UpgradeType.PistolDamage:
                startValue = _gameData.PlayerData.weaponData.pistolStartDamage; 
                currentValue = _gameData.PlayerData.weaponData.pistolCurrentDamage;
                maxValue = _gameData.PlayerData.weaponData.pistolMaxDamage;
                increaseValue = _gameData.PlayerData.weaponData.pistolIncreaseDamage;
                price = _gameData.PlayerData.weaponData.pistolDamagePrice;
                upgradesMade = GetUpgradesMade(startValue, currentValue, increaseValue);
                maxUpgrades = GetMaxUpgrades(startValue, maxValue, increaseValue);
                _currentValueText.text = $"Pistol Damage: {currentValue} \n {upgradesMade} / {maxUpgrades}";
                break;

            case UpgradeType.PistolShootDelay:
                startValue = _gameData.PlayerData.weaponData.pistolStartShootDelay; 
                currentValue = _gameData.PlayerData.weaponData.pistolCurrentShootDelay;
                maxValue = _gameData.PlayerData.weaponData.pistolMinShootDelay;
                increaseValue = _gameData.PlayerData.weaponData.pistolIncreaseShootDelay;
                price = _gameData.PlayerData.weaponData.pistolShootDelayPrice;
                upgradesMade = GetUpgradesMadeDecrease(startValue, currentValue, increaseValue);
                maxUpgrades = GetMaxUpgradesDecrease(startValue, maxValue, increaseValue);
                _currentValueText.text = $"Pistol Shoot Delay: {currentValue} \n {upgradesMade} / {maxUpgrades}";
                break;

            case UpgradeType.HP:
                startValue = _gameData.PlayerData.startHp;
                currentValue = _gameData.PlayerData.currentHp;
                maxValue = _gameData.PlayerData.maxHp;
                increaseValue = _gameData.PlayerData.hpIncreaseValue;
                price = _gameData.PlayerData.hpPrice;
                upgradesMade = GetUpgradesMade(startValue, currentValue, increaseValue);
                maxUpgrades = GetMaxUpgrades(startValue, maxValue, increaseValue);
                _currentValueText.text = $"Health: {currentValue} \n {upgradesMade} / {maxUpgrades}";
                break;
        }

        _priceText.text = $"{price}";
    }

    private void OnUpgradeButtonClicked()
    {
        switch (_upgradeType)
        {
            case UpgradeType.Armor:
                _upgrade.UpgradeArmor();
                break;

            case UpgradeType.Invisibility:
                _upgrade.UpgradeInvisibility();
                break;

            case UpgradeType.HeadShotChance:
                _upgrade.UpgradeHeadShotChance();
                break;

            case UpgradeType.PistolDamage:
                _upgrade.UpgradePistolDamage();
                break;

            case UpgradeType.PistolShootDelay:
                _upgrade.UpgradePistolShootDelay();
                break;
            case UpgradeType.HP:
                _upgrade.UpgradeHP();
                break;
        }

        UpdateUI();
    }
}
