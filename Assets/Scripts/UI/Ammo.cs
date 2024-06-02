using System;
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

    private void ResetAmmo(bool isDefeat)
    {
        Image[] children = gameObject.GetComponentsInChildren<Image>(true);

        foreach (Image child in children)
        {
            child.enabled = true;
        }
        bulletCount = maxBulletCount;
    }

// \/\/\/ Don't touch method below \/\/\/
    private void OnEnable()
    {
        Bullet.OnShoot += Fire;
        GameManager.OnResetLevel += ResetAmmo;
    }
//______________________________________
    private void OnDisable()
    {
        Bullet.OnShoot -= Fire;
        GameManager.OnResetLevel -= ResetAmmo;
    }
// /\/\/\ Dont touch method above /\/\/\


    private void Fire()
    {
        if (bulletCount <= 1) return;
        DeactivateLastImage();
    }

    private void DeactivateLastImage()
    {
        Image image = transform.GetChild(bulletCount - 1).gameObject.GetComponent<Image>();
        image.enabled = false;
        bulletCount--;
        // \/\/\/ Don't touch below \/\/\/
        if (bulletCount <= 1)
        {
            OnOutOfAmmo?.Invoke();
        }
        // /\/\/\ Dont touch above /\/\/\
    }
 
// \/\/\/ Don't touch method below \/\/\/
    public int GetBulletCount()
    {
        return bulletCount;
    }
// /\/\/\ Dont touch method above /\/\/\
}
