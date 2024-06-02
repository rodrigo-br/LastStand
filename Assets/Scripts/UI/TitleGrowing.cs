using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleGrowing : MonoBehaviour
{
    [SerializeField] private float scaleTime = 5f;
    [SerializeField] private GameObject startOptions;
    private PlayfabManager playfabManager;
    private Vector3 scaleTarget;
    private bool optionsEnabled;

    private void Awake()
    {
        playfabManager = FindObjectOfType<PlayfabManager>();
        scaleTarget = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.localScale = Vector2.zero;
    }

    private void Start()
    {
        startOptions.SetActive(false);
        transform.DOScale(scaleTarget, scaleTime);
    }

    private void Update()
    {
        if (transform.localScale != scaleTarget || optionsEnabled) return;

        optionsEnabled = true;
        startOptions.SetActive(true);
        if (playfabManager.HasName)
        {
            startOptions.transform.GetChild(0).gameObject.SetActive(false);
            GameObject startButtonObject = startOptions.transform.GetChild(1).gameObject;
            Button startButton = startButtonObject.GetComponent<Button>();
            startButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
            startButtonObject.SetActive(true);
        }
    }
}
