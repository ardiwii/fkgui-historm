using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSetting : MonoBehaviour
{
    public List<BackgroundSet> backgroundSets;
    public int selectedBackground;

    public delegate void BackgroundChangedDelegate(BackgroundSet newBackground);
    public event BackgroundChangedDelegate OnBackgroundChanged;
    
    public void ChangeBackground(int idx)
    {
        selectedBackground = idx;
        OnBackgroundChanged?.Invoke(backgroundSets[idx]);
    }

    public BackgroundSet GetBackground()
    {
        return backgroundSets[selectedBackground];
    }
}

[System.Serializable]
public struct BackgroundSet
{
    public Sprite background;
    public Texture backgroundMovingMotifs;
}