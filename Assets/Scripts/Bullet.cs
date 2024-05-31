using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] LayerMask layerMask;
    private bool debugShoot;
    private Vector2 targetPosition;
    private Vector2 lastSpawnPosition;
    public static event Action OnHitChar;
    public static event Action OnShoot;
    private GameObject enemyObject;

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
                    enemyObject.SetActive(false);
                    OnHitChar?.Invoke();
                }
                ResetPosition(lastSpawnPosition);
                debugShoot = false;
            }
            return;
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

    private void ResetLevel()
    {
        if (enemyObject == null) return;

        enemyObject.SetActive(true);
    }

    private void Shoot()
    {
        targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        if (!debugShoot)
        {
            OnShoot?.Invoke();
        }
        debugShoot = true;
    }

    public void ResetPosition(Vector2 spawnPosition)
    {
        this.gameObject.transform.localPosition = spawnPosition;
        lastSpawnPosition = spawnPosition;
    }

    private void SetIsShooting(bool isShooting)
    {
        if (isShooting)
        {
            Shoot();
        }
    }
}
