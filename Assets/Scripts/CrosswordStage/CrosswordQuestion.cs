using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrosswordQuestion : MonoBehaviour , IPointerClickHandler
{

    [SerializeField] Image Highlight;
    public CrosswordGroup Group;
    public Sprite Clue;
    [TextArea]public string ClueDescription;
    CrosswordStageManager Manager => (CrosswordStageManager)StageManager.Instance;

    public void Onpressed()
    {
        //Group.Selected(true);
        //Activate();
        Manager.QuestionPressed(this);
    }

    public void MouseEnter()
    {
        if (Clue != null)
        {
            Manager.ImageViewStart(this);
        }
    }

    public void MouseExit()
    {
        Manager.ImageViewFinished(this);
    }

    public void Activate()
    {
        Highlight.enabled = true;
    }

    public void Disabled()
    {
        Highlight.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Onpressed();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            MouseEnter();
        }
    }
}
