using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        SoundManager.PlayMusic(SoundManager.Asset.MainMenuMusic);
    }
}
