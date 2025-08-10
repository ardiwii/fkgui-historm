using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CrossInputDirection
{
    Right,
    Down,
    Left,
    Up
}

public class CrosswordGenerator : ScriptableWizard
{


    public CrossInputDirection Direction;
    public int Number;
    public float Size = 49f;
    CrosswordInput CurrentInput;

    [MenuItem("Tools/Crossword/Create CrosswordInput")]
    public static void CreateWizard()
    {
        CrosswordInput NowInput = Selection.activeGameObject.GetComponent<CrosswordInput>();
        if (!NowInput)
        {
            Debug.LogError("Not Crossoword Input");
            return;
        }
        CrosswordGenerator Generator = DisplayWizard<CrosswordGenerator>("Create Crossword Input", "Create");
        Generator.CurrentInput = NowInput;
    }

    public void OnWizardCreate()
    {
        Vector2 CurrentDirection = Vector2.zero;
        CrosswordInput GO = PrefabUtility.GetCorrespondingObjectFromOriginalSource<CrosswordInput>(CurrentInput);
        switch (Direction)
        {
            case CrossInputDirection.Right:
                CurrentDirection = Vector2.right;
                break;
            case CrossInputDirection.Down:
                CurrentDirection = Vector2.down;
                break;
            case CrossInputDirection.Left:
                CurrentDirection = Vector2.left;
                break;
            case CrossInputDirection.Up:
                CurrentDirection = Vector2.up;
                break;
            default:
                break;
        }
        Vector2 CurrentPos = CurrentInput.transform.localPosition;
        for (int i = 0; i < Number; i++)
        {
            CurrentPos += (CurrentDirection * Size);
            CrosswordInput CI = (CrosswordInput) PrefabUtility.InstantiatePrefab(GO, CurrentInput.transform.parent);
            CI.transform.localPosition = CurrentPos;
            string Name = "LetterInput ";
            int Id = 0;
            while (CurrentInput.transform.parent.Find(Name+Id))
            {
                Id++;
            }
            CI.name = Name + Id;
            //Instantiate(GO, CurrentPos, Quaternion.identity, CurrentInput.transform.parent);
        }
        
    }
}
