using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizTextAnswerUI : QuizAnswerUI
{
    [SerializeField] private GameObject unansweredPanel;
    [SerializeField] private GameObject answeredPanel;
    [SerializeField] private TMP_InputField inputAnswer;
    [SerializeField] private TextMeshProUGUI correctAnswer;

    public override void ShowAnswered(QuizQuestionData data)
    {
        base.ShowAnswered(data);
        answeredPanel.SetActive(true);
        unansweredPanel.SetActive(false);
        correctAnswer.text = data.answer;
    }

    public override void ShowUnanswered(QuizQuestionData data)
    {
        base.ShowUnanswered(data);
        answeredPanel.SetActive(false);
        unansweredPanel.SetActive(true);
    }

    public void InputAnswer()
    {
        bool isCorrect = quizManager.EvaluateAnswer(inputAnswer.text);
        if (isCorrect) inputAnswer.text = "";
    }
}
