using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarSkinExpressionDB", menuName = "ScriptableObjects/AvatarSkinSet", order = 2)]
public class AvatarExpressionDBSO : ScriptableObject
{
    public Sprite body;
    public List<AvatarExpressionSet> expressionSets; //index correspond to the (int)expression
}

[System.Serializable]
public class AvatarExpressionSet
{
    public Sprite face;
    public Sprite coat;
    public Sprite hand;
}

public enum Expression
{
    idle,
    happy,
    sad,
    thinking
}