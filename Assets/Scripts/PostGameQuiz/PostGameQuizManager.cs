using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PostGameQuizManager : MonoBehaviour
{
    [SerializeField] private StageManager currentStageManager;
    [SerializeField] private QuizDataSO quizData;
    
    [SerializeField] private Image imageForQuestion;
    [SerializeField] private TextMeshProUGUI questionNumber;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private QuizMultiChoiceUI multiChoiceUI;
    [SerializeField] private QuizTextAnswerUI textAnswerUI;
    [SerializeField] private QuizPaging quizPaging;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject molaraPanel;
    [SerializeField] private Image imageOnMolara;
    [SerializeField] private TextMeshProUGUI molaraText;

    private QuizQuestionData currentQuestion;
    private int currentQuestionIdx;
    private HashSet<int> answeredQuestionIdxs;

    CanvasGroup group;

    private void OnEnable()
    {
        if (!group) group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
        SoundManager.PlaySound(SoundManager.Asset.Win);
        group.DOFade(1f, 1.25f);
    }

    public void InitializeQuiz()
    {
        answeredQuestionIdxs = new HashSet<int>();
        imageForQuestion.sprite = quizData.image;
        imageOnMolara.sprite = quizData.image;
        currentQuestionIdx = 0;
        quizPaging.Initialize(quizData.possibleQuestions.Count);
        SetQuestion(currentQuestionIdx);
    }

    private void SetQuestion(int questionId)
    {
        currentQuestion = quizData.possibleQuestions[questionId];
        questionNumber.text = (questionId + 1).ToString() + ".";

        quizPaging.ScrollQuestion(questionId);
        questionText.text = currentQuestion.question;
        
        if (currentQuestion.type == QuizType.multiChoice)
        {
            multiChoiceUI.gameObject.SetActive(true);
            textAnswerUI.gameObject.SetActive(false);
            if (answeredQuestionIdxs.Contains(questionId))
            {
                multiChoiceUI.ShowAnswered(currentQuestion);
            }
            else
            {
                multiChoiceUI.ShowUnanswered(currentQuestion);
            }
        }
        else
        {
            multiChoiceUI.gameObject.SetActive(false);
            textAnswerUI.gameObject.SetActive(true);
            if (answeredQuestionIdxs.Contains(questionId))
            {
                textAnswerUI.ShowAnswered(currentQuestion);
            }
            else
            {
                textAnswerUI.ShowUnanswered(currentQuestion);
            }
        }
    }

    public bool EvaluateAnswer(string answer)
    {
        if (currentQuestion.EvaluateAnswer(answer))
        {
            CorrectAnswer();
            return true;
        }
        else
        {
            currentStageManager.MakeMistake();
            return false;
        }
    }

    private void CorrectAnswer()
    {
        SoundManager.PlaySound(SoundManager.Asset.GetStar);
        answeredQuestionIdxs.Add(currentQuestionIdx);
        currentStageManager.avatar.SetExpression(Expression.happy);
        currentStageManager.SetGamePaused(true);
        questionPanel.SetActive(false);
        molaraPanel.SetActive(true);
        molaraText.text = currentQuestion.molaraExplanation;
        SetQuestion(currentQuestionIdx);
    }

    public void CloseMolara()
    {
        if (answeredQuestionIdxs.Count == quizData.possibleQuestions.Count)
        {
            gameObject.SetActive(false);
            currentStageManager.Finished();
        }
        else
        {
            currentStageManager.avatar.SetExpression(Expression.thinking);
            currentStageManager.SetGamePaused(false);
            questionPanel.SetActive(true);
            molaraPanel.SetActive(false);
        }
    }

    public void ScrollQuestion(bool isNext)
    {
        Debug.Log("scroll question");
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        currentQuestionIdx = isNext ? currentQuestionIdx+1 : currentQuestionIdx-1;
        SetQuestion(currentQuestionIdx);
    }
}
