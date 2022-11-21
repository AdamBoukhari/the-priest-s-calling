using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SetAchievement : MonoBehaviour
{
    private Canvas achievementPopUpCanvas;
    private TextMeshProUGUI[] titleDescription;

    private void Awake()
    {
        achievementPopUpCanvas = GetComponent<Canvas>();
        titleDescription = achievementPopUpCanvas.GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void SetText(string title, string description)
    {
        titleDescription[0].text = title;
        titleDescription[1].text = description;
        achievementPopUpCanvas.GetComponent<Animator>().SetTrigger("AchievementUnlocked");
    }
}
