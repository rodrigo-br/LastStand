using System;
using UnityEngine.UI;

public class Ammo : UIBase
{
    private int bulletCount;
    public static event Action OnOutOfAmmo;

    private void Start()
    {
        bulletCount = transform.childCount;
    }

    private void OnEnable()
    {
        Bullet.OnShoot += Fire;
    }

    private void OnDisable()
    {
        Bullet.OnShoot -= Fire;
    }

    private void Fire()
    {
        if (bulletCount <= 0) return;
        DeactivateLastImage();
    }

    private void DeactivateLastImage()
    {
        Image image = transform.GetChild(bulletCount - 1).gameObject.GetComponent<Image>();
        image.enabled = false;
        bulletCount--;
        if (bulletCount <= 0)
        {
            OnOutOfAmmo?.Invoke();
        }
    }

    public int GetBulletCount()
    {
        return bulletCount;
    }
}
