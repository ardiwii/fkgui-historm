using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TitleTex;
    [SerializeField] TextMeshProUGUI StageTex;

    StageManager Manager => StageManager.Instance;

    private void Start()
    {
        TitleTex.text = Manager.StageName;
        StageTex.text = "Stage " + Manager.StageMission+" - " + Manager.StageId + " (Normal)";
    }
}
