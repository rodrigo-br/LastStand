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
    private float timeRemaining;
    private bool endRound;
    private int score = 0;
    public static event Action OnEndOfTime;

    private void Start()
    {
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

    private void ResetLevel()
    {
        timeRemaining = levelTime;
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
        this.endRound = true;
        OnEndOfTime?.Invoke();
    }
}
