using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Image panelImage;
    private List<GameObject> images;
    private int currentComboIndex = 0;
    public static event Action OnMissCombo;

    private void Awake()
    {
        images = new List<GameObject>();
    }

    private void Start()
    {
        GenerateImage();
    }

    private void OnEnable()
    {
        StartCoroutine(AssignInputs());
    }

    private void OnDisable()
    {
        GameManager.Instance.InputObserver.OnGamepadButtonsAction -= CheckComboInput;
        GameManager.OnResetLevel -= ResetLevel;
    }

    private IEnumerator AssignInputs()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.InputObserver.OnGamepadButtonsAction += CheckComboInput;
        GameManager.OnResetLevel += ResetLevel;
    }

    private void CheckComboInput(int buttonIndex)
    {
        GameObject first = images[currentComboIndex].gameObject;
        if (!first.CompareTag(buttonIndex.ToString()))
        {
            GameManager.Instance.AudioManager.PlayIncorrectComboClip();
            images[currentComboIndex].transform.DOShakePosition(1f, strength: 6);
            OnMissCombo?.Invoke();
            return;
        }
        GameManager.Instance.AudioManager.PlayCorrectComboClip();
        first.SetActive(false);
        first.transform.SetParent(null);
        currentComboIndex++;
        if (currentComboIndex == images.Count)
        {
            panelImage.enabled = false;
            GameManager.Instance.InputObserver.DeactivateComboSequence();
        }
    }

    private void ResetLevel(bool isDefeat)
    {
        if (isDefeat)
        {
            for (int i = images.Count - 1; i >= 0 ; i--)
            {
                Destroy(images[i]);
                images.RemoveAt(i);
            }
        }
        panelImage.enabled = true;

        GenerateImage();
        ShuffleMe();
        foreach (var image in images)
        {
            image.transform.SetParent(null);
        }
        foreach (var image in images)
        {
            image.transform.SetParent(this.transform);
            image.SetActive(true);
        }
        currentComboIndex = 0;
        GameManager.Instance.InputObserver.ActivateComboSequence();
    }

    private void GenerateImage()
    {
        GameObject newImage = Instantiate(GetRandomButton(), transform);
        images.Add(newImage);
    }

    private GameObject GetRandomButton()
    {
        return buttons[UnityEngine.Random.Range(0, buttons.Length)];
    }

    public void ShuffleMe()
    {
        System.Random random = new System.Random();

        for (int i = images.Count - 1; i > 1; i--)
        {
            int rnd = random.Next(i + 1);

            GameObject value = images[rnd];
            images[rnd] = images[i];
            images[i] = value;
        }
    }
}
