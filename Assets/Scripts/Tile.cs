using System;
using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer debugSpriteRenderer;
    private Bullet bullet;
    private SpriteRenderer gunSpriteRenderer;
    private SpriteSet mySpriteSet;
    private bool isMe;
    private static bool isShooting;
    private Vector2 spawnPosition;
    public static Action<bool> IsShootingAction;
    private static bool canShoot = true;

    public void Init(bool isOffset, SpriteSet sprite, SpriteRenderer gunSpriteRenderer, Bullet bullet, Vector2 bulletPosition)
    {
        debugSpriteRenderer.color = isOffset ? offsetColor : baseColor;
        this.gunSpriteRenderer = gunSpriteRenderer;
        mySpriteSet = sprite;
        this.bullet = bullet;
        this.spawnPosition = bulletPosition;
    }

    private void OnEnable()
    {
        GameManager.Instance.InputObserver.OnShootAction += Shoot;
        GameManager.OnResetLevel += ResetLevel;
        Ammo.OnOutOfAmmo += () => SetCanShoot(false);
    }

    private void OnDisable()
    {
        GameManager.Instance.InputObserver.OnShootAction -= Shoot;
        GameManager.OnResetLevel -= ResetLevel;
        Ammo.OnOutOfAmmo -= () => SetCanShoot(false);
    }

    private void ResetLevel(bool isDefeat)
    {
        SetCanShoot(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Aim aim = collision.GetComponent<Aim>();

        if (aim == null) { return; }
        isMe = true;
        gunSpriteRenderer.sprite = mySpriteSet.sprite1;
        bullet.ResetPosition(spawnPosition);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isMe = false;
    }

    private void Shoot()
    {
        if (!canShoot || !bullet.CanShoot) { return; }
        if (isMe && !isShooting)
        {
            isShooting = true;
            IsShootingAction?.Invoke(true);
            StartCoroutine(ShootAnimation());
        }
    }

    private void SetCanShoot(bool canShoot)
    {
        Tile.canShoot = canShoot;
    }

    private IEnumerator ShootAnimation()
    {
        gunSpriteRenderer.sprite = mySpriteSet.sprite2;
        yield return new WaitForSeconds(0.1f);
        if (isMe)
        {
            gunSpriteRenderer.sprite = mySpriteSet.sprite3;
        }
        yield return new WaitForSeconds(0.1f);
        if (isMe)
        {
            gunSpriteRenderer.sprite = mySpriteSet.sprite2;
        }
        yield return new WaitForSeconds(0.1f);
        if (isMe)
        {
            gunSpriteRenderer.sprite = mySpriteSet.sprite1;
        }
        yield return new WaitForSeconds(0.3f);
        isShooting = false;
        IsShootingAction?.Invoke(false);
    }
}
