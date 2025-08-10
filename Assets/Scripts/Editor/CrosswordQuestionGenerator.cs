using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CrosswordQuestionGenerator : ScriptableWizard
{

    public List<CrosswordQuestionList> MendatarQuestions = new List<CrosswordQuestionList>();
    public List<CrosswordQuestionList> MenurunQuestions = new List<CrosswordQuestionList>();

    Transform MendatarParent;
    Transform MenurunParent;

    [MenuItem("Tools/Crossword/Fill Questions")]
    public static void CreateWizard()
    {
        CrosswordQuestionGenerator Generator = DisplayWizard<CrosswordQuestionGenerator>("Create Crossword Questions", "Create");
    }

    public void OnWizardCreate()
    {
        MendatarParent = FindObjectByName("MendatarParent").transform;
        MenurunParent = FindObjectByName("MenurunParent").transform;
        if (!MendatarParent || !MenurunParent)
        {
            Debug.LogError("N/A");
            return;
        }
        CrosswordQuestion QuestionPrefab = AssetDatabase.LoadAssetAtPath<CrosswordQuestion>("Assets/Prefabs/Crossword/CrosswordQuestions.prefab");
        CrosswordQuestion QuestionImagePrefab = AssetDatabase.LoadAssetAtPath<CrosswordQuestion>("Assets/Prefabs/Crossword/CrosswordQuestionsImage.prefab");
        if (!QuestionPrefab)
        {
            Debug.LogError("N/A2");
            return;
        }
        foreach (var item in MendatarQuestions)
        {
            if (!item.ClueSprite)
            {
                CrosswordQuestion CQ = (CrosswordQuestion)PrefabUtility.InstantiatePrefab(QuestionPrefab, MendatarParent);
                CQ.name = item.Id.ToString();
                CQ.Group = item.Group;
                Undo.RecordObject(item.Group, "Group");
                item.Group.Question = CQ;
                TextMeshProUGUI TM = CQ.GetComponent<TextMeshProUGUI>();
                TM.text = item.Id + ".<indent=40>" + item.Description;
            }
            else
            {
                CrosswordQuestion CQ = (CrosswordQuestion)PrefabUtility.InstantiatePrefab(QuestionImagePrefab, MendatarParent);
                CQ.name = item.Id.ToString();
                CQ.Group = item.Group;
                Undo.RecordObject(item.Group, "Group");
                item.Group.Question = CQ;
                TextMeshProUGUI TM = CQ.GetComponent<TextMeshProUGUI>();
                TM.text = item.Id + ".<indent=40>\t\t";
                CQ.Clue = item.ClueSprite;
            }
        }
        foreach (var item in MenurunQuestions)
        {
            if (!item.ClueSprite)
            {
                CrosswordQuestion CQ = (CrosswordQuestion)PrefabUtility.InstantiatePrefab(QuestionPrefab, MenurunParent);
                CQ.name = item.Id.ToString();
                CQ.Group = item.Group;
                Undo.RecordObject(item.Group,"Group");
                item.Group.Question = CQ;
                TextMeshProUGUI TM = CQ.GetComponent<TextMeshProUGUI>();
                TM.text = item.Id + ".<indent=40>" + item.Description;
            }
            else
            {
                CrosswordQuestion CQ = (CrosswordQuestion)PrefabUtility.InstantiatePrefab(QuestionImagePrefab, MenurunParent);
                CQ.name = item.Id.ToString();
                CQ.Group = item.Group;
                Undo.RecordObject(item.Group, "Group");
                item.Group.Question = CQ;
                TextMeshProUGUI TM = CQ.GetComponent<TextMeshProUGUI>();
                TM.text = item.Id + ".<indent=40>\t\t";
                CQ.Clue = item.ClueSprite;
            }
        }
        MendatarParent = null;
        MenurunParent = null;
    }

    static GameObject FindObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name == name)
            {
                return objs[i].gameObject;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct CrosswordQuestionList
{

    public int Id;
    [TextArea]public string Description;
    public CrosswordGroup Group;
    public Sprite ClueSprite;
}
