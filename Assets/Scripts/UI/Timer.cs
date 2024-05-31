using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : UIBase
{
    [SerializeField] private TextMeshProUGUI timerText;

    public void SetTimer(float time)
    {
        int timeMs = Mathf.CeilToInt(time * 1000);
        TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, timeMs);
        if (timeSpan.Milliseconds < 0)
        {
            timerText.text = "00:00";
            return;
        }
        int msTwoDigits = timeSpan.Milliseconds > 99 ? timeSpan.Milliseconds / 10 : timeSpan.Milliseconds;
        timerText.text = $"{timeSpan.Seconds:D2}:{msTwoDigits}";
    }
}
