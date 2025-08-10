using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvatarCreation : MonoBehaviour
{
    [SerializeField] private AvatarView view;
    [SerializeField] private AvatarCreationControlView controlView;
    [SerializeField] private ButtonGroup genderButtons;
    [SerializeField] private TMP_InputField nameInput;

    private AvatarManager manager;
    private AvatarPartDBSO avatarDB;
    private Avatar tempAvatar;
    
    private void OnEnable()
    {
        manager = AvatarManager.instance;
        tempAvatar = new Avatar(manager.playerAvatarData);
        avatarDB = AvatarManager.instance.GetAvatarPartDB();
        view.gameObject.SetActive(true);
        controlView.SetAll(tempAvatar);
        genderButtons.SetSelected(tempAvatar.gender);
        if (PlayerDataManager.instance.playerData != null)
        {
            nameInput.text = PlayerDataManager.instance.playerData.displayName;
        }
    }

    public void SetGender(int id)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        Debug.Log("setgender to :" + id);
        tempAvatar.gender = (byte)id;
        avatarDB = AvatarManager.instance.GetAvatarPartDB(id);
        tempAvatar.Reset();
        manager.BuildPlayerAvatar(view, tempAvatar);
        controlView.SetAll(tempAvatar);
        genderButtons.SetSelected(id);
    }

    public void ShiftSkinColor(bool next)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        tempAvatar.body = (byte)ShiftIndex(tempAvatar.body, next, avatarDB.body.Count - 1);
        view.SetBody(tempAvatar.body);
        controlView.SetSkin(tempAvatar.body);
    }

    public void ShiftHair(bool next)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        tempAvatar.hair = (byte)ShiftIndex(tempAvatar.hair, next, avatarDB.hair.Count - 1);
        view.SetHair(tempAvatar.hair);
        controlView.SetHair(tempAvatar.hair);
    }

    public void ShiftHairColor(bool next)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        tempAvatar.hairColor = (byte)ShiftIndex(tempAvatar.hairColor, next, avatarDB.color.Count - 1);
        view.SetHairColor(tempAvatar.hairColor);
        controlView.SetHairColor(tempAvatar.hairColor);
    }

    public void ShiftShirt(bool next)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        tempAvatar.shirt = (byte)ShiftIndex(tempAvatar.shirt, next, avatarDB.shirt.Count - 1);
        view.SetShirt(tempAvatar.shirt);
        controlView.SetShirt(tempAvatar.shirt);
    }

    public void ShiftShirtColor(bool next)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        tempAvatar.shirtColor = (byte)ShiftIndex(tempAvatar.shirtColor, next, avatarDB.color.Count - 1);
        view.SetShirtColor(tempAvatar.shirtColor);
        controlView.SetShirtColor(tempAvatar.shirtColor);
    }

    public void ShiftAcc(bool next)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        tempAvatar.acc = (byte)ShiftIndex(tempAvatar.acc, next, avatarDB.acc.Count - 1);
        view.SetAcc(tempAvatar.acc);
        controlView.SetAccesories(tempAvatar.acc);
    }

    public void SaveAvatar()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            PopupManager.instance.ShowErrorPopup("nama harus diisi");
        }
        else
        {
            SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
            manager.playerAvatarData = new Avatar(tempAvatar);
            PlayerDataManager.instance.playerData.displayName = nameInput.text;
            AvatarManager.instance.SaveAvatar();
        }
    }

    private int ShiftIndex(int startIdx, bool next, int maxIdx)
    {
        int retval = startIdx;
        if (next)
        {
            retval++;
            if (retval > maxIdx)
            {
                retval = 0;
            }
        }
        else
        {
            retval--;
            if (retval < 0)
            {
                retval = (byte)maxIdx;
            }
        }
        return retval;
    }
}
