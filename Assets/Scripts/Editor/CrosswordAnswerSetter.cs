using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrosswordAnswerSetter : ScriptableWizard
{

    public string Answer;
    CrosswordGroup CurrentGroup;

    [MenuItem("Tools/Crossword/Create Crossword Answer")]
    public static void CreateWizard()
    {
        CrosswordGroup NowGroup = Selection.activeGameObject.GetComponent<CrosswordGroup>();
        if (!NowGroup)
        {
            Debug.LogError("Not Crossoword Group");
            return;
        }
        CrosswordAnswerSetter Generator = DisplayWizard<CrosswordAnswerSetter>("Create Crossword Group Answer", "Create");
        Generator.CurrentGroup = NowGroup;
    }

    public void OnWizardCreate()
    {
        Answer = Answer.ToUpper();
        char[] AnswerLetter = Answer.ToCharArray();
        if(CurrentGroup.CrossType == CrosswordGroupType.Horizontal)
        {
            CurrentGroup.InputElements.Sort(HorizontalInputSorter);
        }
        else
        {
            CurrentGroup.InputElements.Sort(VerticalInputSorter);
        }
        EditorUtility.SetDirty(CurrentGroup);
        
        List<CrosswordInput> Inputs = CurrentGroup.InputElements;
        for (int i = 0; i < AnswerLetter.Length; i++)
        {
            if (i >= Inputs.Count) break;
            Inputs[i].CorrectAnswer = AnswerLetter[i].ToString();
            EditorUtility.SetDirty(Inputs[i]);
        }

    }

    int HorizontalInputSorter(CrosswordInput x, CrosswordInput y)
    {
        return (x.transform.position.x.CompareTo(y.transform.position.x));
    }

    int VerticalInputSorter(CrosswordInput x, CrosswordInput y)
    {
        return  (y.transform.position.y.CompareTo(x.transform.position.y));
    }
}


