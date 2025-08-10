using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayMainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalCrownText;

    public void BackToMenu()
    {
        SceneSwitcher.instance.GoToMenu();
    }

    private void OnEnable()
    {
        totalCrownText.text = PlayerDataManager.instance.playerData.GetTotalCrown().ToString();
    }

    public void ActivateHint()
    {
        StageManager.Instance.ActivateHint();
    }
}
