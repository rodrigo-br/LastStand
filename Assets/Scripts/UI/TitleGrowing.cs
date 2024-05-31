using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleGrowing : MonoBehaviour
{
    [SerializeField] private float scaleTime = 5f;
    [SerializeField] private GameObject startOptions;
    private Vector3 scaleTarget;
    private bool optionsEnabled;

    private void Awake()
    {
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
    }
}
