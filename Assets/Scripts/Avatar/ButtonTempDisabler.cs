using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTempDisabler : MonoBehaviour
{
    [SerializeField] private Button buttonToDisable;
    private float delayToEnable;

    private void Start()
    {
        delayToEnable = 0;
    }

    public void TemporaryDisable(float duration)
    {
        delayToEnable = duration;
        buttonToDisable.interactable = false;
    }

    private void Update()
    {
        if(delayToEnable > 0)
        {
            delayToEnable -= Time.deltaTime;
            if(delayToEnable <= 0)
            {
                buttonToDisable.interactable = true;
            }
        }
    }
}
