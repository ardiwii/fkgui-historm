using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MultiChoiceHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MultiChoiceOption option;
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        option.SetSelected();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        option.SetUnselected();
    }

}
