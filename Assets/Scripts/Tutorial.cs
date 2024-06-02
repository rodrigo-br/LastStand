using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private List<Transform> tutorialPages;
    private int currentPage;
    private int totalPages;

    private void Awake()
    {
        currentPage = 0;
        tutorialPages = new List<Transform>();
        foreach (Transform item in this.transform)
        {
            tutorialPages.Add(item);
        }
        totalPages = tutorialPages.Count;
        tutorialPages[currentPage].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            currentPage++;
            if (currentPage < totalPages)
            {
                tutorialPages[currentPage].gameObject.SetActive(true);
                return;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
