using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    private const float MAX_TIME_TO_GET_ACHIEVEMENT = 1200f;
    private const int FIRST_ACHIEVEMENT = 0;
    private const int SECOND_ACHIEVEMENT = 1;
    private const int THIRD_ACHIEVEMENT = 2;
    private const int FOURTH_ACHIEVEMENT = 3;
    private const int FIFTH_ACHIEVEMENT = 4;
    private const int SIXTH_ACHIEVEMENT = 5;
    private const int SEVENTH_ACHIEVEMENT = 6;
    private const int EIGHTH_ACHIEVEMENT = 7;
    private const int NINTH_ACHIEVEMENT = 8;
    private const int TENTH_ACHIEVEMENT = 9;
    private const int ELEVENTH_ACHIEVEMENT = 10;
    private const int TWELVETH_ACHIEVEMENT = 11;

    public static AchievementManager Instance;

    [SerializeField] private GameObject achievementsList;
    private Canvas achievementPopUpCanvas;
    private AchievementsUI[] achievements;
    private string title;
    private string description;
    private bool oneChestOpened = false;
    private bool oneChestClosed = false;
    private bool oneCollectibleNotCollected = false;
    private bool oneAchievementNotAchieved = false;
    private bool achievementDisplayed = false;
    private int chestsOpened = 0;
    private int collectibleCollected = 0;

    private Queue<AchievementContainer> queuedAchievements;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        achievementPopUpCanvas = GameObject.FindGameObjectWithTag(Harmony.Tags.AchievementPopUpCanvas).GetComponent<Canvas>();
        achievements = achievementsList.GetComponentsInChildren<AchievementsUI>();
        queuedAchievements = new Queue<AchievementContainer>();
    }

    private void Update()
    {
        if (!achievementDisplayed && queuedAchievements.Count > 0)
        {
            StartCoroutine(DisplayNextAchievement());
        }
    }

    public void UnlockPrisonAchievement()
    {
        UnlockAchievement(Achievements.AchievementsEnum.BB, EIGHTH_ACHIEVEMENT);
    }

    public void UnlockGrottoAchievement()
    {
        UnlockAchievement(Achievements.AchievementsEnum.BOT, TENTH_ACHIEVEMENT);
    }

    public void UnlockVillageAchievement()
    {
        UnlockAchievement(Achievements.AchievementsEnum.FA, NINTH_ACHIEVEMENT);
    }

    public void UnlockMountainAchievement()
    {
        UnlockAchievement(Achievements.AchievementsEnum.GAC, ELEVENTH_ACHIEVEMENT);
    }

    public void UnlockAllChestsAchievements()
    {
        foreach (Chest chest in XMLManager.Instance.savedData.chestsDB.listOfChest)
        {
            if (chest.closed)
            {
                oneChestClosed = true;
            }
            else
            {
                chestsOpened++;
            }
        }
        if (!oneChestClosed)
        {
            UnlockAchievement(Achievements.AchievementsEnum.UPU, FOURTH_ACHIEVEMENT);
        }
        else
        {
            ShowChestCountPopUp();
        }
        oneChestClosed = false;
        chestsOpened = 0;
    }

    public void UnlockNoChestAchievements()
    {
        foreach (Chest chest in XMLManager.Instance.savedData.chestsDB.listOfChest)
        {
            if (!chest.closed)
            {
                oneChestOpened = true;
                break;
            }
        }
        if (!oneChestOpened)
        {
            UnlockAchievement(Achievements.AchievementsEnum.CW, SIXTH_ACHIEVEMENT);
        }
        oneChestOpened = false;
    }

    public void UnlockAllCollectiblesAchievement()
    {
        foreach (Collectible collectible in XMLManager.Instance.savedData.collectibleDB.listOfCollectibles)
        {
            if (!collectible.collected)
            {
                oneCollectibleNotCollected = true;
            }
            else
            {
                collectibleCollected++;
            }
        }
        if (!oneCollectibleNotCollected)
        {
            UnlockAchievement(Achievements.AchievementsEnum.TH, SECOND_ACHIEVEMENT);
        }
        else
        {
            ShowCollectibleCountPopUp();
        }
        oneCollectibleNotCollected = false;
        collectibleCollected = 0;
    }

    public void UnlockAllAchievementsAchievement()
    {
        foreach (Achievement achievement in XMLManager.Instance.savedData.achievementDB.listOfAchievements)
        {
            if (!achievement.achieved && achievement.id != 0)
            {
                oneAchievementNotAchieved = true;
                break;
            }
        }
        if (!oneAchievementNotAchieved)
        {
            UnlockAchievement(Achievements.AchievementsEnum.BOTB, FIRST_ACHIEVEMENT);
        }
        oneAchievementNotAchieved = false;
    }

    public void UnlockNoDeathAchievement()
    {
        if (XMLManager.Instance.savedData.playerDeaths == 0)
        {
            UnlockAchievement(Achievements.AchievementsEnum.BD, THIRD_ACHIEVEMENT);
        }
    }

    public void UnlockYetiAchievement()
    {
        UnlockAchievement(Achievements.AchievementsEnum.WBN, FIFTH_ACHIEVEMENT);
    }

    public void UnlockDarkhenAchievement()
    {
        UnlockAchievement(Achievements.AchievementsEnum.TPC, TWELVETH_ACHIEVEMENT);
    }

    public void UnlockSpeedRunnerAchievement()
    {
        if (XMLManager.Instance.savedData.timer < MAX_TIME_TO_GET_ACHIEVEMENT)
        {
            UnlockAchievement(Achievements.AchievementsEnum.SPEEDRUNNER, SEVENTH_ACHIEVEMENT);
        }
    }

    public void UnlockEndGameAchievement()
    {
        UnlockDarkhenAchievement();
        UnlockSpeedRunnerAchievement();
        UnlockNoDeathAchievement();
        UnlockNoChestAchievements();
    }

    private void UnlockAchievement(Achievements.AchievementsEnum achievement, int achievementNumber)
    {
        if (!XMLManager.Instance.savedData.achievementDB.listOfAchievements[achievementNumber].achieved) 
        {
            queuedAchievements.Enqueue(new AchievementContainer(achievement, achievementNumber));
        }
    }

    IEnumerator DisplayNextAchievement()
    {
        achievementDisplayed = true;
        AchievementContainer achievement = queuedAchievements.Dequeue();
        title = Achievements.GetTitleForAchievement(achievement.AchievementName);
        description = Achievements.GetDescriptionForAchievement(achievement.AchievementName);
        achievementPopUpCanvas.GetComponent<SetAchievement>().SetText(title, description);
        achievements[achievement.AchievementNumber].SetAchieved(achievement.AchievementNumber, true);

        yield return new WaitForSeconds(3f);
        achievementDisplayed = false;
    }

    public void ShowChestCountPopUp()
    {
        title = Achievements.GetTitleForAchievement(Achievements.AchievementsEnum.UPU);
        description = "Chests opened: " + chestsOpened + "/" + XMLManager.Instance.savedData.chestsDB.listOfChest.Count;
        achievementPopUpCanvas.GetComponent<SetAchievement>().SetText(title, description);
    }

    public void ShowCollectibleCountPopUp()
    {
        title = Achievements.GetTitleForAchievement(Achievements.AchievementsEnum.TH);
        description = "Collectibles collected: " + collectibleCollected + "/" + XMLManager.Instance.savedData.collectibleDB.listOfCollectibles.Count;
        achievementPopUpCanvas.GetComponent<SetAchievement>().SetText(title, description);
    }
    public struct AchievementContainer
    {
        public AchievementContainer(Achievements.AchievementsEnum achievementName, int achievementNumber)
        {
            AchievementName = achievementName;
            AchievementNumber = achievementNumber;
        }

        public Achievements.AchievementsEnum AchievementName { get; }
        public int AchievementNumber { get; }
    }
}