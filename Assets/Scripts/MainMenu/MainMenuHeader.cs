using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuHeader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainMenuName;

    private void OnEnable()
    {
        if (PlayerDataManager.instance == null) return;
            mainMenuName.text = PlayerDataManager.instance.playerData.displayName;
    }
}
