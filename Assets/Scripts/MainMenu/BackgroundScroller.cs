using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    private RawImage image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = (image.uvRect.position + new Vector2(-0.2f, 0.2f) * Time.deltaTime);
        newPos.x %= 1;
        newPos.y %= 1;
        image.uvRect = new Rect(newPos, image.uvRect.size);
    }
}
