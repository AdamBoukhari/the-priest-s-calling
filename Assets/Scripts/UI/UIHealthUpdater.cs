using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthUpdater : MonoBehaviour
{
    const float HALF_HEART_VALUE = 0.5f;
    [SerializeField] private RawImage[] hearts;

    public void UpdateHealthUI(int currentMaxHearts, float currentHearts)
    {
        for (int i = 0; i < currentMaxHearts; i++)
        {
            hearts[i].enabled = true;
            if (i < currentHearts - HALF_HEART_VALUE)
            {
                hearts[i].texture = Consts.T_FULL_HEART;
            }
            else if (i == currentHearts - HALF_HEART_VALUE)
            {
                hearts[i].texture = Consts.T_HALF_HEART;
            }
            else
            {
                hearts[i].texture = Consts.T_EMPTY_HEART;
            }
        }
        for (int i = currentMaxHearts; i < hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }
    }

    public void AddMaxHeartUI(int currentMaxHearts, float currentHearts)
    {
        for(int i = 0; i < currentMaxHearts; i++)
        {
            hearts[i].enabled = true;
        }
        UpdateHealthUI(currentMaxHearts, currentHearts);
    }
}
