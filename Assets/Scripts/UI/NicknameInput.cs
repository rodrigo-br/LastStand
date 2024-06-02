using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NicknameInput : MonoBehaviour
{
    [SerializeField] private Button startButton;
    private TMP_InputField nicknameInput;

    private void Awake()
    {
        nicknameInput = GetComponent<TMP_InputField>();
        nicknameInput.onValueChanged.AddListener(CheckNicknameSize);
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        startButton.gameObject.SetActive(false);
    }

    private void CheckNicknameSize(string nick)
    {
        startButton.gameObject.SetActive(nick.Length >= 3);
    }

    private void StartGame()
    {
        FindObjectOfType<PlayfabManager>().SubmitName();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
