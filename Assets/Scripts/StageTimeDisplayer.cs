using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTimeDisplayer : MonoBehaviour
{
    [SerializeField] Timer timer;
    float CurrentTime => StageManager.Instance.time;
    
    private void Update()
    {
        timer.SetTime(CurrentTime);
    }
}
