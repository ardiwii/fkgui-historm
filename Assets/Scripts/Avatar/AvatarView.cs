using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarView : MonoBehaviour
{
    [SerializeField] private bool dontSetOnEnable;

    [SerializeField] private Image body;
    [SerializeField] private Image face;
    [SerializeField] private Image shirt;
    [SerializeField] private Image coat;
    [SerializeField] private Image hair;
    [SerializeField] private Image acc;
    [SerializeField] private Image hand;

    private AvatarPartDBSO avatarPartDb;

    private float briefExpressionTimeRemaining;
    private Expression expressionToReturn;

    private void OnEnable()
    {
        avatarPartDb = AvatarManager.instance.GetAvatarPartDB();
        if(!dontSetOnEnable)
            AvatarManager.instance.BuildPlayerAvatar(this);
    }

    public void SetGender(int id)
    {
        avatarPartDb = AvatarManager.instance.GetAvatarPartDB(id);
    }

    public void SetBody(int id)
    {
        body.sprite = avatarPartDb.body[id].body;
        face.sprite = avatarPartDb.body[id].expressionSets[0].face;
        coat.sprite = avatarPartDb.body[id].expressionSets[0].coat;
    }

    public void SetShirt(int id)
    {
        shirt.sprite = avatarPartDb.shirt[id];
    }

    public void SetShirtColor(int id)
    {
        shirt.color = avatarPartDb.color[id];
    }

    public void SetHair(int id)
    {
        hair.sprite = avatarPartDb.hair[id];
    }

    public void SetHairColor(int id)
    {
        hair.color = avatarPartDb.color[id];
    }

    public void SetAcc(int id)
    {
        acc.sprite = avatarPartDb.acc[id];
        acc.gameObject.SetActive(acc.sprite != null);
    }

    public void SetAccColor(int id)
    {
        acc.color = avatarPartDb.color[id];
    }

    public void SetExpression(Expression exp)
    {
        if (AvatarManager.instance == null) return;
        int skinId = AvatarManager.instance.playerAvatarData.body;
        int expIdx = (int)exp;
        body.sprite = avatarPartDb.body[skinId].body;
        face.sprite = avatarPartDb.body[skinId].expressionSets[expIdx].face;
        coat.sprite = avatarPartDb.body[skinId].expressionSets[expIdx].coat;
        if(avatarPartDb.body[skinId].expressionSets[expIdx].hand == null)
        {
            hand.gameObject.SetActive(false);
        }
        else
        {
            hand.gameObject.SetActive(true);
            hand.sprite = avatarPartDb.body[skinId].expressionSets[expIdx].hand;
        }
        expressionToReturn = exp; //set expression to return to this "permanent expression" so that it won't change after
    }

    public void SetTimedExpression(Expression exp, float time, Expression nextExp)
    {
        SetExpression(exp);
        briefExpressionTimeRemaining = time;
        expressionToReturn = nextExp;
    }

    private void Update()
    {
        if(briefExpressionTimeRemaining > 0)
        {
            briefExpressionTimeRemaining -= Time.deltaTime;
            if(briefExpressionTimeRemaining <= 0)
            {
                SetExpression(expressionToReturn);
            }
        }
    }
}
