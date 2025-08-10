using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MultiChoiceOption : MonoBehaviour
{
    [SerializeField] private Image button;
    [SerializeField] private TextMeshProUGUI optionText;
    [SerializeField] private TextMeshProUGUI choiceLetter;
    [SerializeField] private Sprite unselectedButton;
    [SerializeField] private Sprite selectedButton;

    public void SetOption(string option)
    {
        optionText.text = option;
    }

    public void SetSelected()
    {
        button.sprite = selectedButton;
        choiceLetter.color = Color.black;
    }

    public void SetUnselected()
    {
        button.sprite = unselectedButton;
        choiceLetter.color = Color.white;
    }
}
