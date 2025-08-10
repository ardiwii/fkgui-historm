using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public string playfabId;
    public string displayName;
    public byte subjectGroup; //subject 1 or 2
    public List<PlayerLevelData> firstSubjectData; //fixed size of STAGE_COUNT
    public List<PlayerLevelData> secondSubjectData; //fixed size of STAGE_COUNT

    const int CURRENT_SUBJECT_GROUP = 2;
    const int STAGE_COUNT = 3;
    const int MISSION_COUNT = 5;

    /// <summary>
    /// get the json data of a level in a subjectgroup
    /// </summary>
    /// <param name="subjectGroup"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public string GetJSONData(byte subjectGroup, byte level)
    {
        if (subjectGroup == 0)
        {
            if (firstSubjectData.Count >= level)
            {
                string json = JsonUtility.ToJson(firstSubjectData[level]);
                Debug.Log("getting json data of " + subjectGroup + "-" + level + ": " + json);
                return json;
            }
            else
            {
                return "";
            }
        }
        else
        {
            if (secondSubjectData.Count >= level)
            {
                string json = JsonUtility.ToJson(secondSubjectData[level]);
                Debug.Log("getting json data of " + subjectGroup + "-" + level + ": " + json);
                return json;
            }
            else
            {
                return "";
            }
        }
    }

    public void Initialize()
    {
        subjectGroup = CURRENT_SUBJECT_GROUP;
        AvatarManager.instance.isAvatarLoaded = false;
        firstSubjectData = new List<PlayerLevelData>();
        for (int i = 0; i < STAGE_COUNT; i++)
        {
            firstSubjectData.Add(new PlayerLevelData(MISSION_COUNT));
        }
        secondSubjectData = new List<PlayerLevelData>();
        for (int i = 0; i < STAGE_COUNT; i++)
        {
            secondSubjectData.Add(new PlayerLevelData(MISSION_COUNT));
        }
    }

    public int GetTotalCrown()
    {
        int totalCrown = 0;
        for (int i = 0; i < firstSubjectData.Count; i++)
        {
            if (firstSubjectData[i] != null)
            {
                totalCrown += firstSubjectData[i].GetTotalCrown(Difficulty.normal);
                totalCrown += firstSubjectData[i].GetTotalCrown(Difficulty.hard);
            }
        }
        for (int i = 0; i < secondSubjectData.Count; i++)
        {
            if (secondSubjectData[i] != null)
            {
                totalCrown += secondSubjectData[i].GetTotalCrown(Difficulty.normal);
                totalCrown += secondSubjectData[i].GetTotalCrown(Difficulty.hard);
            }
        }
        return totalCrown;
    }

    public uint GetTotalBestTime()
    {
        uint totalBestTime = 0;
        for (int i = 0; i < firstSubjectData.Count; i++)
        {
            if (firstSubjectData[i] != null)
            {
                totalBestTime += firstSubjectData[i].GetTotalBestTime();
            }
        }
        for (int i = 0; i < secondSubjectData.Count; i++)
        {
            if (secondSubjectData[i] != null)
            {
                totalBestTime += secondSubjectData[i].GetTotalBestTime();
            }
        }
        return totalBestTime;
    }

    public int GetTotalMissionCleared()
    {
        int totalMissionCleared = 0;
        for (int i = 0; i < firstSubjectData.Count; i++)
        {
            if (firstSubjectData[i] != null)
            {
                totalMissionCleared += firstSubjectData[i].GetMissionCleared();
            }
        }
        for (int i = 0; i < secondSubjectData.Count; i++)
        {
            if (secondSubjectData[i] != null)
            {
                totalMissionCleared += secondSubjectData[i].GetMissionCleared();
            }
        }
        return totalMissionCleared;
    }
}

[Serializable]
public class PlayerLevelData
{
    public List<PlayerMissionData> normalMissions;
    public List<PlayerMissionData> hardMissions;

    public PlayerLevelData(int missionCount)
    {
        normalMissions = new List<PlayerMissionData>();
        for (int i = 0; i < missionCount; i++)
        {
            normalMissions.Add(new PlayerMissionData() { isCleared = false } );
        }
        hardMissions = new List<PlayerMissionData>();
        for (int i = 0; i < missionCount; i++)
        {
            hardMissions.Add(new PlayerMissionData() { isCleared = false });
        }
    }

    public int GetTotalCrown(Difficulty diff)
    {
        int totalCrown = 0;
        if (diff == Difficulty.normal)
        {
            if (normalMissions == null) return totalCrown;
            for (int i = 0; i < normalMissions.Count; i++)
            {
                totalCrown += normalMissions[i].bestStars;
            }
        }
        else
        {
            if (hardMissions == null) return totalCrown;
            for (int i = 0; i < hardMissions.Count; i++)
            {
                totalCrown += hardMissions[i].bestStars;
            }

        }
        return totalCrown;
    }

    public int GetMaxCrown(Difficulty diff)
    {
        if (diff == Difficulty.normal)
        {
            if (normalMissions == null) return 0;
            return normalMissions.Count * 3;
        }
        else
        {
            if (hardMissions == null) return 0;
            return hardMissions.Count * 3;
        }
    }

    public uint GetTotalBestTime()
    {
        uint totalBestTime = 0;
        if (normalMissions != null)
        {
            for (int i = 0; i < normalMissions.Count; i++)
            {
                totalBestTime += normalMissions[i].bestDuration;
            }
        }
        if (hardMissions != null)
        {
            for (int i = 0; i < hardMissions.Count; i++)
            {
                totalBestTime += hardMissions[i].bestStars;
            }
        }
        return totalBestTime;
    }

    public int GetMissionCleared()
    {
        int totalMissionCleared = 0;
        if (normalMissions != null)
        {
            for (int i = 0; i < normalMissions.Count; i++)
            {
                if (normalMissions[i].isCleared) totalMissionCleared++;
            }
        }
        if (hardMissions != null)
        {
            for (int i = 0; i < hardMissions.Count; i++)
            {
                if (hardMissions[i].isCleared) totalMissionCleared++;
            }
        }
        return totalMissionCleared;
    }
}

[Serializable]
public struct PlayerMissionData
{
    public bool isCleared;
    public uint bestDuration;
    public byte bestStars;
}

[Serializable]
public struct MainData
{
    public ulong playtime;
    public int totalScore;
}
