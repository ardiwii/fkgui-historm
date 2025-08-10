using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    protected MainMenuState menuState;

    public void OnEnable()
    {
        OnDisplayed();
    }

    public virtual void OnDisplayed()
    {
        menuState = MainMenuState.instance;
    }

}
