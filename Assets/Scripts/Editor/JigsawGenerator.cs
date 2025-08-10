using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.VersionControl;

public class JigsawGenerator : ScriptableWizard
{

    public string Path;
    Transform AnswerParent;
    
    

    const string AnswerParentName = "AnswerZone";
    
    [MenuItem("Tools/Jigsaw/Fill Jigsaw")]
    public static void CreateWizard()
    {
        Transform CurrentAnswerParent = GameObject.Find(AnswerParentName)?.transform;
        if (!CurrentAnswerParent)
        {
            Debug.LogError("No Answers");
            return;
        }
        JigsawGenerator Generator = DisplayWizard<JigsawGenerator>("Fill Jigsaw", "Create");
        Generator.AnswerParent = CurrentAnswerParent;
    }

    public void OnWizardCreate()
    {
        if (!AssetDatabase.IsValidFolder(Path))
        {
            Debug.LogError("Path not exists");
            return;
        }

        List<JigsawAnswer> Answers = new List<JigsawAnswer>();
        Answers = AnswerParent.GetComponentsInChildren<JigsawAnswer>().ToList();
        char CurentChar = 'A';
        int CurrentId = 0;
        for (int i = 5; i > 0; i--)
        {
            
            for (int j = 0; j < 6; j++)
            {
                string FileName = CurentChar.ToString() + i;
                Sprite spr = AssetDatabase.LoadAssetAtPath<Sprite>(Path + "/" + FileName+".png");
                if (!spr)
                {
                    Debug.LogError("Can't found" + " " + FileName + ".png");
                    return;
                }
                Undo.RecordObject(Answers[CurrentId].img, "JigsawFill");
                
                Answers[CurrentId].img.sprite = spr;
                
                CurrentId++;
                CurentChar++;
                if (CurentChar == 'G')
                {
                    CurentChar = 'A';
                }
            }
        }
    }
}
