using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrosswordInput : MonoBehaviour
{

    public string CorrectAnswer;
    public TMP_InputField Input { get; private set; }
    public bool Solved;
    bool Filled => Input.text != string.Empty;
    CrosswordStageManager Manager => (CrosswordStageManager)StageManager.Instance;

    const float ClickDelay = 0.06f;
    float ClickCooldown = 0f;

    private void Awake()
    {
        Input = GetComponent<TMP_InputField>();
        Input.onSelect.AddListener(x =>Selected());
    }

    public void ValueChange()
    {
        switch (Input.text)
        {
            case "":

                break;
            case " ":
                Input.text = "";
                CheckCorrect();
                InputComplete();
                break;
            default:
                Input.text = Input.text.ToUpper();
                CheckCorrect();
                InputComplete();
                
                break;
        }
        
    }

    public void Pressed()
    {
        if (ClickCooldown<=0f && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            Selected();
            Manager.InputPressed(this);
            ClickCooldown = ClickDelay;
        }
    }

    public void Selected()
    {
        
    }

    public void AutoSelect()
    {
        if (!enabled)
        {
            
            return;
        }
        Input.Select();
        //if (Filled)
        //{
        //    Next?.AutoSelect();
        //}
    }

    public void CheckCorrect()
    {
        if (Input.text == CorrectAnswer)
        {
            print(Input.text + " Solved");
            Solved = true;
        }
        else
        {
            if (Solved)
            {
                Solved = false;
                print(Input.text + " UnSolved");
            }
        }
    }

    public void BecomeHint()
    {
        Input.textComponent.color = Color.red;
        Input.textComponent.DOColor(Input.textComponent.color.WithAlpha(0f), 0f);
        Input.textComponent.DOColor(Input.textComponent.color.WithAlpha(1f),0.4f);
        Input.text = CorrectAnswer;
        Input.enabled = false;
        
    }

    public void InputComplete()
    {
        Manager.InputComplete(this);
    }


    private void Update()
    {
        if (ClickCooldown > 0f)
        {
            ClickCooldown -= Time.deltaTime;
        }
        else if(ClickCooldown<0f)
        {
            ClickCooldown = 0f;
        }
    }
}

