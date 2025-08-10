using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    [SerializeField] private GameObject smallNotif;
    [SerializeField] private TextMeshProUGUI smallNotifMessage;
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private TextMeshProUGUI statusMessage;
    [SerializeField] private GameObject failedPanel;
    [SerializeField] private TextMeshProUGUI failedMessage;
    [SerializeField] private TextMeshProUGUI failedButtonText;
    private Action popupButtonAction;
    private float smallNotifTimer;

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    private void Update()
    {
        if(smallNotifTimer > 0)
        {
            smallNotifTimer -= Time.deltaTime;
            if (smallNotifTimer <= 0)
                smallNotif.SetActive(false);
        }
    }

    public void ShowSmallNotif(string message, float duration)
    {
        smallNotif.SetActive(true);
        smallNotifMessage.text = message;
        smallNotifTimer = duration;
    }

    public void ShowStatusPopup(string message)
    {
        statusPanel.SetActive(true);
        statusMessage.text = message;
    }

    public void CloseStatusPopup()
    {
        statusPanel.SetActive(false);
    }

    public void ShowErrorPopup(string message, string buttonText = "", Action onButtonPress = null)
    {
        failedPanel.SetActive(true);
        failedMessage.text = message;
        if (string.IsNullOrEmpty(buttonText))
        {
            failedButtonText.text = "Close";
        }
        else
        {
            failedButtonText.text = buttonText;
        }
        if(onButtonPress != null)
        {
            popupButtonAction = onButtonPress;
        }
        else
        {
            popupButtonAction = CloseErrorPopup;
        }
    }

    public void ErrorButton()
    {
        popupButtonAction.Invoke();
        failedPanel.SetActive(false);
    }

    public void CloseErrorPopup()
    {
        failedPanel.SetActive(false);
    }
}
