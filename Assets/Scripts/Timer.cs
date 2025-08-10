using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TimerText;

    float CurrentTime;

    public int Hours=>Mathf.FloorToInt(CurrentTime/3600f);
    public int Minutes =>Mathf.FloorToInt((CurrentTime%3600f)/60f);
    public int Seconds => Mathf.FloorToInt((CurrentTime % 3600f) % 60f);

    public void SetTime(float time)
    {
        CurrentTime = time;
        TimerText.text = Hours.ToString("D2") + ":" + Minutes.ToString("D2") + ":" + Seconds.ToString("D2");
    }

    public void SetTimeWithPrefix(float time, string prefix)
    {
        CurrentTime = time;
        TimerText.text = prefix + Minutes.ToString("D2") + ":" + Seconds.ToString("D2");
    }
}
