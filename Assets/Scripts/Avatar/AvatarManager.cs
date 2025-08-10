using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager instance;

    public AvatarPartDBSO femaleAvatarPartDB;
    public AvatarPartDBSO maleAvatarPartDB;
    public Avatar playerAvatarData;
    public bool isAvatarLoaded = false;

    private int ongoingProcessCount;
    private int savingProcessSuccessCount;

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    public AvatarPartDBSO GetAvatarPartDB()
    {
        if (playerAvatarData.gender == 0)
            return femaleAvatarPartDB;
        else
            return maleAvatarPartDB;
    }

    public AvatarPartDBSO GetAvatarPartDB(int tempGender)
    {
        if (tempGender == 0)
            return femaleAvatarPartDB;
        else
            return maleAvatarPartDB;
    }

    public void SaveAvatar()
    {
        ongoingProcessCount = 0;
        savingProcessSuccessCount = 0;
        SaveAvatarData();
        SaveDisplayName();
        PopupManager.instance.ShowSmallNotif("saving data", 3f);
    }

    private void SaveAvatarData()
    {
        ongoingProcessCount++;
        var req = new UpdateAvatarUrlRequest() { ImageUrl = playerAvatarData.ToString() };
        PlayFabClientAPI.UpdateAvatarUrl(req, OnSaveAvatarSuccess, OnSaveFailed);
    }

    private void SaveDisplayName()
    {
        ongoingProcessCount++;
        var req = new UpdateUserTitleDisplayNameRequest() { DisplayName = PlayerDataManager.instance.playerData.displayName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(req, OnUpdateDisplayNameSuccess, OnSaveFailed);
    }

    private void OnInitializeStatSuccess(UpdatePlayerStatisticsResult obj)
    {
        CheckProcessSuccess();
    }

    private void OnSaveAvatarSuccess(EmptyResponse obj)
    {
        CheckProcessSuccess();
    }

    private void OnUpdateDisplayNameSuccess(UpdateUserTitleDisplayNameResult obj)
    {
        CheckProcessSuccess();
    }

    private void CheckProcessSuccess()
    {
        savingProcessSuccessCount++;
        if(savingProcessSuccessCount >= ongoingProcessCount)
        {
            PopupManager.instance.ShowSmallNotif("save success", 3f);
        }
    }

    private void OnSaveFailed(PlayFabError obj)
    {
        Debug.LogError(obj.ErrorMessage);
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowErrorPopup(obj.ErrorMessage, "close");
    }

    public void BuildPlayerAvatar(AvatarView view)
    {
        view.SetGender(playerAvatarData.gender);
        view.SetBody(playerAvatarData.body);
        view.SetShirt(playerAvatarData.shirt);
        view.SetShirtColor(playerAvatarData.shirtColor);
        view.SetHair(playerAvatarData.hair);
        view.SetHairColor(playerAvatarData.hairColor);
        view.SetAcc(playerAvatarData.acc);
    }

    public void BuildPlayerAvatar(AvatarView view, Avatar avatarData)
    {
        view.SetGender(avatarData.gender);
        view.SetBody(avatarData.body);
        view.SetShirt(avatarData.shirt);
        view.SetShirtColor(avatarData.shirtColor);
        view.SetHair(avatarData.hair);
        view.SetHairColor(avatarData.hairColor);
        view.SetAcc(avatarData.acc);
    }
}
