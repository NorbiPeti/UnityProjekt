using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndAchievements : MonoBehaviour
{
    public Text scoreText;
    public Text achievementText;
    private int _score;
    private float _displayedTime;
    private int _killCount;
    private int _level;

    private void Start()
    {
        achievementText.text = "";
    }

    private void Update()
    {
        if (_displayedTime > 0 && Time.time - _displayedTime > 5f)
        {
            achievementText.text = "";
            _displayedTime = 0;
        }
    }

    public void AddScore(int score)
    {
        _score += score;
        scoreText.text = "Score: " + _score;
    }

    public void CountKill()
    {
        _killCount++;
        
        if (_killCount == 1)
            CheckAndDisplayAchievement("Kills", 1, "First kill");
        else if (_killCount == 25)
            CheckAndDisplayAchievement("Kills", 2, "25 kills");
        else if (_killCount == 50)
            CheckAndDisplayAchievement("Kills", 3, "50 kills");
    }

    public void NextLevel()
    {
        _level++;
        if (_level == 5)
            CheckAndDisplayAchievement("NextLevel", 1, "Level 5 completed");
        else if (_level == 8)
            CheckAndDisplayAchievement("NextLevel", 2, "Level 8 completed");
        else if (_level == 10)
            CheckAndDisplayAchievement("NextLevel", 3, "This is so next level");
    }

    private void CheckAndDisplayAchievement(string key, int value, string message)
    {
        if (PlayerPrefs.GetInt(key) >= value) return;
        achievementText.text = message;
        _displayedTime = Time.time;
        PlayerPrefs.SetInt(key, value);
    }
}