using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : MonoBehaviour
{
    public static MainMenuState instance;

    public int selectedSubMateri { get; private set; }
    public Difficulty selectedDifficulty { get; private set; }
    public int selectedStage { get; private set; }
    public int selectedMission { get; private set; }

    public delegate void StateChangeDelegate();
    public event StateChangeDelegate OnSelectedSubMateriChanged;
    public event StateChangeDelegate OnSelectedStageChanged;
    public event StateChangeDelegate OnSelectedDifficultyChanged;

    private void OnEnable()
    {
        instance = this;
        SetSubMateri(1);
        SetDifficulty(Difficulty.normal);
        SetStage(0);
        Debug.unityLogger.logEnabled = false;
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void SetMission(int missionId)
    {
        selectedMission = missionId;
    }

    public void SetSubMateri(int subMateriId)
    {
        selectedSubMateri = subMateriId;
        OnSelectedSubMateriChanged?.Invoke();
    }

    public void SetStage(int stageId)
    {
        selectedStage = stageId;
        OnSelectedStageChanged?.Invoke();
    }

    public void SetDifficulty(Difficulty diff)
    {
        selectedDifficulty = diff;
        OnSelectedDifficultyChanged?.Invoke();
    }
}
