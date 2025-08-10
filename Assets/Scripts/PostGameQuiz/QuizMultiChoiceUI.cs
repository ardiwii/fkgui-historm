using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizMultiChoiceUI : QuizAnswerUI
{
    [SerializeField] private GameObject unansweredPanel;
    [SerializeField] private GameObject answeredPanel;
    [SerializeField] private List<MultiChoiceOption> unansweredChoices;
    [SerializeField] private List<MultiChoiceOption> answeredChoices;

    public override void ShowAnswered(QuizQuestionData data)
    {
        base.ShowAnswered(data);
        answeredPanel.SetActive(true);
        unansweredPanel.SetActive(false);
        int correctIdx = 0;
        switch (data.answer)
        {
            case "a": correctIdx = 0;break;
            case "b": correctIdx = 1; break;
            case "c": correctIdx = 2; break;
        }

        if (data.choices.Count == 2)
        {
            answeredChoices[2].gameObject.SetActive(false);
        }
        else if (data.choices.Count == 3)
        {
            answeredChoices[2].gameObject.SetActive(true);
        }
        for (int i = 0; i < data.choices.Count; i++)
        {
            answeredChoices[i].SetOption(data.choices[i]);
            if(i == correctIdx)
            {
                answeredChoices[i].SetSelected();
            }
            else
            {
                answeredChoices[i].SetUnselected();
            }
        }
    }

    public override void ShowUnanswered(QuizQuestionData data)
    {
        base.ShowUnanswered(data);
        answeredPanel.SetActive(false);
        unansweredPanel.SetActive(true);

        if(data.choices.Count == 2)
        {
            unansweredChoices[2].gameObject.SetActive(false);
        }
        else if(data.choices.Count == 3)
        {
            unansweredChoices[2].gameObject.SetActive(true);
        }
        for (int i = 0; i < data.choices.Count; i++)
        {
            unansweredChoices[i].SetOption(data.choices[i]);
            unansweredChoices[i].SetUnselected();
        }
    }

    public void SubmitAnswer(int choice)
    {
        switch (choice)
        {
            case 0: quizManager.EvaluateAnswer("a"); break;
            case 1: quizManager.EvaluateAnswer("b"); break;
            case 2: quizManager.EvaluateAnswer("c"); break;
        }
    }
}
