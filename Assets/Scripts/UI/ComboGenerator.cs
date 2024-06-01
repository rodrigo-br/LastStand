using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    private List<GameObject> images;
    private int currentComboIndex = 0;

    private void Awake()
    {
        images = new List<GameObject>();
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            GenerateImage();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AssignInputs());
    }

    private void OnDisable()
    {
        GameManager.Instance.InputObserver.OnGamepadButtonsAction -= CheckComboInput;
    }

    private IEnumerator AssignInputs()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.InputObserver.OnGamepadButtonsAction += CheckComboInput;
    }

    private void CheckComboInput(int buttonIndex)
    {
        GameObject first = images[currentComboIndex].gameObject;
        if (!first.CompareTag(buttonIndex.ToString())) return ;

        first.SetActive(false);
        first.transform.SetParent(null);
        currentComboIndex++;
        if (currentComboIndex == images.Count)
        {
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
        }
    }

    private void GenerateImage()
    {
        GameObject newImage = Instantiate(GetRandomButton(), transform);
        images.Add(newImage);
    }

    private GameObject GetRandomButton()
    {
        return buttons[Random.Range(0, buttons.Length)];
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
