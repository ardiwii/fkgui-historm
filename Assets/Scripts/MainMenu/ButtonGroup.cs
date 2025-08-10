using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonGroup : MonoBehaviour
{
    public List<Button> buttons;
    public List<TextMeshProUGUI> texts;

    public Color interactableTextColor;
    public Color uninteractableTextColor;

    public void SetSelected(int id)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i != id)
                buttons[i].interactable = true;
            else
                buttons[i].interactable = false;
        }
        if(texts.Count > 0 && id < texts.Count)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                if (i != id)
                    texts[i].color = interactableTextColor;
                else
                    texts[i].color = uninteractableTextColor;
            }
        }
    }
}