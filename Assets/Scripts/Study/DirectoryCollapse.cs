using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DirectoryCollapse : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI icon;
    [SerializeField] private GameObject expansion;
    [SerializeField] private RectTransform layoutGroup;
    private bool isExpanded;

    public void ToggleExpand()
    {
        if (!isExpanded)
        {
            expansion.SetActive(true);
            icon.text = "-";
            isExpanded = true;
        }
        else
        {
            expansion.SetActive(false);
            icon.text = "+";
            isExpanded = false;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
    }
}
