using PlayFab.ClientModels;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Timer timerUI;
    [SerializeField] private Score scoreUI;
    [SerializeField] private Ammo ammoUI;
    [SerializeField] private GameObject leaderboardUI;
    [SerializeField] private float levelTime = 10.0f;
    [SerializeField] private int pointsPerTimeLeft = 100;
    [SerializeField] private int pointsPerBulletsLeft = 100;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private PlayfabManager playfabManager;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowsParent;
    [SerializeField] private Button leaderboardAroundPlayerButton;
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
        leaderboardAroundPlayerButton.onClick.AddListener(() => playfabManager.GetLeaderBoardAroundPlayer());
        Cursor.visible = false;
        nextShoot = levelTime - 2f;
        timeRemaining = levelTime;
        timerUI.SetTimer(timeRemaining);
        scoreUI.SetScore(score);
    }

    private void Update()
    {
        if (endRound) return;
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < nextShoot)
        {
            enemyAnimator.SetTrigger("Shoot");
            GameManager.Instance.AudioManager.PlayEnemyShootClip();
            if (nextShoot > 5 && nextShoot < 7)
            {
                GameManager.Instance.AudioManager.PlayEnemySoundClip();
            }
            enemy.Explode();
            nextShoot -= 2f;
        }
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
        ComboGenerator.OnMissCombo += ReduceTimer;
    }


    private void OnDisable()
    {
        Bullet.OnHitChar -= SetEndRound;
        GameManager.OnResetLevel -= ResetLevel;
        ComboGenerator.OnMissCombo -= ReduceTimer;
    }

    private void ReduceTimer()
    {
        timeRemaining -= 0.25f;
    }

    private void ResetLevel(bool isDefeat)
    {
        if (!isDefeat)
        {
            enemyAnimator.SetTrigger("Reset");
        }
        Cursor.visible = false;
        leaderboardUI.SetActive(false);
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
        GameManager.Instance.InputObserver.DeactivateAll();
        GameManager.Instance.AudioManager.PlayPlayerDieClip();
        playfabManager.SendLeaderboard(score);
        GameManager.Instance.SetDefeat(true);
        score = 0;
        scoreUI.SetScore(score);
        this.endRound = true;
        OnEndOfTime?.Invoke();
    }

    public void ShowLeaderBoard(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        leaderboardUI.SetActive(true);
        Cursor.visible = true;

        foreach (var item in result.Leaderboard)
        {
            GameObject newRow = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = item.DisplayName.Truncate(9);
            texts[1].text = "$" + item.StatValue.ToString();
            Debug.Log($"{item.Position} {item.PlayFabId} {item.StatValue}");
        }
        GameManager.Instance.InputObserver.ActivateUI();
    }

    public void ShowLeaderBoard(GetLeaderboardAroundPlayerResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newRow = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = $"{(item.Position + 1)} {item.DisplayName.Truncate(9)}";
            texts[1].text = "$" + item.StatValue.ToString();
            Debug.Log($"{item.Position} {item.PlayFabId} {item.StatValue}");
        }
    }
}
