using DG.Tweening;
using UnityEngine;

public class PistolWeapon : WeaponBase
{
    public override void Shoot(ITakeDamage takeDamageDamage, Vector3 pos, bool headshoot)
    {
        _bulletOnMagazine--;
        _shootEffect.Play();
        onShoot?.Invoke(_bulletOnMagazine, _bulletsOnMagazine.Count);

        Transform bullet = _bulletsOnMagazine[0];
        bullet.transform.parent = null;
        bullet.gameObject.SetActive(true);
        bullet.DOMove(pos, BULLET_TWEEN_DURATION)
            .SetEase(Ease.Linear)
            .OnComplete(() => BulletHitTarget(bullet, takeDamageDamage, headshoot));

        _bulletsOnMagazine.RemoveAt(0);
    }
}
