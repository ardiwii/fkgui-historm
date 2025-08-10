using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public static GameSetting instance;

    public BackgroundSetting bgSetting;
    public SettingData setting;

    private void OnEnable()
    {
        instance = this;

        if (PlayerPrefs.HasKey("setting"))
        {
            LoadFromPlayerPref();
        }
        else
        {
            setting = new SettingData();
            SaveToPlayerPref();
        }
    }

    private void OnDisable()
    {
        instance = null;
    }

    private void LoadFromPlayerPref()
    {
        setting = JsonUtility.FromJson<SettingData>(PlayerPrefs.GetString("setting"));
        bgSetting.ChangeBackground(setting.backgroundTheme);
        SoundManager.InitializeSetting();
    }

    public void SaveToPlayerPref()
    {
        PlayerPrefs.SetString("setting", JsonUtility.ToJson(setting));
    }
}

public class SettingData
{
    public bool isSoundOn;
    public int soundVolume;
    public bool isMusicOn;
    public int musicVolume;
    public int backgroundTheme;

    public SettingData()
    {
        isSoundOn = true;
        soundVolume = 10;
        isMusicOn = true;
        musicVolume = 10;
        backgroundTheme = 0;
    }
}
