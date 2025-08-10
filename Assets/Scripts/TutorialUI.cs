using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Image TutorImage;
    public TextMeshProUGUI PageTex;
    TutorialImageSO CurrentTutor;
    int CurrentPage;

    bool Transitioning = false;
    
    public void Setup()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        CurrentTutor = StageManager.Instance.TutorialAsset;
        CurrentPage = 0;
        TutorImage.sprite = CurrentTutor.Images[CurrentPage];
        PageTex.text = (CurrentPage + 1) + "/" + CurrentTutor.Images.Count;
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        Transitioning = true;
        transform.DOScale(1f, 0.25f).onComplete += delegate
        {
            Transitioning = false;
        };
        StageManager.Instance.SetGamePaused(true);
    }

    public void NextPressed()
    {
        if (Transitioning) return;
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        if (CurrentPage < CurrentTutor.Images.Count - 1)
        {
            CurrentPage++;
            TutorImage.sprite = CurrentTutor.Images[CurrentPage];
            PageTex.text = (CurrentPage+1) + "/" + CurrentTutor.Images.Count;
        }
    }

    public void PreviousPressed()
    {
        if (Transitioning) return;
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        if (CurrentPage >0 )
        {
            CurrentPage--;
            TutorImage.sprite = CurrentTutor.Images[CurrentPage];
            PageTex.text = (CurrentPage + 1) + "/" + CurrentTutor.Images.Count;
        }
    }

    public void TutupPressed()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
        Transitioning = true;
        transform.DOScale(0f, 0.25f).onComplete += delegate
        {
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
            Transitioning = false;
        };
        StageManager.Instance.SetGamePaused(false);
    }
}

