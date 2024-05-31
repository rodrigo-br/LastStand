using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetScore(int score)
    {
        scoreText.text = $"${score}";
    }
}
