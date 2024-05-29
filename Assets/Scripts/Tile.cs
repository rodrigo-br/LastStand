using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer debugSpriteRenderer;
    private Vector2 bulletPosition;
    private GameObject bullet;
    private SpriteRenderer gunSpriteRenderer;
    private Sprite mySprite;

    public void Init(bool isOffset, Sprite sprite, SpriteRenderer gunSpriteRenderer, GameObject bullet, Vector2 bulletPosition)
    {
        debugSpriteRenderer.color = isOffset ? offsetColor : baseColor;
        this.gunSpriteRenderer = gunSpriteRenderer;
        mySprite = sprite;
        this.bullet = bullet;
        this.bulletPosition = bulletPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Aim aim = collision.GetComponent<Aim>();

        if (aim == null) { return; }

        gunSpriteRenderer.sprite = mySprite;
        bullet.transform.localPosition = bulletPosition;
    }
}
