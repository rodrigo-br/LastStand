using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem hitParticle;
    private bool debugShoot;
    private Vector2 targetPosition;
    private Vector2 lastSpawnPosition;
    public static event Action OnHitChar;
    public static event Action OnShoot;
    private GameObject enemyObject;
    private bool canShoot = true;
    public bool CanShoot => canShoot;
    private SpriteRenderer aimSprite;

    private void Awake()
    {
        aimSprite = target.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (debugShoot)
        {
            trailRenderer.emitting = true;
            this.transform.localScale = Vector2.MoveTowards(this.transform.localScale, Vector2.zero, shrinkSpeed * Time.deltaTime);
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, speed * Time.deltaTime);
            if (Mathf.Abs(Vector2.Distance(this.transform.position, targetPosition)) < 0.2f)
            {
                Collider2D collider = Physics2D.OverlapPoint(transform.position, layerMask);
                if (collider != null)
                {
                    enemyObject = collider.gameObject;
                    Animator enemyAnimator = enemyObject.GetComponent<Animator>();
                    hitParticle.Play();
                    GameManager.Instance.AudioManager.PlayEnemyDieClip();
                    enemyAnimator.SetTrigger("Die");
                    canShoot = false;
                    OnHitChar?.Invoke();
                }
                ResetPosition(lastSpawnPosition);
                debugShoot = false;
            }
        }
    }

    private void OnEnable()
    {
        Tile.IsShootingAction += SetIsShooting;
        GameManager.OnResetLevel += ResetLevel;
    }

    private void OnDisable()
    {
        Tile.IsShootingAction -= SetIsShooting;
        GameManager.OnResetLevel -= ResetLevel;
    }

    private void ResetLevel(bool isDefeat)
    {
        if (enemyObject == null) return;

        enemyObject.SetActive(true);
        canShoot = true;
    }

    private void Shoot()
    {
        targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        if (!debugShoot)
        {
            OnShoot?.Invoke();
            StartCoroutine(BlinkAim());
            GameManager.Instance.AudioManager.PlayPlayerShootClip();
            _particleSystem.Play();
        }
        debugShoot = true;
    }

    private IEnumerator BlinkAim()
    {
        int blinks = 2;
        while (blinks > 0)
        {
            aimSprite.enabled = true;
            yield return new WaitForSeconds(0.06f);
            aimSprite.enabled = false;
            yield return new WaitForSeconds(0.06f);
            blinks --;
        }
    }

    public void ResetPosition(Vector2 spawnPosition)
    {
        this.gameObject.transform.localPosition = spawnPosition;
        lastSpawnPosition = spawnPosition;
    }

    private void SetIsShooting(bool isShooting)
    {
        if (isShooting && canShoot)
        {
            Shoot();
        }
    }
}
