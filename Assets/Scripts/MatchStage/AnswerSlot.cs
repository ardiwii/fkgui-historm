using TMPro;
using UnityEngine;

public class AnswerSlot : MonoBehaviour
{

    
    
    public Vector2Int Position
    {
        get
        {
            return new Vector2Int((int)(transform.localPosition.x/MatchStageManager.HorizontalRange), (int)(transform.localPosition.y / MatchStageManager.VerticalRange));
        }
    }

    [HideInInspector] public GameObject Target;
    [HideInInspector] public TextMeshPro Tex;

    private void Awake()
    {
        Tex = GetComponentInChildren<TextMeshPro>();
    }

    public void Setup(QuestionPicture pic)
    {
        Target = pic.gameObject;
        Tex.text = pic.name;
    }



    
}
