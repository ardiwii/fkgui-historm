using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizAnswerUI : MonoBehaviour
{
    [SerializeField] protected PostGameQuizManager quizManager;

    public virtual void ShowUnanswered(QuizQuestionData data)
    {

    }

    public virtual void ShowAnswered(QuizQuestionData data)
    {

    }
}
