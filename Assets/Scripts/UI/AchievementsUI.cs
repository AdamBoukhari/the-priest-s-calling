using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsUI : MonoBehaviour
{
    private const float ACHIEVED_R = 255;
    private const float ACHIEVED_G = 188;
    private const float ACHIEVED_B = 0;
    [SerializeField] private int id;
    private bool achieved = false;

    private void Awake()
    {
        foreach (Achievement achievement in XMLManager.Instance.savedData.achievementDB.listOfAchievements)
        {
            if (achievement.id == id)
            {
                if (achievement.achieved)
                {
                    gameObject.GetComponent<Image>().color = new Color(ACHIEVED_R, ACHIEVED_G, ACHIEVED_B);
                }
                else
                {
                    gameObject.GetComponent<Image>().color = Color.white;
                }
                achieved = achievement.achieved;
                break;
            }
        }
        Publisher.PushData += LoadAchievements;
        Publisher.FetchData += SaveAchievements;
    }
    private void OnEnable()
    {
        foreach (Achievement achievement in XMLManager.Instance.savedData.achievementDB.listOfAchievements)
        {
            if (achievement.id == id)
            {
                if (achievement.achieved)
                {
                    gameObject.GetComponent<Image>().color = new Color(ACHIEVED_R, ACHIEVED_G, ACHIEVED_B);
                }
                else
                {
                    gameObject.GetComponent<Image>().color = Color.white;
                }
                achieved = achievement.achieved;
                break;
            }
        }
        Publisher.PushData -= LoadAchievements;
        Publisher.FetchData -= SaveAchievements;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadAchievements;
        Publisher.FetchData -= SaveAchievements;
    }

    public void SetAchieved(int id, bool achievementAchieved)
    {
        achieved = achievementAchieved;
        XMLManager.Instance.savedData.achievementDB.listOfAchievements[id].achieved = achieved;
        if(!XMLManager.Instance.savedData.achievementDB.listOfAchievements[0].achieved)
        {
            AchievementManager.Instance.UnlockAllAchievementsAchievement();
        }
    }

    private void SaveAchievements(bool switchScene)
    {
        foreach (Achievement achievement in XMLManager.Instance.savedData.achievementDB.listOfAchievements)
        {
            if (achievement.id == id)
            {
                achievement.achieved = achieved;
            }
            break;
        }
    }

    private void LoadAchievements(bool changeScene, bool playerDead)
    {
        foreach (Achievement achievement in XMLManager.Instance.savedData.achievementDB.listOfAchievements)
        {
            if (achievement.id == id)
            {
                if (achievement.achieved)
                {
                    gameObject.GetComponent<Image>().color = new Color(ACHIEVED_R, ACHIEVED_G, ACHIEVED_B);
                }
                achieved = achievement.achieved;
                break;
            }
        }
    }
}
