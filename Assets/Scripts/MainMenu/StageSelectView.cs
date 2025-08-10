using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelectView : MonoBehaviour
{
    public TextMeshProUGUI crownText;
    public Timer bestClearTime;

    public void SetDisplay(int obtainedCrown, int maxCrown)
    {
        crownText.text = obtainedCrown + "/" + maxCrown;
    }

    public void SetDisplay(PlayerLevelData stageData, Difficulty diff)
    {
        if (stageData == null)
            crownText.text = "0/0";
        else
        {
            int bestCrown = stageData.GetTotalCrown(diff);
            int maxCrown = stageData.GetMaxCrown(diff);
            crownText.text = bestCrown + "/" + maxCrown;
        }
    }

    public void SetDisplay(PlayerMissionData missionData)
    {
        int bestCrown = missionData.bestStars;
        int maxCrown = 3;
        crownText.text = bestCrown + "/" + maxCrown;
        if (missionData.isCleared)
        {
            bestClearTime.gameObject.SetActive(true);
            bestClearTime.SetTime(missionData.bestDuration);
        }
        else
        {
            bestClearTime.gameObject.SetActive(false);
        }
    }
}
