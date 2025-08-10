using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyContentSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> contents;
    private GameObject currentOpenedContent;
    
    public void SwitchContent(int idx)
    {
        if(currentOpenedContent != null)
        {
            currentOpenedContent.SetActive(false);
        }
        contents[idx].SetActive(true);
        currentOpenedContent = contents[idx];
    }
}
