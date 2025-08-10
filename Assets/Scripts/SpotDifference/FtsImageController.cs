using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FtsImageController : MonoBehaviour
{
    [SerializeField] private SpotManager manager;
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private RectTransform rootCanvas;
    [SerializeField] private GameObject spotMarkerPrefab;
    [SerializeField] private Image controlledImage;
    [SerializeField] private GameObject hintObject;

    public void SpotCheck()
    {
        Vector2 rectPosition = new Vector2(thisRect.position.x / rootCanvas.localScale.x, thisRect.position.y / rootCanvas.localScale.y);
        Vector2 mousePosition = new Vector2(Input.mousePosition.x / rootCanvas.localScale.x, Input.mousePosition.y / rootCanvas.localScale.y);
        Debug.Log("click event: click relative position: " + (mousePosition - rectPosition));
        Vector2 relativePosition = mousePosition - rectPosition;
        manager.CheckSpot(relativePosition);
    }

    public void SetImage(Sprite image)
    {
        controlledImage.sprite = image;
    }

    public void SpawnSpottedMarker(Vector2 position)
    {
        GameObject spotMarker = Instantiate(spotMarkerPrefab, transform);
        spotMarker.GetComponent<RectTransform>().localPosition = position;
    }

    public void ShowHint(Vector2 position)
    {
        hintObject.SetActive(true);
        hintObject.GetComponent<RectTransform>().localPosition = position;
    }

    public void HideHint()
    {
        hintObject.SetActive(false);
    }
}
