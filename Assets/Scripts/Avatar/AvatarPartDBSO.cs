using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarPartDB", menuName = "ScriptableObjects/Avatar", order = 1)]
public class AvatarPartDBSO : ScriptableObject
{
    public List<AvatarExpressionDBSO> body;
    public List<Sprite> face;
    public List<Sprite> shirt;
    public List<Sprite> hair;
    public List<Sprite> acc;
    public List<Color> color;
}
