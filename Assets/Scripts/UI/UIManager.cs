using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Timer timerUI;
    [SerializeField] private Score scoreUI;
    [SerializeField] private Ammo ammoUI;
    [SerializeField] private float levelTime = 10.0f;
    [SerializeField] private int pointsPerTimeLeft = 100;
    [SerializeField] private int pointsPerBulletsLeft = 100;
    [SerializeField] private Animator enemyAnimator;
    private float timeRemaining;
    private bool endRound;
    private int score = 0;
    public static event Action OnEndOfTime;
    private float nextShoot;
    private Enemy enemy;

    private void Awake()
    {
        enemy = enemyAnimator.GetComponent<Enemy>();
    }

    private void Start()
    {
        nextShoot = levelTime - 2f;
        timeRemaining = levelTime;
        timerUI.SetTimer(timeRemaining);
        scoreUI.SetScore(score);
    }

    private void Update()
    {
        if (endRound) return;
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
        {
            SetEndRoundWithoutScore();
        }
        if (timeRemaining < nextShoot)
        {
            enemyAnimator.SetTrigger("Shoot");
            enemy.Explode();
            nextShoot -= 2f;
        }
        timerUI.SetTimer(timeRemaining);
    }

    private void OnEnable()
    {
        Bullet.OnHitChar += SetEndRound;
        GameManager.OnResetLevel += ResetLevel;
    }

    private void OnDisable()
    {
        Bullet.OnHitChar -= SetEndRound;
        GameManager.OnResetLevel -= ResetLevel;
    }

    private void ResetLevel(bool isDefeat)
    {
        if (!isDefeat)
        {
            enemyAnimator.SetTrigger("Reset");
        }
        timeRemaining = levelTime;
        nextShoot = levelTime - 2f;
        timerUI.SetTimer(levelTime);
        endRound = false;
    }

    private void SetEndRound()
    {
        this.endRound = true;
        if (timeRemaining > 0)
        {
            score += (int)(timeRemaining * pointsPerTimeLeft);
            score += (ammoUI.GetBulletCount() * pointsPerBulletsLeft);
            scoreUI.SetScore(score);
        }
    }

    private void SetEndRoundWithoutScore()
    {
        GameManager.Instance.SetDefeat(true);
        score = 0;
        scoreUI.SetScore(score);
        this.endRound = true;
        OnEndOfTime?.Invoke();
    }
}
