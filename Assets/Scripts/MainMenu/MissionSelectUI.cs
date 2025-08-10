using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionSelectUI : MainMenuUI
{
    private PlayerDataManager dataManager;

    public GameObject stageSelectMenu;

    public ButtonGroup difficultyButtons;
    public ButtonGroup stageButtons;

    public List<StageSelectView> missionCrowns;

    [SerializeField] private TextMeshProUGUI studyGoalText;
    [SerializeField] [TextArea] private string[] studyGoals;

    public override void OnDisplayed()
    {
        base.OnDisplayed();
        dataManager = PlayerDataManager.instance;
        menuState.OnSelectedDifficultyChanged += MenuState_OnSelectedDifficultyChanged;
        menuState.OnSelectedStageChanged += MenuState_OnSelectedStageChanged;
        UpdateDisplay();
    }

    private void MenuState_OnSelectedStageChanged()
    {
        UpdateDisplay();
    }

    private void MenuState_OnSelectedDifficultyChanged()
    {
        UpdateDisplay();
    }

    public void SelectStage(int idx)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        menuState.SetStage(idx);
    }

    public void SelectDifficulty(int diff)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        menuState.SetDifficulty((Difficulty)diff);
    }

    public void Return()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        stageSelectMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void StartMission(int missionId)
    {
        SceneSwitcher.instance.StartLevel(missionId);
    }

    private void UpdateDisplay()
    {
        List<PlayerLevelData> subjectData;
        List<PlayerMissionData> missionData;
        if (menuState.selectedSubMateri == 0)
            subjectData = dataManager.playerData.firstSubjectData;
        else
            subjectData = dataManager.playerData.secondSubjectData;
        if (subjectData[menuState.selectedStage] == null)
        {
            for (int i = 0; i < missionCrowns.Count; i++)
            {
                missionCrowns[i].SetDisplay(0, 0);
            }
        }
        else
        {
            if (menuState.selectedDifficulty == Difficulty.normal)
                missionData = subjectData[menuState.selectedStage].normalMissions;
            else
                missionData = subjectData[menuState.selectedStage].hardMissions;
            for (int i = 0; i < missionCrowns.Count; i++)
            {
                if(i >= missionData.Count)
                {
                    missionCrowns[i].SetDisplay(0, 3);
                }
                else
                {
                    missionCrowns[i].SetDisplay(missionData[i]);
                }
            }
        }
        difficultyButtons.SetSelected((int)menuState.selectedDifficulty);
        stageButtons.SetSelected(menuState.selectedStage);
        studyGoalText.text = studyGoals[menuState.selectedStage + (menuState.selectedSubMateri*3)];
    }
}
