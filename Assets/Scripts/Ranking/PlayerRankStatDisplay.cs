using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerRankStatDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private AvatarView avatarView;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private TextMeshProUGUI lastLoginText;
    [SerializeField] private TextMeshProUGUI clearedMissionText;
    [SerializeField] private TextMeshProUGUI crownCountText;
    [SerializeField] private Timer totalClearTime;
    [SerializeField] private GameObject currentPlayerArrow;

    [SerializeField] private Image bg;
    [SerializeField] private List<Sprite> rowBg;

    public void SetDisplay(int rank, string avatarStr, string displayName, string lastlogin, int clearedMission, int crownCount, int clearTime, bool isLocalPlayer)
    {
        if (rank == -1)
            rankText.text = "-";
        else
            rankText.text = (rank+1).ToString();
        if (avatarView != null)
        {
            if (string.IsNullOrEmpty(avatarStr)) avatarStr = "0000000000";
            AvatarManager.instance.BuildPlayerAvatar(avatarView, new Avatar(avatarStr));
        }
        if(displayNameText != null)
            displayNameText.text = displayName;
        clearedMissionText.text = clearedMission.ToString();
        if(crownCountText != null)
            crownCountText.text = crownCount.ToString();
        if (currentPlayerArrow != null)
            currentPlayerArrow.SetActive(isLocalPlayer);
        totalClearTime.SetTime(clearTime);
        
        if (rank == 0)
        {
            bg.sprite = rowBg[0];
        }
        else if (rank > 0 && rank < 5)
        {
            bg.sprite = rowBg[1];
        }
        else if (rank >= 5 && rank < 15)
        {
            bg.sprite = rowBg[2];
        }
        else
        {
            bg.sprite = rowBg[3];
        }
    }
}
