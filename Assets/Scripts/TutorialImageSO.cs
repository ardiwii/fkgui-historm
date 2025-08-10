using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TutorialAsset", menuName = "ScriptableObjects/Tutorial Asset", order = 7)]
public class TutorialImageSO : ScriptableObject
{
    public List<Sprite> Images = new List<Sprite>();
}
