using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CrosswordOrientation
{
    Horizontal,
    Vertical
}

public class CrosswordStageManager : StageManager
{

    public int HintNumber;
    [SerializeField] Transform GameParent;
    [SerializeField] Zoomer zoomer;
    [SerializeField] List<CrosswordInput> Letters = new List<CrosswordInput>();

    [HideInInspector] public CrosswordGroup CurrentGroup;
    [HideInInspector] public CrosswordInput LastCrossword;

    

    protected override void Initialize()
    {
        FindInputs();
        //if (!PlayerPrefs.HasKey("crosswordTutorialDone"))
        //{
        //    Tutorial.Setup();
        //    PlayerPrefs.SetInt("crosswordTutorialDone", 1);
        //}
    }

    void FindInputs()
    {
        Letters = GameParent.GetComponentsInChildren<CrosswordInput>().ToList();
        ChooseRandomHint();
    }

    public override void ActivateHint()
    {
        base.ActivateHint();
        for (int i = 0; i < 2; i++)
        {
            ChooseRandomHint();
        }
        DisableHint();
    }

    protected override void DisableHint()
    {
        base.DisableHint();
        hintTimer = 30f;
    }

    void ChooseRandomHint()
    {
        List<int> Numbers = new List<int>();
        for (int i = 0; i < HintNumber; i++)
        {
            int RandomId = Random.Range(0, Letters.Count);
            while (Numbers.Contains(RandomId))
            {
                RandomId = Random.Range(0, Letters.Count);
            }
            Numbers.Add(RandomId);
        }
        foreach (var item in Numbers)
        {
            CrosswordInput Chosen = Letters[item];
            Chosen.BecomeHint();
        }
    }

    public void InputComplete(CrosswordInput input)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        LastCrossword = input;
        CurrentGroup?.GoNext();
    }

    public void Back()
    {
        CrosswordInput ipt = EventSystem.current.currentSelectedGameObject.GetComponent<CrosswordInput>();
        if (ipt) 
        {
            LastCrossword = ipt;
            CurrentGroup?.GoBack();
        }
    }

    public void QuestionPressed(CrosswordQuestion quest)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        CurrentGroup?.Question.Disabled();
        CurrentGroup?.Deactivate();
        quest.Activate();
        quest.Group.Activate();
        CurrentGroup = quest.Group;
        CurrentGroup.InputElements[0].AutoSelect();
    }

    public void InputPressed(CrosswordInput input)
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        CurrentGroup?.Question.Disabled();
        CurrentGroup?.Deactivate();
        List<CrosswordGroup> Groups =  GameParent.GetComponentsInChildren<CrosswordGroup>().ToList();
        CrosswordGroup Selected = Groups.Find(x => x.InputElements.Contains(input) && x != CurrentGroup);
        if (Selected != null) CurrentGroup = Selected;
        CurrentGroup.Activate();
        CurrentGroup.Question.Activate();
    }

    public void ImageViewStart(CrosswordQuestion Question)
    {
        zoomer.Setup(Question.Clue,Question.ClueDescription);
    }

    public void ImageViewFinished(CrosswordQuestion Question)
    {
        zoomer.Exit();
    }

    public override void EnableCheat()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var item in Letters)
            {
                item.Input.text = item.CorrectAnswer;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Back();
        }
    }

    //public void 

    public void FinishPressed()
    {
        print(Letters.FindAll(x => x.Solved).Count +" "+ Letters.Count);
        if (Letters.FindAll(x => x.Solved).Count == Letters.Count)
        {
            Finished();
        }
        else
        {
            MakeMistake();
        }
    }
}


