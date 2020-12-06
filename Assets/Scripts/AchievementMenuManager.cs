using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementMenuManager : MonoBehaviour
{
    public Text[] killTexts;
    public Text[] levelTexts;
    
    // Start is called before the first frame update
    void Start()
    {
        int kills = PlayerPrefs.GetInt("Kills");
        for (int i = 0; i < killTexts.Length; i++)
        {
            if (kills <= i)
                killTexts[i].color = new Color(0.75f, 0.75f, 0.75f);
        }

        int nextLevel = PlayerPrefs.GetInt("NextLevel");
        for (int i = 0; i < levelTexts.Length; i++)
        {
            if (nextLevel <= i)
                levelTexts[i].color = new Color(0.75f, 0.75f, 0.75f);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
