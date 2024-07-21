using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bulletText;
    [SerializeField] private Image _hpImage;

    [SerializeField] private Player _player; 
    private void Awake()
    {
        _player.currentWeapon.onShoot += RenderBulletsCount;
        _player.currentWeapon.onEndReload += RenderBulletsCount;
        _player.onChangeHP += RenderHP; 
    }
    private void OnDestroy()
    {
        _player.currentWeapon.onShoot -= RenderBulletsCount;
        _player.currentWeapon.onEndReload -= RenderBulletsCount;
        _player.onChangeHP -= RenderHP;
    }
    private void RenderHP(float hp, float maxHp)
    {
        _hpImage.fillAmount = hp / maxHp; 
    }
    private void RenderBulletsCount(int bulletCount, int magazineRange)
    {
        _bulletText.text = $"{bulletCount}/{magazineRange}"; 
    }
}
