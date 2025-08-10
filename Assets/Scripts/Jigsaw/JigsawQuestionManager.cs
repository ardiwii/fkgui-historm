using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JigsawQuestionManager : MonoBehaviour
{
    public List<MultipleChoice> Questions = new List<MultipleChoice>();

    [SerializeField] TextMeshProUGUI QuestionText;
    [SerializeField] TextMeshProUGUI[] AnswerText = new TextMeshProUGUI[3];
    [SerializeField] GameObject QuestionObject;

    int CurrentQuestionId = 0;
    JigsawManager Manager => (JigsawManager) StageManager.Instance;

    const float ZoomFactor = 1.8f;
    bool IsZoomed = false;
    [SerializeField] GameObject RaycastBlocker;
    [SerializeField] Transform ZoomedImage;


    public void Setup()
    {
        CurrentQuestionId = 0;
        LoadQuestion();
        QuestionObject.SetActive(true);
    }

    void LoadQuestion()
    {
        QuestionText.text = CurrentQuestionId + ". " + Questions[CurrentQuestionId].Question;
        for (int i = 0; i < 3; i++)
        {
            AnswerText[i].text = Questions[CurrentQuestionId].Answers[i];
        }
    }
    
    public void QuestionAnswered(int answer)
    {
        if((Answer)answer != Questions[CurrentQuestionId].ChoiceAnswer)
        {
            Manager.MakeMistake();
        }
        CurrentQuestionId++;
        if (CurrentQuestionId < Questions.Count)
        {
            LoadQuestion();
        }
        else
        {
            QuestionObject.SetActive(false);
            Manager.Finished();
        }
        
    }

    public void Zoom()
    {
        if (!IsZoomed)
        {
            RaycastBlocker.SetActive(true);
            ZoomedImage.localScale = Vector3.one * ZoomFactor;
            IsZoomed = true;
        }
        else
        {
            UnZoom();
        }
        
    }

    void UnZoom()
    {
        ZoomedImage.localScale = Vector3.one;
        RaycastBlocker.SetActive(false);
        IsZoomed = false;
    }

}

public enum Answer
{
    A,
    B,
    C
}

[System.Serializable]
public struct MultipleChoice
{
    [TextArea]public string Question;
    public Answer ChoiceAnswer;
    [TextArea] public string[] Answers;
}