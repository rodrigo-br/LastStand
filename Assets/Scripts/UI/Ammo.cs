using System;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : UIBase
{
    private int bulletCount;
    private int maxBulletCount;
    public static event Action OnOutOfAmmo;

    private void Start()
    {
        maxBulletCount = transform.childCount;
        bulletCount = maxBulletCount;
    }

    private void ResetAmmo()
    {
        Image[] children = gameObject.GetComponentsInChildren<Image>(true);

        foreach (Image child in children)
        {
            child.enabled = true;
        }
        bulletCount = maxBulletCount;
    }

    private void OnEnable()
    {
        Bullet.OnShoot += Fire;
        GameManager.OnResetLevel += ResetAmmo;
    }

    private void OnDisable()
    {
        Bullet.OnShoot -= Fire;
        GameManager.OnResetLevel -= ResetAmmo;
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
