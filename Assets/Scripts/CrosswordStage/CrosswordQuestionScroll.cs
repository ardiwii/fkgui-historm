using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CrosswordQuestionScroll : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Transform ImagePoint;
    [SerializeField] float MaxDistance;

    Vector3 OriginalPoint;

    private void Start()
    {
        OriginalPoint = ImagePoint.transform.position;
    }

    public void OnChange()
    {
        ImagePoint.position = ImagePoint.position.WithY(OriginalPoint.y + (slider.value * MaxDistance));
    }

}
