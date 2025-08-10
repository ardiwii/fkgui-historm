using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class JigsawManager : StageManager
{
    [SerializeField] int Hints = 2;
    [SerializeField] JigsawPiece PiecePrefab;
    [SerializeField] List<JigsawAnswer> Answers = new List<JigsawAnswer>();
    [SerializeField] Transform GameParent;
    [SerializeField] Transform PieceParent;
    [SerializeField] private PostGameQuizManager postQuizManager;
    [SerializeField] private List<GameObject> jigsawGameObjects;
    [SerializeField] GameObject QuestionHintHighlight;
    [SerializeField] List<GameObject> AnswerHintHighlight;
    
    float HighestPieceHeight = 0f;
    float hintZModifier = 0;

    protected override void Initialize()
    {
        Answers = GameParent.GetComponentsInChildren<JigsawAnswer>().ToList();
        Bounds PieceBound = PieceParent.GetComponent<Collider2D>().bounds;
        List<JigsawAnswer> AnswersCopy = new List<JigsawAnswer>(Answers);
        List<JigsawAnswer> HintAnswers = new List<JigsawAnswer>();
        for (int i = 0; i < Hints; i++)
        {
            int Rand = Random.Range(0, AnswersCopy.Count);
            HintAnswers.Add(AnswersCopy[Rand]);
            AnswersCopy.RemoveAt(Rand);
        }
        foreach (var item in Answers)
        {
            if (!HintAnswers.Contains(item))
            {
                Vector2 RandomPos = new Vector2(Random.Range(PieceBound.min.x+0.5f, PieceBound.max.x - 0.5f), Random.Range(PieceBound.min.y + 0.5f, PieceBound.max.y - 0.5f));
                item.SpawnPiece(PiecePrefab, ((Vector3)RandomPos).WithZ(-5f), Quaternion.identity, PieceParent);
            }
            else
            {
                item.BecomeHint(PiecePrefab, PieceParent);
                item.transform.position = item.transform.position.WithZ(-5f);
            }
            
        }
        HighestPieceHeight = -5f;
        //if (!PlayerPrefs.HasKey("jigsawTutorialDone"))
        //{
        //    Tutorial.Setup();
        //    PlayerPrefs.SetInt("jigsawTutorialDone", 1);
        //}
    }

    public void PieceDrop(JigsawPiece Piece,Vector2 Pos)
    {
        Bounds GameBound = GameParent.GetComponent<Collider2D>().bounds;
        Bounds PieceBound = PieceParent.GetComponent<Collider2D>().bounds;
        print(GameBound);
        print(Pos);
        print(GameBound.Contains(Pos));
        if (GameBound.Contains(Pos))
        {
            JigsawAnswer Chosen = Answers[0];
            foreach (var item in Answers)
            {
                if (Vector2.SqrMagnitude(Pos - (Vector2)item.transform.position) < Vector2.SqrMagnitude(Pos - (Vector2)Chosen.transform.position)) 
                {
                    Chosen = item;
                }
            }
            if (!Chosen.CurrentPiece)
            {
                Piece.transform.position = Chosen.transform.position.WithZ(Piece.transform.position.z);
                Piece.LastPosition = Piece.transform.position;
                Piece.CurrentAnswer = Chosen;
                Chosen.CurrentPiece = Piece;
                if (isHintActive)
                {
                    DisableHint();
                    HideHint();
                }
                SoundManager.PlaySound(SoundManager.Asset.DropPiece);
                //CheckSolved();
            }
            else
            {
                Piece.transform.position = Piece.LastPosition;
            }
            
        }
        else if(PieceBound.Contains(Pos))
        {
            Piece.transform.position = Pos.WithZ(Piece.transform.position.z);
            Piece.LastPosition = Piece.transform.position;
        }
        else
        {
            Piece.transform.position = Piece.LastPosition;
        }
    }

    public void PiecePick(JigsawPiece Piece)
    {
        HighestPieceHeight -= 0.00001f;
        Piece.transform.position = Piece.transform.position.WithZ(HighestPieceHeight);
        SoundManager.PlaySound(SoundManager.Asset.PickingPiece);
    }

    void CheckSolved()
    {
        List<JigsawAnswer> SolvedAnswer = Answers.FindAll(x => x.Solved);
        print(SolvedAnswer.Count);
        if (SolvedAnswer.Count == Answers.Count)
        {
            JigsawFinished();
        }
        else
        {
            MakeMistake();
        }
    }

    public void SelesaiPressed()
    {
        CheckSolved();
    }

    void JigsawFinished()
    {
        DOVirtual.DelayedCall(1f,delegate 
        {
            for (int i = 0; i < jigsawGameObjects.Count; i++)
            {
                jigsawGameObjects[i].SetActive(false);
            }
            postQuizManager.gameObject.SetActive(true);
            postQuizManager.InitializeQuiz();
        });
        
        
    }


    public void DebugFinished()
    {
        JigsawFinished();
    }
    
    public void FinishPressed()
    {
        Finished();
    }

    public override void EnableCheat()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DebugFinished();
        }
    }
    
    public override void ActivateHint()
    {
        base.ActivateHint();
        List<JigsawAnswer> unsolvedSpots = new List<JigsawAnswer>();

        for (int i = 0; i < Answers.Count; i++)
        {
            if (!Answers[i].Solved) unsolvedSpots.Add(Answers[i]);
        }

        if (unsolvedSpots.Count == 0)
        {
            Debug.Log("all spots are solved, hint unused");
            return;
        }

        JigsawAnswer hintedPicture = unsolvedSpots[Random.Range(0, unsolvedSpots.Count)];

        Vector2 correctPosition = new Vector2Int();
        List<JigsawAnswer> candidate = new List<JigsawAnswer>(Answers);

        for (int i = candidate.Count - 1; i >= 0; i--)
        {
            if (candidate[i] == hintedPicture)
            {
                correctPosition = candidate[i].gameObject.transform.position;
                candidate.RemoveAt(i);
            }
        }
        AnswerHintHighlight[0].transform.position = new Vector3(correctPosition.x, correctPosition.y, -1);
        AnswerHintHighlight[0].gameObject.SetActive(true);

        if (candidate.Count > 0)
        {
            int otherPositionIdx1 = Random.Range(0, candidate.Count);
            Vector2 otherPosition1 = candidate[otherPositionIdx1].gameObject.transform.position;
            AnswerHintHighlight[1].transform.position = new Vector3(otherPosition1.x, otherPosition1.y, -1);
            AnswerHintHighlight[1].gameObject.SetActive(true);
            candidate.RemoveAt(otherPositionIdx1);
        }
        if (candidate.Count > 0)
        {
            int otherPositionIdx2 = Random.Range(0, candidate.Count);
            Vector2 otherPosition2 = candidate[otherPositionIdx2].gameObject.transform.position;
            AnswerHintHighlight[2].transform.position = new Vector3(otherPosition2.x, otherPosition2.y, -1);
            AnswerHintHighlight[2].gameObject.SetActive(true);
            candidate.RemoveAt(otherPositionIdx2);
        }

        hintZModifier -= 0.01f;
        Vector3 currPosition = hintedPicture.CorrectPiece.GetComponent<JigsawPiece>().LastPosition;
        Vector3 newPos = new Vector3(currPosition.x, currPosition.y, currPosition.z + hintZModifier);
        hintedPicture.CorrectPiece.GetComponent<JigsawPiece>().LastPosition = newPos;
        hintedPicture.CorrectPiece.transform.position = newPos;
        QuestionHintHighlight.transform.position = new Vector3(hintedPicture.CorrectPiece.transform.position.x, hintedPicture.CorrectPiece.transform.position.y, hintedPicture.CorrectPiece.transform.position.z - 0.01f);
        QuestionHintHighlight.transform.parent = hintedPicture.CorrectPiece.transform;
        QuestionHintHighlight.SetActive(true);
    }

    private void HideHint()
    {
        for (int i = 0; i < AnswerHintHighlight.Count; i++)
        {
            AnswerHintHighlight[i].gameObject.SetActive(false);
        }
        QuestionHintHighlight.transform.parent = PieceParent.transform;
        QuestionHintHighlight.SetActive(false);
    }
}
