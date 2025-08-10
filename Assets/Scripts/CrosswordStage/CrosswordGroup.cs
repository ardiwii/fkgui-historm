using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CrosswordGroupType
{
    Horizontal,
    Vertical
}

public class CrosswordGroup : MonoBehaviour
{
    [SerializeField] Image Highlight;
    public CrosswordGroupType CrossType;
    public CrosswordQuestion  Question;
    public CrosswordQuestion NextQuestion;
    public List<CrosswordInput> InputElements = new List<CrosswordInput>();

    CrosswordStageManager Manager => (CrosswordStageManager)StageManager.Instance;

    public void GoNext()
    {
        if (InputElements.Contains(Manager.LastCrossword))
        {
            int NextIndex = InputElements.IndexOf(Manager.LastCrossword) + 1;
            while (NextIndex < InputElements.Count && !InputElements[NextIndex].Input.enabled)
            {
                NextIndex+=1;
            }
            
            if (NextIndex < InputElements.Count)
            {
                CrosswordInput NextInput = InputElements[NextIndex];
                NextInput.AutoSelect();
            }
            else
            {
                if(NextQuestion) Manager.QuestionPressed(NextQuestion);
                //Deactivate();
                //Question.Disabled();
                //NextQuestion?.Onpressed();
            }
        }
    }

    public void GoBack()
    {
        if (InputElements.Contains(Manager.LastCrossword))
        {
            int NextIndex = InputElements.IndexOf(Manager.LastCrossword) - 1;
            while (NextIndex >= 0 && !InputElements[NextIndex].Input.enabled)
            {
                NextIndex -= 1;
            }

            if (NextIndex >= 0)
            {
                CrosswordInput NextInput = InputElements[NextIndex];
                NextInput.AutoSelect();
            }
        }
    }

    public void Activate()
    {
        Highlight.enabled = true;
    }

    public void Deactivate()
    {
        Highlight.enabled = false;
    }

    public void Selected(bool Override=false)
    {
        if (Override)
        {
            InputElements[0].AutoSelect();
        }
        Manager.CurrentGroup = this;
        Highlight.enabled = true;
        Question.Activate();
    }


}
