using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public InputObserver InputObserver { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            InputObserver = new InputObserver();
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}