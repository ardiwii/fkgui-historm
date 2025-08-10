using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public Image background;
    public RawImage movingMotifs;

    private bool eventListened;

    private void OnEnable()
    {
        if (GameSetting.instance != null)
        {
            GameSetting.instance.bgSetting.OnBackgroundChanged += OnBackgroundChanged;
            ChangeBackground(GameSetting.instance.bgSetting.GetBackground());
            eventListened = true;
        }
        else
        {
            eventListened = false;
            StartCoroutine(WaitSettingInstance());
        }

    }

    IEnumerator WaitSettingInstance()
    {
        yield return new WaitUntil(() => GameSetting.instance != null);
        GameSetting.instance.bgSetting.OnBackgroundChanged += OnBackgroundChanged;
        ChangeBackground(GameSetting.instance.bgSetting.GetBackground());
    }

    private void OnDisable()
    {
        if(eventListened)
            GameSetting.instance.bgSetting.OnBackgroundChanged -= OnBackgroundChanged;
    }

    private void OnBackgroundChanged(BackgroundSet newBackground)
    {
        ChangeBackground(newBackground);
    }

    private void ChangeBackground(BackgroundSet newBackground)
    {
        background.sprite = newBackground.background;
        movingMotifs.texture = newBackground.backgroundMovingMotifs;
    }
}
