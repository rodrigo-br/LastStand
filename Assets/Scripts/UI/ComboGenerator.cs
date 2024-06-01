using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    private List<GameObject> images;

    private void Awake()
    {
        images = new List<GameObject>();
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GenerateImage();
        }
    }

    private void GenerateImage()
    {
        GameObject newImage = Instantiate(GetRandomButton(), transform);
        images.Add(newImage);
    }

    private GameObject GetRandomButton()
    {
        return buttons[Random.Range(0, buttons.Length - 1)];
    }

    public void ShuffleMe(List<GameObject> list)
    {
        System.Random random = new System.Random();

        for (int i = list.Count - 1; i > 1; i--)
        {
            int rnd = random.Next(i + 1);

            GameObject value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }
}
