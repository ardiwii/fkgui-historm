using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AvatarCreationControlView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skinNumber;
    [SerializeField] private TextMeshProUGUI hairNumber;
    [SerializeField] private TextMeshProUGUI hairColorNumber;
    [SerializeField] private TextMeshProUGUI shirtNumber;
    [SerializeField] private TextMeshProUGUI shirtColorNumber;
    [SerializeField] private TextMeshProUGUI accNumber;

    public void SetAll(Avatar avatar)
    {
        SetSkin(avatar.body);
        SetHair(avatar.hair);
        SetHairColor(avatar.hairColor);
        SetShirt(avatar.shirt);
        SetShirtColor(avatar.shirtColor);
        SetAccesories(avatar.acc);
    }

    public void SetSkin(int id)
    {
        skinNumber.text = (id+1).ToString();
    }
    public void SetHair(int id)
    {
        hairNumber.text = (id + 1).ToString();
    }
    public void SetHairColor(int id)
    {
        hairColorNumber.text = (id+1).ToString();
    }
    public void SetShirt(int id)
    {
        shirtNumber.text = (id + 1).ToString();
    }
    public void SetShirtColor(int id)
    {
        shirtColorNumber.text = (id + 1).ToString();
    }
    public void SetAccesories(int id)
    {
        accNumber.text = (id + 1).ToString();
    }

}
