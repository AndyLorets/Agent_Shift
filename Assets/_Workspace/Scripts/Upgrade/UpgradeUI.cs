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
        PistolShootDelay
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

    private void UpdateUI()
    {
        float currentValue = 0;
        float maxValue = 0;
        float increaseValue = 0;
        int price = 0;
        int priceIncreaseValue = 0;

        switch (_upgradeType)
        {
            case UpgradeType.Armor:
                currentValue = _gameData.PlayerData.abilitiesData.armorCurrentValue;
                maxValue = _gameData.PlayerData.abilitiesData.armorMaxValue;
                increaseValue = _gameData.PlayerData.abilitiesData.armorIncreaseValue;
                price = _gameData.PlayerData.abilitiesData.armorPrice;
                priceIncreaseValue = _gameData.PlayerData.abilitiesData.armorPriceIncreaseValue;

                _currentValueText.text = $"Armor Duration:\n{currentValue} / {maxValue}";
                break;

            case UpgradeType.Invisibility:
                currentValue = _gameData.PlayerData.abilitiesData.invisibilityCurrentValue;
                maxValue = _gameData.PlayerData.abilitiesData.invisibilityMaxValue;
                increaseValue = _gameData.PlayerData.abilitiesData.invisibilityIncreaseValue;
                price = _gameData.PlayerData.abilitiesData.invisibilityPrice;
                priceIncreaseValue = _gameData.PlayerData.abilitiesData.invisibilityPriceIncreaseValue;

                _currentValueText.text = $"Invisibility Duration:\n{currentValue} / {maxValue}";
                break;

            case UpgradeType.HeadShotChance:
                currentValue = _gameData.PlayerData.abilitiesData.headShotChanceCurrentValue;
                maxValue = _gameData.PlayerData.abilitiesData.headShotChanceMaxValue;
                increaseValue = _gameData.PlayerData.abilitiesData.headShotChanceIncreaseValue;
                price = _gameData.PlayerData.abilitiesData.headShotChancePrice;
                priceIncreaseValue = _gameData.PlayerData.abilitiesData.headShotChancePriceIncreaseValue;

                _currentValueText.text = $"HeadShot Chance:\n{currentValue} / {maxValue}";
                break;

            case UpgradeType.PistolDamage:
                currentValue = _gameData.PlayerData.weaponData.pistolCurrentDamage;
                maxValue = _gameData.PlayerData.weaponData.pistolMaxDamage;
                increaseValue = _gameData.PlayerData.weaponData.pistolIncreaseDamage;
                price = _gameData.PlayerData.weaponData.pistolDamagePrice;
                priceIncreaseValue = _gameData.PlayerData.weaponData.pistolDamagePriceIncreaseValue;

                _currentValueText.text = $"Pistol Damage:\n{currentValue} / {maxValue}";
                break;

            case UpgradeType.PistolShootDelay:
                currentValue = _gameData.PlayerData.weaponData.pistolCurrentShootDelay;
                maxValue = _gameData.PlayerData.weaponData.pistolMaxShootDelay;
                increaseValue = _gameData.PlayerData.weaponData.pistolIncreaseShootDelay;
                price = _gameData.PlayerData.weaponData.pistolShootDelayPrice;
                priceIncreaseValue = _gameData.PlayerData.weaponData.pistolShootDelayPriceIncreaseValue;

                _currentValueText.text = $"Pistol Shoot Delay:\n{currentValue} / {maxValue}";
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
        }

        UpdateUI();
    }
}
