using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{

    [SerializeField] AvatarView avatar;
    [SerializeField] TextMeshProUGUI LevelText;

    [SerializeField] Timer FinishTime;
    [SerializeField] Timer RecordTime;

    [SerializeField] Image[] AchievementImage = new Image[3];
    [SerializeField] Sprite spriteComplete;
    [SerializeField] Sprite spriteIncomplete;

    [SerializeField] ScoreCriteriaDisplay[] criteriaDisplays;

    CanvasGroup group;

    StageManager Manager => StageManager.Instance;

    public virtual void Setup()
    {
        LevelText.text = Manager.StageMission + " - " + Manager.StageId;
        byte crownAchieved = (byte)Manager.CheckAllCriteria();
        uint clearTime = (uint)Mathf.RoundToInt(Manager.time);
        PlayerMissionData missionData = new PlayerMissionData() { isCleared = true, bestDuration = clearTime, bestStars = crownAchieved };

        if (PlayerDataManager.instance)
        {
            PlayerDataManager.instance.SetMissionDataAndSave(
                        (byte)MainMenuState.instance.selectedSubMateri,
                        (byte)MainMenuState.instance.selectedStage,
                        (byte)MainMenuState.instance.selectedMission,
                        MainMenuState.instance.selectedDifficulty,
                        missionData
                    );
        }
        

        for (int i = 0; i < 3; i++)
        {
            int Id = i;
            if(i < crownAchieved)
            {
                DOVirtual.DelayedCall(Id * 0.5f+ 1.5f, delegate 
                {
                    SoundManager.PlaySound(SoundManager.Asset.GetStar);
                    AchievementImage[Id].sprite = spriteComplete; 
                });
                
            }
            else
            {
                AchievementImage[i].sprite = spriteIncomplete;
            }
        }
        FinishTime.SetTime(Manager.time);
        if(!group) group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
        group.DOFade(1f, 1f);
        gameObject.SetActive(true);
        avatar.SetExpression(Expression.happy);
        SoundManager.PlaySound(SoundManager.Asset.Win);
        SetCriteriaDisplay();
    }

    private void SetCriteriaDisplay()
    {
        Criteria[] scoreCriteria = Manager.StarCriteria;
        int minMistake = 0;
        for (int i = criteriaDisplays.Length-1; i>=0; i--)
        {
            if(i == 0)
            {
                criteriaDisplays[i].mistakeText.text = ": " + minMistake + " ~ ";
                criteriaDisplays[i].timeText.SetTimeWithPrefix(scoreCriteria[i+1].time, ": >");
            }
            else
            {
                criteriaDisplays[i].mistakeText.text = ": " + minMistake + " ~ " + scoreCriteria[i].Mistake;
                criteriaDisplays[i].timeText.SetTimeWithPrefix(scoreCriteria[i].time, ": <");
            }
            minMistake = scoreCriteria[i].Mistake + 1;
        }
    }
}

[System.Serializable]
public class ScoreCriteriaDisplay
{
    public TextMeshProUGUI mistakeText;
    public Timer timeText;
}
