using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;

public class PlayfabLogin : MonoBehaviour
{
    [SerializeField] private TMP_InputField idInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private GameObject changeIdButton;
    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject avatarCreationScreen;
    [SerializeField] private GameObject mainMenuScreen;
    private LoginState state;
    private string inputId;
    private string inputPass;

    // Start is called before the first frame update
    void Start()
    {
        state = LoginState.Offline;
        SetIdInputState();
        if (EncryptedPlayerPrefs.HasKey("signedInId"))
        {
            inputId = EncryptedPlayerPrefs.GetString("signedInId");
            inputPass = EncryptedPlayerPrefs.GetString("signedInPass");
            Login();
        }
    }

    public void LoginButton()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        inputId = idInput.text;
        inputPass = passwordInput.text;
        Login();
    }

    public void Login()
    {
        if (state == LoginState.Offline)
        {
            var req = new LoginWithCustomIDRequest() { CustomId = inputId };
            PlayFabClientAPI.LoginWithCustomID(req, OnLoginSuccess, OnFailed);
            PopupManager.instance.ShowStatusPopup("logging in");
        }
        else if(state == LoginState.NeedPassword)
        {
            CheckPassword();
        }
    }

    public void Logout()
    {
        PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest()
        {
            EventName = "player_logout",
            
        },
        null, null);
        PlayFabClientAPI.ForgetAllCredentials();
        EncryptedPlayerPrefs.DeleteKey("signedInId");
        EncryptedPlayerPrefs.DeleteKey("signedInPass");
        gameObject.SetActive(true);
        state = LoginState.Offline;
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        state = LoginState.NeedPassword;
        CheckPassword();
    }

    private void CheckPassword()
    {
        var req = new ExecuteCloudScriptRequest() { FunctionName = "checkPassword", FunctionParameter = new { password = inputPass } };
        PlayFabClientAPI.ExecuteCloudScript(req, OnPasswordCorrect, OnFailed);
        PopupManager.instance.ShowStatusPopup("checking password");
    }

    private void OnPasswordCorrect(ExecuteCloudScriptResult obj)
    {
        Debug.Log("result: " + obj.FunctionResult.ToString());
        SuccessResult json = JsonUtility.FromJson<SuccessResult>(obj.FunctionResult.ToString());
        if (json.success)
        {
            PopupManager.instance.CloseStatusPopup();
            state = LoginState.LoggedIn;
            PlayerDataManager.instance.LoadPlayerData(AfterLoad);

            EncryptedPlayerPrefs.SetString("signedInId", inputId);
            EncryptedPlayerPrefs.SetString("signedInPass", inputPass);
        }
        else
        {
            PopupManager.instance.CloseStatusPopup();
            PopupManager.instance.ShowErrorPopup("incorrect password");
            SetIdInputState();
        }
    }

    private void AfterLoad()
    {
        loginScreen.SetActive(false);
        if(AvatarManager.instance.isAvatarLoaded)
            mainMenuScreen.SetActive(true);
        else
            avatarCreationScreen.SetActive(true);
    }

    private void OnFailed(PlayFabError obj)
    {
        PopupManager.instance.CloseStatusPopup();
        PopupManager.instance.ShowErrorPopup(obj.ErrorMessage);
        SetIdInputState();
    }

    public void SetIdInputState()
    {
        if(state == LoginState.NeedPassword)
        {
            idInput.text = inputId;
            idInput.interactable = false;
            changeIdButton.SetActive(true);
        }
        else if(state == LoginState.Offline)
        {
            idInput.text = "";
            idInput.interactable = true;
            changeIdButton.SetActive(false);
        }
    }
}

public enum LoginState
{
    Offline,
    NeedPassword,
    LoggedIn
}

[System.Serializable]
public class SuccessResult
{
    public bool success;
}
