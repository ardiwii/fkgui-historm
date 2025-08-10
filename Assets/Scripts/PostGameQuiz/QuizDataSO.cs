using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PostGameQuizData", menuName = "ScriptableObjects/PostGameQuizData", order = 4)]
public class QuizDataSO : ScriptableObject
{
    public Sprite image;
    public List<QuizQuestionData> possibleQuestions;

}

[System.Serializable]
public class QuizQuestionData
{
    public QuizType type;
    [TextArea(1,2)]public string question;
    [TextArea]public List<string> choices;
    public string answer;
    [TextArea(1, 4)] public string molaraExplanation;
    
    public bool EvaluateAnswer(string userAnswer)
    {
        return string.Equals(userAnswer.ToLower(), answer.ToLower());
    }
}

public enum QuizType
{
    multiChoice,
    textInput
}
