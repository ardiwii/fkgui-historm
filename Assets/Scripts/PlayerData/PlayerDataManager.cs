using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

/// <summary>
/// add this class to the main singleton manager class for playerdata management
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;

    public PlayerData playerData;
    public Action afterLoadAction;
    
    private Dictionary<string, string> failedSaveData;

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void LoadData()
    {
        string jsonData = EncryptedPlayerPrefs.GetString("playerData");
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        Debug.Log("player data loaded from playerpref");
    }

    public void SaveData()
    {
        //playerData.timestamp = DateTime.UtcNow;
        string jsonData = JsonUtility.ToJson(playerData);
        EncryptedPlayerPrefs.SetString("playerData", jsonData);
        Debug.Log("player data saved to playerpref");
    }

    public void LoadPlayerData(Action afterLoad)
    {
        afterLoadAction = afterLoad;
        LoadPlayerData();
    }
    
    private void LoadPlayerData()
    {
        PlayerProfileViewConstraints profileConstraints = new PlayerProfileViewConstraints() { ShowDisplayName = true, ShowAvatarUrl = true };
        GetPlayerCombinedInfoRequestParams param = new GetPlayerCombinedInfoRequestParams() { GetPlayerProfile = true, GetUserData = true, GetPlayerStatistics = true,  ProfileConstraints = profileConstraints };
        var req = new GetPlayerCombinedInfoRequest() { InfoRequestParameters = param };
        PlayFabClientAPI.GetPlayerCombinedInfo(req, OnLoadSuccess, OnLoadFailed);
    }

    private void OnLoadSuccess(GetPlayerCombinedInfoResult obj)
    {
        playerData.playfabId = obj.PlayFabId;
        playerData.displayName = obj.InfoResultPayload.PlayerProfile.DisplayName;
        var data = obj.InfoResultPayload.UserData;
        playerData.Initialize();
        for (int i = 0; i < 3; i++)
        {
            if (data.ContainsKey("data1-" + (i + 1)))
                playerData.firstSubjectData[i] = JsonUtility.FromJson<PlayerLevelData>(data["data1-" + (i + 1)].Value);
        }
        for (int i = 0; i < 3; i++)
        {
            if (data.ContainsKey("data2-" + (i + 1)))
                playerData.secondSubjectData[i] = JsonUtility.FromJson<PlayerLevelData>(data["data2-" + (i + 1)].Value);
        }
        if (!string.IsNullOrEmpty(obj.InfoResultPayload.PlayerProfile.AvatarUrl))
        {
            AvatarManager.instance.playerAvatarData = new Avatar(obj.InfoResultPayload.PlayerProfile.AvatarUrl);
            AvatarManager.instance.isAvatarLoaded = true;
        }
        afterLoadAction();
    }

    private void OnLoadFailed(PlayFabError obj)
    {
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowErrorPopup(obj.ErrorMessage, "retry", LoadPlayerData);
    }

    #region post mission save data

    /// <summary>
    /// set mission data with the new score and returns the level data of the level of the edited mission
    /// </summary>
    /// <param name="subjectGroup">subject group of the new mission</param>
    /// <param name="stage">level of the mission</param>
    /// <param name="mission">mission with the new data.</param>
    /// <param name="difficulty">difficulty of the mission</param>
    /// <param name="newData">new data to be set to the live player data</param>
    /// <returns></returns>
    public void SetMissionDataAndSave(byte subjectGroup, byte stage, byte mission, Difficulty difficulty, PlayerMissionData newData)
    {
        PlayerMissionData currentMissionData = GetMissionData(subjectGroup, stage, mission, difficulty);
        uint currBestDuration = currentMissionData.bestDuration == 0? 99999999 : currentMissionData.bestDuration;
        byte currBestCrown = currentMissionData.bestStars;

        if (currBestDuration < newData.bestDuration) newData.bestDuration = currBestDuration;
        if (currBestCrown > newData.bestStars) newData.bestStars = currBestCrown;
        
        if (subjectGroup == 0)
        {
            if (difficulty == Difficulty.normal)
                playerData.firstSubjectData[stage].normalMissions[mission] = newData;
            else
                playerData.firstSubjectData[stage].hardMissions[mission] = newData;
        }
        else
        {
            if (difficulty == Difficulty.normal)
                playerData.secondSubjectData[stage].normalMissions[mission] = newData;
            else
                playerData.secondSubjectData[stage].hardMissions[mission] = newData;
        }
        Debug.Log("saving data, subject group: " + subjectGroup + " stage: " + stage + " mission: " + mission);
        Dictionary<string, string> updatedData = new Dictionary<string, string>();
        updatedData.Add("data" + (subjectGroup+1) + "-" + (stage+1), playerData.GetJSONData(subjectGroup, stage));
        SaveGameDataPostMission(updatedData);
    }

    public void SaveGameDataPostMission(Dictionary<string, string> updatedData)
    {
        failedSaveData = updatedData;
        var req = new UpdateUserDataRequest() { Data = updatedData };
        PlayFabClientAPI.UpdateUserData(req, onPostMissionSaveSuccess, onPostMissionSaveFailed);
        PopupManager.instance.ShowSmallNotif("saving data", 3f);
    }

    private void RetryPostMissionSave()
    {
        if (failedSaveData == null)
        {
            Debug.LogError("no failed save data to retry");
        }
        var req = new UpdateUserDataRequest() { Data = failedSaveData };
        PlayFabClientAPI.UpdateUserData(req, onSaveSuccess, onSaveFailed);
        PopupManager.instance.ShowSmallNotif("saving data", 3f);
    }

    private void onPostMissionSaveFailed(PlayFabError obj)
    {
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowErrorPopup(obj.ErrorMessage, "retry", RetryPostMissionSave);
    }

    private void onPostMissionSaveSuccess(UpdateUserDataResult obj)
    {
        failedSaveData = null;
        UpdateLeaderboardStatistic();
    }

    private void UpdateLeaderboardStatistic()
    {
        int totalBestClearTime = (int)playerData.GetTotalBestTime();
        int totalScore = (playerData.GetTotalMissionCleared() * 1000) + playerData.GetTotalCrown();

        string scoreKey = playerData.subjectGroup == 1 ? "FirstGroupScore" : "SecondGroupScore";

        List<StatisticUpdate> statUpdate = new List<StatisticUpdate>();
        statUpdate.Add(new StatisticUpdate() { StatisticName = scoreKey, Value = totalScore });
        statUpdate.Add(new StatisticUpdate() { StatisticName = "TotalBestClearTime", Value = totalBestClearTime });

        var req = new UpdatePlayerStatisticsRequest() { Statistics = statUpdate };
        PlayFabClientAPI.UpdatePlayerStatistics(req, onSaveStatSuccess, onSaveStatFailed);
    }

    private void onSaveStatSuccess(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("updating leaderboard statistic success");
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowSmallNotif("save success", 3f);
    }

    private void onSaveStatFailed(PlayFabError obj)
    {
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowErrorPopup(obj.ErrorMessage, "retry", UpdateLeaderboardStatistic);
    }

    private PlayerMissionData GetMissionData(byte subjectGroup, byte stage, byte mission, Difficulty difficulty)
    {
        if (subjectGroup == 1)
        {
            if (difficulty == Difficulty.normal)
                return playerData.firstSubjectData[stage].normalMissions[mission];
            else
                return playerData.firstSubjectData[stage].hardMissions[mission];
        }
        else
        {
            if (difficulty == Difficulty.normal)
                return playerData.secondSubjectData[stage].normalMissions[mission];
            else
                return playerData.secondSubjectData[stage].hardMissions[mission];
        }
    }

    #endregion

    #region generic playfab saving

    public void SaveAllDataToPlayfab()
    {
        Dictionary<string, string> updatedData = new Dictionary<string, string>();
        updatedData.Add("avatar", JsonUtility.ToJson(AvatarManager.instance.playerAvatarData));
        updatedData.Add("data1-1", playerData.GetJSONData(0, 0));
        updatedData.Add("data1-2", playerData.GetJSONData(0, 1));
        updatedData.Add("data1-3", playerData.GetJSONData(0, 2));
        updatedData.Add("data2-1", playerData.GetJSONData(1, 0));
        updatedData.Add("data2-2", playerData.GetJSONData(1, 1));
        updatedData.Add("data2-3", playerData.GetJSONData(1, 2));
        failedSaveData = updatedData;
        var req = new UpdateUserDataRequest() { Data = updatedData };
        PlayFabClientAPI.UpdateUserData(req, onSaveSuccess, onSaveFailed);
        PopupManager.instance.ShowSmallNotif("saving data", 3f);
    }

    public void SaveDataToPlayfab(Dictionary<string, string> updatedData)
    {
        failedSaveData = updatedData;
        var req = new UpdateUserDataRequest() { Data = updatedData };
        PlayFabClientAPI.UpdateUserData(req, onSaveSuccess, onSaveFailed);
        PopupManager.instance.ShowSmallNotif("saving data", 3f);
    }

    public void RetrySaveDataToPlayfab()
    {
        if(failedSaveData == null)
        {
            Debug.LogError("no failed save data to retry");
        }
        var req = new UpdateUserDataRequest() { Data = failedSaveData };
        PlayFabClientAPI.UpdateUserData(req, onSaveSuccess, onSaveFailed);
        PopupManager.instance.ShowSmallNotif("saving data", 3f);
    }

    private void onSaveFailed(PlayFabError obj)
    {
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowErrorPopup(obj.ErrorMessage, "retry", RetrySaveDataToPlayfab);
    }

    private void onSaveSuccess(UpdateUserDataResult obj)
    {
        failedSaveData = null;
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowSmallNotif("save success", 3f);
    }

    #endregion
}


public enum Difficulty
{
    normal,
    hard
}