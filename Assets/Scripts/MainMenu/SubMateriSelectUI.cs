using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubMateriSelectUI : MainMenuUI
{
    public GameObject selectMissionMenu;
    public StageSelectView stage1Crown;
    public StageSelectView stage2Crown;
    public StageSelectView stage3Crown;

    public ButtonGroup difficultyButtons;
    public ButtonGroup subMateriButtons;

    private PlayerDataManager dataManager;

    public override void OnDisplayed()
    {
        base.OnDisplayed();
        dataManager = PlayerDataManager.instance;
        menuState.OnSelectedSubMateriChanged += MenuState_OnSelectedSubMateriChanged;
        menuState.OnSelectedDifficultyChanged += MenuState_OnSelectedDifficultyChanged;
        UpdateDisplay();
    }

    private void MenuState_OnSelectedSubMateriChanged()
    {
        UpdateDisplay();
    }

    private void MenuState_OnSelectedDifficultyChanged()
    {
        UpdateDisplay();
    }

    public void SelectSubMateri(int idx)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        menuState.SetSubMateri(idx);
    }

    public void SelectDifficulty(int idx)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        menuState.SetDifficulty((Difficulty) idx);
    }

    public void SelectStage(int idx)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        menuState.SetStage(idx);
        gameObject.SetActive(false);
        selectMissionMenu.SetActive(true);
    }

    private void UpdateDisplay()
    {
        if (menuState.selectedSubMateri == 0)
        {
            List<PlayerLevelData> subjectData = dataManager.playerData.firstSubjectData;
            stage1Crown.SetDisplay(subjectData[0], menuState.selectedDifficulty);
            stage2Crown.SetDisplay(subjectData[1], menuState.selectedDifficulty);
            stage3Crown.SetDisplay(subjectData[2], menuState.selectedDifficulty);
        }
        else
        {
            List<PlayerLevelData> subjectData = dataManager.playerData.secondSubjectData;
            stage1Crown.SetDisplay(subjectData[0],menuState.selectedDifficulty);
            stage2Crown.SetDisplay(subjectData[1],menuState.selectedDifficulty);
            stage3Crown.SetDisplay(subjectData[2],menuState.selectedDifficulty);
        }
        difficultyButtons.SetSelected((int)menuState.selectedDifficulty);
        subMateriButtons.SetSelected(menuState.selectedSubMateri);
    }
}
