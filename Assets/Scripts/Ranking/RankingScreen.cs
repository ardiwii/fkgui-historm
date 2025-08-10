using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class RankingScreen : MonoBehaviour
{
    [SerializeField] private PlayerRankStatDisplay currentPlayerRankDisplay;
    [SerializeField] private List<PlayerRankStatDisplay> playerRankDisplays;
    [SerializeField] private TextMeshProUGUI pageDisplay;
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject prevPageButton;
    [SerializeField] private TextMeshProUGUI molaraText;

    private List<int> tiedRank;
    private List<PlayerLeaderboardEntry> leaderboardEntry;
    private int playerPosition;
    private int playerRank;
    private int currentShownPage;
    private const int ENTRY_PER_PAGE = 6;
    private int pageCount;

    private void OnEnable()
    {
        GetLeaderboard();
    }

    private void GetLeaderboard()
    {
        PlayerProfileViewConstraints constraints = new PlayerProfileViewConstraints() { ShowDisplayName = true, ShowLastLogin = true, ShowStatistics = true, ShowAvatarUrl = true };
        string statName = PlayerDataManager.instance.playerData.subjectGroup == 1 ? "FirstGroupScore" : "SecondGroupScore";
        var req = new GetLeaderboardRequest() { MaxResultsCount = 100, StatisticName = statName, ProfileConstraints = constraints };
        PlayFabClientAPI.GetLeaderboard(req, onGetLeaderboard, onLoadLeaderboardFailed);
        PopupManager.instance.ShowStatusPopup("Loading leaderboard..");
    }

    private void onGetLeaderboard(GetLeaderboardResult obj)
    {
        PopupManager.instance.CloseStatusPopup();
        leaderboardEntry = obj.Leaderboard;
        int previousValue = 99999;
        int previousTiedRank = 0;
        tiedRank = new List<int>();
        playerRank = -1;
        playerPosition = -1;
        for (int i = 0; i < leaderboardEntry.Count; i++)
        {
            if(leaderboardEntry[i].StatValue == previousValue)
            {
                tiedRank.Add(previousTiedRank);
            }
            else
            {
                tiedRank.Add(i);
                previousTiedRank = i;
                previousValue = leaderboardEntry[i].StatValue;
            }

            Debug.Log(leaderboardEntry[i].PlayFabId);
            if(leaderboardEntry[i].PlayFabId == PlayerDataManager.instance.playerData.playfabId)
            {
                playerPosition = i;
                playerRank = tiedRank[i];
            }
        }

        if(playerRank == -1)
        {
            SetDisplayForUnranked(currentPlayerRankDisplay);
        }
        else
        {
            SetDisplayWithEntry(leaderboardEntry[playerPosition], currentPlayerRankDisplay);
        }

        pageCount = leaderboardEntry.Count / ENTRY_PER_PAGE;
        if (leaderboardEntry.Count % ENTRY_PER_PAGE > 0) pageCount++;
        currentShownPage = 1;
        DisplayCurrentPage(currentShownPage);
        SetMolaraText();
    }

    private void SetMolaraText()
    {
        string text = "";
        if (playerRank == -1)
        {
            text = "Mainkan satu misi untuk membuka peringkatmu.";
        }
        else if (playerRank == 0)
        {
            text = "Ayo pertahankan peringkatmu agar selalu menjadi yang terdepan.";
        }
        else if (playerRank > 0 && playerRank < 5)
        {
            text = "Sedikit lagi kamu menjadi yang terdepan diantara teman-temanmu.";
        }
        else if (playerRank >= 5 && playerRank < 15)
        {
            text = "Peringkatmu sudah mulai naik, ayo terus selesaikan lebih banyak misi lagi.";
        }
        else
        {
            text = "Selesaikan lebih banyak misi dan peroleh banyak gigi emas untuk meningkatkan rankingmu.";
        }
        molaraText.text = text;
    }
    
    private void onLoadLeaderboardFailed(PlayFabError obj)
    {
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowErrorPopup(obj.ErrorMessage, "retry", GetLeaderboard);
    }

    private void DisplayCurrentPage(int pageNumber)
    {
        int start = (pageNumber - 1) * ENTRY_PER_PAGE;
        int end = start + ENTRY_PER_PAGE;
        if (end > leaderboardEntry.Count)
        {
            end = leaderboardEntry.Count;
        }
        nextPageButton.SetActive(end < leaderboardEntry.Count);
        prevPageButton.SetActive(start > 0);
        pageDisplay.text = pageNumber.ToString() + "/" + pageCount;

        int displayId = 0;

        for (int i = start; i < end; i++)
        {
            if (!playerRankDisplays[displayId].gameObject.activeInHierarchy) playerRankDisplays[displayId].gameObject.SetActive(true);
            SetDisplayWithEntry(leaderboardEntry[i], playerRankDisplays[displayId]);
            displayId++;
        }

        if (displayId < 6) //disable any row that is outside the entry
        {
            for (int i = displayId; i < 6; i++)
            {
                playerRankDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetDisplayForUnranked(PlayerRankStatDisplay display)
    {
        int rank = -1;
        int clearedMission = 0;
        int crownCount = 0;
        int totalClearTime = 0;
        display.SetDisplay(rank, "", "", "", clearedMission, crownCount, totalClearTime, true);
    }

    private void SetDisplayWithEntry(PlayerLeaderboardEntry entry, PlayerRankStatDisplay display)
    {
        string displayName = entry.Profile.DisplayName;
        DateTime? lastLogin = entry.Profile.LastLogin;
        string avatar = entry.Profile.AvatarUrl;
        int rank = tiedRank[entry.Position];
        int score = entry.StatValue;
        int clearedMission = score / 1000;
        int crownCount = score % 1000;
        List<StatisticModel> stats = entry.Profile.Statistics;
        int totalClearTime = 0;
        for (int j = 0; j < stats.Count; j++)
        {
            if (stats[j].Name.Equals("TotalBestClearTime"))
            {
                totalClearTime = stats[j].Value;
            }
        }
        display.SetDisplay(rank, avatar, displayName, GetLastLoginString(lastLogin), clearedMission, crownCount, totalClearTime, entry.Position == playerPosition);
    }

    public void CheckPlayerPosition()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        if (playerPosition > -1)
        {
            currentShownPage = (playerPosition / ENTRY_PER_PAGE) + 1;
            DisplayCurrentPage(currentShownPage);
        }
    }

    public void NextPage()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        currentShownPage++;
        DisplayCurrentPage(currentShownPage);
    }

    public void PrevPage()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        currentShownPage--;
        DisplayCurrentPage(currentShownPage);
    }

    private string GetLastLoginString(DateTime? lastLoginTime)
    {
        return "";
    }
}

public enum MedalType
{
    Unranked,
    Gold,
    Silver,
    Bronze,
    Standard
}
