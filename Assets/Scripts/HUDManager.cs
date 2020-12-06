using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Image[] hearts;

    public Sprite heartFull, heartHalf, heartEmpty;

    public void UpdateHealth(float health)
    {
        int hpph = 100 / hearts.Length;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (health >= hpph * i + 3 * hpph / 4)
                hearts[i].sprite = heartFull;
            else if (health < hpph * i + hpph / 4)
                hearts[i].sprite = heartEmpty;
            else
                hearts[i].sprite = heartHalf;
        }
    }
}