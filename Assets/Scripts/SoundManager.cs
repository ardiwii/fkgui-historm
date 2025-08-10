using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Audio;
using UnityEditor;

public static class SoundManager
{

    static List<AudioSource> AudioPool = new List<AudioSource>();
    static AudioSource MusicSource;

    const string AssetPath = "AudioAsset";
    static AudioAsset asset;

    public static AudioAsset Asset
    {
        get
        {
            if (!asset)
            {
                asset = Resources.Load<AudioAsset>(AssetPath);
                if (!asset) Debug.LogError("Asset not exists");
                return asset;
            }
            else
            {
                return asset;
            }
        }
    }
    
    static void Clean()
    {
        AudioPool.RemoveAll(x => x == null);
    }

    static AudioSource Create()
    {
        AudioSource aud = new GameObject("TempAudio").AddComponent<AudioSource>();
        AudioPool.Add(aud);
        return aud;
    }

    static AudioSource GetAudio()
    {
        Clean();
        AudioSource aud = AudioPool.Find(x => x!=null && !x.gameObject.activeSelf);
        
        if (aud)
        {
            aud.gameObject.SetActive(true);
            return aud;
        }
        else
        {
            return Create();
        }
        
    }

    public static void PlaySound(AudioClip Clip)
    {
        AudioSource aud = GetAudio();
        aud.volume = 1f;
        if (GameSetting.instance)
        {
            aud.volume *= GameSetting.instance.setting.soundVolume * 0.1f;
            aud.volume = GameSetting.instance.setting.isSoundOn ? aud.volume : 0f;
        }
        aud.PlayOneShot(Clip);
        DOVirtual.DelayedCall(Clip.length, () => aud.gameObject.SetActive(false));
    }

    public static void PlayMusic(AudioClip Clip)
    {
        StopMusic();
        if (!MusicSource)
        {
            AudioSource aud = new GameObject("Music").AddComponent<AudioSource>();
            aud.loop = true;
            MusicSource = aud;

        }
        MusicSource.clip = Clip;
        if (GameSetting.instance)
        {
            if (!GameSetting.instance.setting.isMusicOn) return;
            MusicSource.volume = 1f;
            MusicSource.volume *= GameSetting.instance.setting.musicVolume * 0.1f;
        }
        
        MusicSource.Play();
    }

    public static void StopMusic()
    {
        if (MusicSource)
        {
            MusicSource.Stop();
        }
    }

    public static void StartMusic()
    {
        if (MusicSource)
        {
            MusicSource.Play();
        }
    }

    public static void SetPressedSound(this Button but,AudioClip Clip)
    {
        but.onClick.AddListener(delegate { PlaySound(Clip); });
    }

    public static void OnAudioSettingChanged()
    {
        MusicSource.volume = GameSetting.instance.setting.musicVolume * 0.1f;
        foreach (var item in AudioPool)
        {
            item.volume =  GameSetting.instance.setting.soundVolume * 0.1f;
        }
    }

    public static void InitializeSetting()
    {
        MusicSource.volume = GameSetting.instance.setting.musicVolume * 0.1f;
        foreach (var item in AudioPool)
        {
            item.volume = GameSetting.instance.setting.soundVolume * 0.1f;
            item.volume = GameSetting.instance.setting.isSoundOn ? item.volume : 0f;
        }
        if (!GameSetting.instance.setting.isMusicOn)
        {
            StopMusic();
        }
    }
}
