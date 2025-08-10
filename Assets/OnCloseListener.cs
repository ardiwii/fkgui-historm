using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class OnCloseListener : MonoBehaviour
{
    /// <summary>
    /// Called when the Browser is closed.
    /// </summary>
    public void OnClose()
    {
        PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest()
        {
            EventName = "player_logout"
        },
        null, null);
    }
}
