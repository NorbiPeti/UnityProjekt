using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public Text scoreText;
    private int _score;

    public void AddScore(int score)
    {
        _score += score;
        scoreText.text = "Score: " + _score;
    }
}
