using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStageManager : StageManager
{
    [SerializeField] Vector2Int Size;
    [SerializeField] Vector2Int EmptyPos;
    [SerializeField] Transform GameParent;
    [SerializeField] float HighestPieceHeight = -2f;
    [SerializeField] List<Sprite> Questions;
    [SerializeField] List<string> hintText;
    [SerializeField] List<QuestionPicture> QuestionSlot;
    [SerializeField] List<AnswerSlot> Answers;
    [SerializeField] Zoomer ZoomObject;
    [SerializeField] GameObject SelectionObj;
    [SerializeField] GameObject QuestionHintHighlight;
    [SerializeField] List<GameObject> AnswerHintHighlight;

    int Solved => QuestionSlot.FindAll(x => x.Solved).Count;
    public bool IsMoving { get; private set; }
    const float MovingTime = 0.25f;

    public const float HorizontalRange = 1.5f;
    public const float VerticalRange = 1.12f;

    protected override void Initialize()
    {
        List<int> UsedId = new List<int>();
        List<AnswerSlot> ActiveAnswerSlot = new List<AnswerSlot>(Answers);
        for (int i = 0; i < QuestionSlot.Count; i++)
        {
            int ChosenId = Random.Range(0, Questions.Count);
            while (UsedId.Contains(ChosenId))
            {
                ChosenId = Random.Range(0, Questions.Count);
            }
            QuestionSlot[i].Setup(Questions[ChosenId], hintText[ChosenId]);
            RandomAnswer(QuestionSlot[i], ActiveAnswerSlot);
            UsedId.Add(ChosenId);
        }
        foreach (var item in QuestionSlot)
        {
            CheckSolve(item);
        }
        EmptyPos = new Vector2Int(2, -2);
        HighestPieceHeight = QuestionSlot[1].transform.position.z;
        if (!PlayerPrefs.HasKey("matchTutorialDone"))
        {
            Tutorial.Setup();
            PlayerPrefs.SetInt("matchTutorialDone", 1);
        }
    }

    void RandomAnswer(QuestionPicture ChosenPic,List<AnswerSlot> AnswerList)
    {
        int ChosenId = Random.Range(0, AnswerList.Count);
        AnswerList[ChosenId].Setup(ChosenPic);
        AnswerList.RemoveAt(ChosenId);
    }

    void CheckSolve(QuestionPicture pic)
    {
        AnswerSlot MatchAnswer = FindAnswerAt(pic.Position);
        if (MatchAnswer && MatchAnswer.Target == pic.gameObject)
        {
            pic.Solved = true;
            print(pic.name + " was solved");
        }
        else if(pic.Solved)
        {
            pic.Solved = false;
            print(pic.name + " was unsolved");
        }
    }

    public void FinishCheck()
    {
        int SolvedCount = QuestionSlot.FindAll(x => x.Solved).Count;
        if (SolvedCount >= QuestionSlot.Count)
        {
            Finished();
        }
        else
        {
            MakeMistake();            
        }
    }

    public void SelectQuestion(QuestionPicture pic)
    {
        SelectionObj.transform.position = pic.transform.position;
        SelectionObj.SetActive(true);
    }

    public void UnSelect()
    {
        SelectionObj.SetActive(false);
    }

    public void MoveToEmpty(QuestionPicture pic)
    {
        MoveQuestion(pic,EmptyPos);
    }

    public void MoveQuestion(QuestionPicture pic,Vector2 Direction)
    {
        if (IsMoving) return;
        Vector2Int Destination = pic.Position + Vector2Int.RoundToInt(Direction);
        //print(pic.Position+" "+Destination);
        //print(pic.transform.localPosition.y / VerticalRange);
        
        if (CheckMovable(pic, Destination))
        {
            StartCoroutine(Moving(pic, Direction));
        }
    }

    public void MoveQuestion(QuestionPicture pic, Vector2Int Destination)
    {
        if (IsMoving) return;
        if (CheckMovable(pic, Destination))
        {
            UnSelect();
            Vector2 RealDestination = new Vector2(Destination.x * HorizontalRange, Destination.y * VerticalRange);
            EmptyPos = pic.Position;
            StartCoroutine(Moving(pic, RealDestination));
            if (isHintActive)
            {
                DisableHint();
                HideHint();
            }
        }
    }

    

    IEnumerator Moving(QuestionPicture pic, Vector2 Destination)
    {
        IsMoving = true;
        Vector3 StartPos = pic.transform.localPosition;
        Vector2 Direction = (Destination - (Vector2)pic.transform.localPosition).normalized;
        float Speed = Vector2.Distance(pic.transform.localPosition, Destination)/MovingTime;
        
        for (float i = 0; i <= MovingTime; i += Time.deltaTime)
        {
            pic.transform.localPosition += (Vector3)Direction * Speed * Time.deltaTime;

            yield return null;
        }
        pic.transform.localPosition = Destination.WithZ(pic.transform.localPosition.z);
        IsMoving = false;
        CheckSolve(pic);
        pic.LastPosition = pic.transform.localPosition;
        yield break;
    }

    bool CheckMovable(QuestionPicture Pic,Vector2Int Destination)
    {
        Vector2Int Delta = Destination - Pic.Position;
        if (CurrentDifficulty == Difficulty.hard && (Mathf.Abs(Delta.x) + Mathf.Abs(Delta.y) != 1)) return false;
        if(Destination.x>=Size.x || Destination.x<0 || Destination.y <= Size.y || Destination.y > 0)
        {
            print("Bound");
            return false;
        }
        else if(FindQuestionAt(Destination))
        {
            QuestionPicture DebugObj = FindQuestionAt(Destination);
            Debug.Log(DebugObj,DebugObj.gameObject);
            print("Occupied");
            return false;
        }
        else
        {
            return true;
        }

    }

    public void PiecePick(QuestionPicture pic)
    {
        HighestPieceHeight -= 0.00001f;
        pic.transform.position = pic.transform.position.WithZ(HighestPieceHeight);
        SelectQuestion(pic);
        SoundManager.PlaySound(SoundManager.Asset.PickingPiece);
    }

    public void PieceDrop(QuestionPicture pic,Vector2 Pos)
    {
        Bounds GameBound = GameParent.GetComponent<Collider2D>().bounds;
        print(GameBound.Contains(Pos.WithZ(10f)));
        print(Pos + " " + GameBound);
        if (GameBound.Contains(Pos.WithZ(10f)))
        {
            
            QuestionPicture Chosen = null;
            
            foreach (var item in QuestionSlot)
            {
                if (item==pic)
                {
                    continue;
                }
                if (Chosen == null)
                {
                    Chosen = item;
                    continue;
                }
                if ((Vector2.SqrMagnitude(Pos - (Vector2)item.transform.position) < Vector2.SqrMagnitude(Pos - (Vector2)Chosen.transform.position)) )
                {
                    Chosen = item;
                }
            }
            
            pic.transform.position = Chosen.transform.position.WithZ(pic.transform.position.z);
            print(pic.LastPosition);
            MoveQuestion(Chosen, pic.RealLastPosition);
            pic.LastPosition = pic.transform.localPosition;
            CheckSolve(pic);
            SoundManager.PlaySound(SoundManager.Asset.DropPiece);

        }
        else
        {
            pic.transform.localPosition = pic.LastPosition;
        }
        UnSelect();
    }

    public void PieceDrag(QuestionPicture pic)
    {
        SelectionObj.transform.position = pic.transform.position;
    }

    public void EnterZoom(QuestionPicture Pic)
    {
        Debug.Log(Pic.gameObject.name + ": " + Pic.hintText);
        ZoomObject.Setup(Pic, time > 180);
    }

    QuestionPicture FindQuestionAt(Vector2Int Target)
    {
        return QuestionSlot.Find(x => x.Position == Target);
    }

    AnswerSlot FindAnswerAt(Vector2Int Target)
    {
        return Answers.Find(x => x.Position == Target);
    }

    void ActivateCheat()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var item in Answers)
            {
                item.Target.GetComponent<QuestionPicture>().ActivateCheat(item.Position.ToString());
            }
        }
    }

    public override void ActivateHint()
    {
        base.ActivateHint();
        List<QuestionPicture> unsolvedSpots = new List<QuestionPicture>();

        for (int i = 0; i < QuestionSlot.Count; i++)
        {
            if (!QuestionSlot[i].Solved) unsolvedSpots.Add(QuestionSlot[i]);
        }

        if (unsolvedSpots.Count == 0)
        {
            Debug.Log("all spots are solved, hint unused");
            return;
        }

        QuestionPicture hintedPicture = unsolvedSpots[Random.Range(0, unsolvedSpots.Count)];

        Vector2Int correctPosition = new Vector2Int();
        List<AnswerSlot> candidate = new List<AnswerSlot>(Answers);

        for (int i = candidate.Count - 1; i >= 0; i--)
        {
            if (candidate[i].Target == hintedPicture.gameObject)
            {
                correctPosition = candidate[i].Position;
                candidate.RemoveAt(i);
            }
            else if (candidate[i].Position == hintedPicture.Position)
            {
                candidate.RemoveAt(i);
            }
        }
        AnswerHintHighlight[0].transform.localPosition = new Vector3(0.02f + correctPosition.x * 1.5f, -0.01f + correctPosition.y * 1.12f, 7);
        AnswerHintHighlight[0].gameObject.SetActive(true);

        if (candidate.Count > 0)
        {
            int otherPositionIdx1 = Random.Range(0, candidate.Count);
            Vector2Int otherPosition1 = candidate[otherPositionIdx1].Position;
            AnswerHintHighlight[1].transform.localPosition = new Vector3(0.02f + otherPosition1.x * 1.5f, -0.01f + otherPosition1.y * 1.12f, 7);
            AnswerHintHighlight[1].gameObject.SetActive(true);
            candidate.RemoveAt(otherPositionIdx1);
        }
        if (candidate.Count > 0)
        {
            int otherPositionIdx2 = Random.Range(0, candidate.Count);
            Vector2Int otherPosition2 = candidate[otherPositionIdx2].Position;
            AnswerHintHighlight[2].transform.localPosition = new Vector3(0.02f + otherPosition2.x * 1.5f, -0.01f + otherPosition2.y * 1.12f, 7);
            AnswerHintHighlight[2].gameObject.SetActive(true);
            candidate.RemoveAt(otherPositionIdx2);
        }

        QuestionHintHighlight.transform.localPosition = new Vector3(hintedPicture.Position.x * 1.5f, hintedPicture.Position.y * 1.12f, 10);
        QuestionHintHighlight.SetActive(true);
    }

    private void HideHint()
    {
        for (int i = 0; i < AnswerHintHighlight.Count; i++)
        {
            AnswerHintHighlight[i].gameObject.SetActive(false);
        }
        QuestionHintHighlight.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        //ActivateCheat();
    }
}
