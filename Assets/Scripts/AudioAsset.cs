using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioAsset", menuName = "ScriptableObjects/Audio Asset", order = 6)]
public class AudioAsset : ScriptableObject
{
    public AudioClip ButtonTap, ChangeSceneClick, GetStar, Mistake, PickingPiece, DropPiece, Win,CrosswordType;

    public AudioClip StageMusic, MainMenuMusic;
}
