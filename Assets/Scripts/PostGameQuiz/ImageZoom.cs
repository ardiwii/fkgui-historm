using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageZoom : MonoBehaviour
{
    [SerializeField] private Image sourceImage;
    [SerializeField] private Image zoomedImage;

    private void OnEnable()
    {
        zoomedImage.sprite = sourceImage.sprite;
    }
}
