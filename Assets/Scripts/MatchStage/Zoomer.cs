using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Zoomer : MonoBehaviour
{
    [SerializeField] Image ZoomedImage;
    [SerializeField] TextMeshProUGUI Tex;
    [SerializeField] GameObject molaraHintButton;
    [SerializeField] Image molaraImage;

    public void Setup(QuestionPicture Pic, bool showHintButton)
    {
        ZoomedImage.sprite = Pic.GetComponent<SpriteRenderer>().sprite;
        
        gameObject.SetActive(true);
        molaraHintButton.SetActive(showHintButton);
        Tex.text = Pic.hintText;
        Tex.gameObject.SetActive(true);
        molaraImage.sprite = ZoomedImage.sprite;
    }

    public void Setup(Sprite Pic, string Des = "")
    {
        ZoomedImage.sprite = Pic;
        if (Des != "")
        {
            Tex.text = Des;
            Tex.gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        Tex?.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
