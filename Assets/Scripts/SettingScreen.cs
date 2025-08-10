using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingScreen : MonoBehaviour
{
    [SerializeField] private ButtonGroup soundToggleButton;
    [SerializeField] private TextMeshProUGUI soundVolumeText;
    [SerializeField] private GameObject soundUpButton;
    [SerializeField] private GameObject soundDownButton;
    [SerializeField] private ButtonGroup musicToggleButton;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private GameObject musicUpButton;
    [SerializeField] private GameObject musicDownButton;
    [SerializeField] private TextMeshProUGUI themeText;
    [SerializeField] private GameObject themeUpButton;
    [SerializeField] private GameObject themeDownButton;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject creditPanel;

    private SettingData currSettingData;

    private void OnEnable()
    {
        currSettingData = GameSetting.instance.setting;
        SetSound(currSettingData.isSoundOn);
        SetSoundVolume(currSettingData.soundVolume);
        SetMusic(currSettingData.isMusicOn);
        SetMusicVolume(currSettingData.musicVolume);
        SetTheme(currSettingData.backgroundTheme);
    }

    private void OnDisable()
    {
        GameSetting.instance.SaveToPlayerPref();
    }

    public void SetSound(bool isOn)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        currSettingData.isSoundOn = isOn;
        soundToggleButton.SetSelected(currSettingData.isSoundOn ? 0 : 1);
        SoundManager.OnAudioSettingChanged();
    }

    public void ShiftSoundVolume(bool isUp)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        if (isUp)
        {
            SetSoundVolume(currSettingData.soundVolume +1);
        }
        else
        {
            SetSoundVolume(currSettingData.soundVolume - 1);
        }
    }

    private void SetSoundVolume(int newVolume)
    {
        currSettingData.soundVolume = newVolume;
        if (currSettingData.soundVolume == 10)
        {
            soundUpButton.SetActive(false);
        }
        else
        {
            if (!soundUpButton.gameObject.activeInHierarchy) soundUpButton.SetActive(true);
        }
        if (currSettingData.soundVolume == 0)
        {
            soundDownButton.SetActive(false);
        }
        else
        {
            if (!soundDownButton.gameObject.activeInHierarchy) soundDownButton.SetActive(true);
        }

        soundVolumeText.text = currSettingData.soundVolume.ToString();
    }
    
    public void SetMusic(bool isOn)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        if (isOn && !currSettingData.isMusicOn) SoundManager.StartMusic();
        else if (!isOn && currSettingData.isMusicOn) SoundManager.StopMusic();
        currSettingData.isMusicOn = isOn;
        musicToggleButton.SetSelected(currSettingData.isMusicOn ? 0 : 1);
    }

    public void ShiftMusicVolume(bool isUp)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        if (isUp)
        {
            SetMusicVolume(currSettingData.musicVolume +1);
        }
        else
        {
            SetMusicVolume(currSettingData.musicVolume - 1);
        }
    }

    private void SetMusicVolume(int newVolume)
    {
        currSettingData.musicVolume = newVolume;
        if (currSettingData.musicVolume == 10)
        {
            musicUpButton.SetActive(false);
        }
        else
        {
            if (!musicUpButton.gameObject.activeInHierarchy) musicUpButton.SetActive(true);
        }
        if (currSettingData.musicVolume == 0)
        {
            musicDownButton.SetActive(false);
        }
        else
        {
            if (!musicDownButton.gameObject.activeInHierarchy) musicDownButton.SetActive(true);
        }
        musicVolumeText.text = currSettingData.musicVolume.ToString();
        SoundManager.OnAudioSettingChanged();
    }

    public void ShiftTheme(bool isUp)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        if (isUp)
        {
            if (currSettingData.backgroundTheme == 17)
                SetTheme(0);
            else
                SetTheme(currSettingData.backgroundTheme + 1);
        }
        else
        {
            if (currSettingData.backgroundTheme == 0)
                SetTheme(17);
            else
                SetTheme(currSettingData.backgroundTheme - 1);
        }
    }

    private void SetTheme(int theme)
    {
        currSettingData.backgroundTheme = theme;
        themeText.text = (currSettingData.backgroundTheme+1).ToString();
        GameSetting.instance.bgSetting.ChangeBackground(theme);
    }

    public void SwitchToCredit()
    {
        creditPanel.SetActive(true);
        settingPanel.SetActive(false);
    }

    public void SwitchToSetting()
    {
        creditPanel.SetActive(false);
        settingPanel.SetActive(true);
    }
}
