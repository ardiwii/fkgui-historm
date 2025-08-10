using UnityEngine;
using UnityEngine.UI;

public class JigsawAnswer : MonoBehaviour
{

    public Vector2Int Position;
    public SpriteRenderer img;

    public GameObject CorrectPiece;
    public JigsawPiece CurrentPiece;
    public bool Solved
    {
        get
        {
            if (!CurrentPiece)
            {
                return false;
            }
            return CurrentPiece.gameObject == CorrectPiece;
        }
    }
    

    JigsawManager Manager => (JigsawManager)StageManager.Instance;


    private void Awake()
    {
        img.color = img.color.WithAlpha(0f);   
    }



    public JigsawPiece SpawnPiece(JigsawPiece PiecePrefab,Vector3 Pos, Quaternion Rot,Transform Parent)
    {
        JigsawPiece newpiece = Instantiate(PiecePrefab,Pos,Rot,Parent);
        newpiece.SetImage(img.sprite);
        CorrectPiece = newpiece.gameObject;
        return newpiece;
    }
    
    public JigsawPiece BecomeHint(JigsawPiece PiecePrefab, Transform Parent)
    {
        JigsawPiece newpiece = SpawnPiece(PiecePrefab,transform.position,transform.rotation,Parent);
        newpiece.CurrentAnswer = this;
        CurrentPiece = newpiece;
        newpiece.IsHint = true;
        return newpiece;
    }
}
