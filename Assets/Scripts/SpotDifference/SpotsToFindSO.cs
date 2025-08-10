using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarSkinExpressionDB", menuName = "ScriptableObjects/FindTheSpotLevel", order = 3)]
public class SpotsToFindSO : ScriptableObject
{
    public Sprite realImage;
    public Sprite fakeImage;
    public List<Vector2> spotsPositions;
}
