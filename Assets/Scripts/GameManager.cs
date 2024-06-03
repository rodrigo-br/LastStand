using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public InputObserver InputObserver { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public static event Action<bool> OnResetLevel;
    public bool isResetingLevel = false;
    private bool isDefeat = false;
    private bool canReset = false;

    private void Awake()
    {
        if (Instance == null)
        {
            InputObserver = new InputObserver();
            AudioManager = FindObjectOfType<AudioManager>();
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        UIManager.OnEndOfTime += ResetLevel;
        Bullet.OnHitChar += ResetLevel;
        InputObserver.OnSelectAction += SetCanReset;
    }

    private void OnDisable()
    {
        UIManager.OnEndOfTime -= ResetLevel;
        Bullet.OnHitChar -= ResetLevel;
        InputObserver.OnSelectAction -= SetCanReset;
    }

    private void ResetLevel()
    {
        if (isResetingLevel) { return; }
        isResetingLevel = true;
        StartCoroutine(ResetLevelCoroutine());
    }

    private void SetCanReset()
    {
        canReset = true;
    }

    private IEnumerator ResetLevelCoroutine()
    {
        if (!isDefeat)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitUntil(() => canReset);
        }
        canReset = false;
        OnResetLevel?.Invoke(isDefeat);
        yield return new WaitForSeconds(0.5f);
        isResetingLevel = false;
        SetDefeat(false);
    }

    public void SetDefeat(bool defeat)
    {
        isDefeat = defeat;
    }
}