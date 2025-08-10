using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionLoader : MonoBehaviour
{
    public void SetButtonEvent(Button But,int MissionId,int StageId)
    {
        But.onClick.RemoveAllListeners();
        But.onClick.AddListener(delegate { GoToStage(MissionId, StageId); });
    }

    void GoToStage(int MissionId, int StageId)
    {
        string Name = "Stage " + MissionId + "-" + StageId;
        SceneManager.LoadScene(Name);
    }
}
