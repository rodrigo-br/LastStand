using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;
using System.Collections;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance = null;
    [SerializeField] private GameObject nameWindow;
    [SerializeField] private GameObject startWindow;
    [SerializeField] private TextMeshProUGUI nameInput;
    private bool hasName = false;
    public bool HasName => hasName;
    private const string UID_KEY = "UniqueID";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Login();
    }

    private void Login()
    {
        string uniqueID = GetOrCreateUID();
        var request = new LoginWithCustomIDRequest
        {
            CustomId = uniqueID,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccessLogin, OnError);
    }

    private string GetOrCreateUID()
    {
        if (PlayerPrefs.HasKey(UID_KEY))
        {
            return PlayerPrefs.GetString(UID_KEY);
        }

        string newUID = Guid.NewGuid().ToString();
        PlayerPrefs.SetString(UID_KEY, newUID);

        return newUID;
    }

    private void OnSuccessLogin(LoginResult result)
    {
        Debug.Log("Successfuly login!");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }

        hasName = name != null;
        Debug.Log($"HAS NAME : {hasName}");
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error on Playfab Action!");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "CurrencyScore",
                    Value = score,
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Sucessfuly update leaderboard");
        PlayfabManager.Instance.Invoke("GetLeaderBoard", 1f);
    }

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "CurrencyScore",
            StartPosition = 0,
            MaxResultsCount = 6,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        Debug.Log("Successfuly get leaderboard");
        UIManager uIManager = FindObjectOfType<UIManager>();
        if (uIManager == null) { return; }

        uIManager.ShowLeaderBoard(result);
    }

    public void GetLeaderBoardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "CurrencyScore",
            MaxResultsCount = 6,
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }

    private void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        Debug.Log("Successfuly get leaderboard around player");
        UIManager uIManager = FindObjectOfType<UIManager>();
        if (uIManager == null) { return; }

        uIManager.ShowLeaderBoard(result);
    }

    public void SubmitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
    }
}
