using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosswordQuestionTab : MonoBehaviour
{
    [SerializeField] Image MendatarImage;
    [SerializeField] Image MenurunImage;

    [SerializeField] GameObject MendatarObj;
    [SerializeField] GameObject MenurunObj;
    
    public void MendatarClicked()
    {
        MendatarImage.color = MendatarImage.color.WithAlpha(1f);
        MenurunImage.color = MenurunImage.color.WithAlpha(0f);
        MendatarObj.SetActive(true);
        MenurunObj.SetActive(false);
    }

    public void MenurunClicked()
    {
        MenurunImage.color = MenurunImage.color.WithAlpha(1f);
        MendatarImage.color = MendatarImage.color.WithAlpha(0f);
        MendatarObj.SetActive(false);
        MenurunObj.SetActive(true);
    }
}
