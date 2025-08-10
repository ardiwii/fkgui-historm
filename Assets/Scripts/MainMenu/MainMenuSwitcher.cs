using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> mainMenuPanels;

    public void LogoutAndSetDefault()
    {
        SetPanelShown(2);
        gameObject.SetActive(false);
    }

    public void SetPanelShown(int idx)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        for (int i = 0; i < mainMenuPanels.Count; i++)
        {
            if(mainMenuPanels[i] != null)
                mainMenuPanels[i].SetActive(false);
        }
        mainMenuPanels[idx].SetActive(true);
    }

    public void PressButtonSound()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
    }
}
