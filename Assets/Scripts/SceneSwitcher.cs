using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;
    public GameObject mainMenu;
    public string loadedLevel;
    

    //this class should have an access to a database which tells which mission is which type of game

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void StartLevel(int missionNumber)
    {
        //in the future, also check the subject, stage, and difficulty to determine which game scene should be loaded
        MainMenuState.instance.SetMission(missionNumber);
        loadedLevel = "Stage " + ((MainMenuState.instance.selectedSubMateri * 3) + MainMenuState.instance.selectedStage + 1) + "-" + (missionNumber + 1);

        //if (missionNumber == 0)
        //{
        //    loadedLevel = "SceneStageJigsaw";
        //}
        //else if(missionNumber == 1)
        //{
        //    loadedLevel = "SceneStageMatch";
        //}
        //else if(missionNumber == 2)
        //{
        //    loadedLevel = "SceneStageCrossword";
        //}
        //else
        //{
        //    loadedLevel = "SceneStageFindSpot";
        //}

        SceneManager.LoadScene(loadedLevel, LoadSceneMode.Additive);
        mainMenu.gameObject.SetActive(false);
    }

    public void GoToMenu()
    {
        mainMenu.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(loadedLevel);
    }
}
