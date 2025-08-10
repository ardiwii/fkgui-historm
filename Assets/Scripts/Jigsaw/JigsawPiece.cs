using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JigsawPiece : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
{

    public JigsawAnswer CurrentAnswer;
    public Vector3 LastPosition;
    [SerializeField] SpriteRenderer img;

    Vector3 PrevPos;
    float DeltaFactor;
    bool RaycastBlocked = false;
    const float DragScale = 1.1f;
    public bool IsHint=false;

    JigsawManager Manager => (JigsawManager)StageManager.Instance;

    private void Start()
    {
        LastPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsHint) return;
        transform.position = eventData.pointerCurrentRaycast.worldPosition.WithZ(transform.position.z);
        Manager.PiecePick(this);
        transform.localScale = Vector3.one * DragScale;
        if (CurrentAnswer)
        {
            CurrentAnswer.CurrentPiece = null;
            CurrentAnswer = null;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsHint) return;
        transform.localScale = Vector3.one;
        Manager.PieceDrop(this,eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsHint) return;
        if (eventData.pointerCurrentRaycast.worldPosition != Vector3.zero)
        {
            //print(eventData.delta.magnitude / (eventData.pointerCurrentRaycast.worldPosition - PrevPos).magnitude);
            if (!RaycastBlocked)
            {
                DeltaFactor = eventData.delta.magnitude / (eventData.pointerCurrentRaycast.worldPosition - PrevPos).magnitude;
                RaycastBlocked = false;
            }

            PrevPos = eventData.pointerCurrentRaycast.worldPosition;
            
            transform.position = eventData.pointerCurrentRaycast.worldPosition.WithZ(transform.position.z);
        }
        else
        {
            RaycastBlocked = true;
            transform.Translate(eventData.delta/DeltaFactor);
        }
        

        //print(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void SetImage(Sprite spr)
    {
        img.sprite = spr;
    }
}
