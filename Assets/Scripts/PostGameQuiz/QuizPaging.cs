using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizPaging : MonoBehaviour
{
    [SerializeField] GameObject previousButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] TextMeshProUGUI questionPage;

    private int questionCount;

    public void Initialize(int _questionCount)
    {
        questionCount = _questionCount;
    }

    public void ScrollQuestion(int idx)
    {
        if(idx == 0)
        {
            previousButton.SetActive(false);
            nextButton.SetActive(true);
        }
        else if(idx == questionCount - 1)
        {
            previousButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else
        {
            previousButton.SetActive(true);
            nextButton.SetActive(true);
        }
        questionPage.text = (idx + 1) + "/" + questionCount;
    }
}
