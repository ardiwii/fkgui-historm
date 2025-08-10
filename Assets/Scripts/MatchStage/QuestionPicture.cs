using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestionPicture : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField]TextMeshPro CheatText;

    public Vector2Int Position
    {
        get
        {
            return new Vector2Int(Mathf.RoundToInt(transform.localPosition.x/MatchStageManager.HorizontalRange), Mathf.RoundToInt(transform.localPosition.y/ MatchStageManager.VerticalRange));
        }
    }

    Difficulty CurrentDifficutly => MatchStageManager.Instance.CurrentDifficulty;

    SpriteRenderer Img;
    [HideInInspector] public bool Solved;

    Vector3 PrevPos;
    float DeltaFactor;
    bool RaycastBlocked = false;
    public Vector3 LastPosition;
    public Vector2Int RealLastPosition=> new Vector2Int(Mathf.RoundToInt(LastPosition.x / MatchStageManager.HorizontalRange), Mathf.RoundToInt(LastPosition.y / MatchStageManager.VerticalRange));
    public string hintText;
    const float DragScale = 1.1f;


    MatchStageManager Manager => (MatchStageManager)StageManager.Instance;
    const float DragThreshold = 1f;
    bool Dragging = false;

    private void Awake()
    {
        Img = GetComponent<SpriteRenderer>();
    }

    public void Setup(Sprite pic, string hint)
    {
        Img.sprite = pic;
        name = pic.name;
        LastPosition = transform.localPosition;
        hintText = hint;
    }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CurrentDifficutly == Difficulty.hard) return;
        Dragging = true;
        transform.position = eventData.pointerCurrentRaycast.worldPosition.WithZ(transform.position.z);
        Manager.PiecePick(this);
        transform.localScale = Vector3.one * DragScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CurrentDifficutly == Difficulty.hard) return;
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            
            if (!RaycastBlocked)
            {
                DeltaFactor = eventData.delta.magnitude / (eventData.pointerCurrentRaycast.worldPosition - PrevPos).magnitude;
                RaycastBlocked = false;
            }

            PrevPos = eventData.pointerCurrentRaycast.worldPosition;

            transform.position = eventData.pointerCurrentRaycast.worldPosition.WithZ(transform.position.z);
            Manager.PieceDrag(this);

        }
        else
        {
            RaycastBlocked = true;
            transform.Translate(eventData.delta / DeltaFactor);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CurrentDifficutly == Difficulty.hard) return;
        Dragging = false;
        transform.localScale = Vector3.one;
        Manager.PieceDrop(this, eventData.pointerCurrentRaycast.worldPosition);
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        //print(eventData.button);
        if(eventData.button == PointerEventData.InputButton.Right) Manager.EnterZoom(this);
        if (CurrentDifficutly == Difficulty.normal) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
            Manager.MoveToEmpty(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Manager.SelectQuestion(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Manager.UnSelect();
    }

    

    public void ActivateCheat(string Tex)
    {
        CheatText.text = Tex;
        CheatText.gameObject.SetActive(true);
        
    }
}
