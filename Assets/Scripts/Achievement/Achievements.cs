using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    private const string BOTB_TITLE = "The best of the bests";
    private const string BOTB_DESCRIPTION = "Unlock every achievements";
    private const string TH_TITLE = "Treasure Hunter";
    private const string TH_DESCRIPTION = "Collect every collectible";
    private const string BD_TITLE = "I'm just built DIFFERENT";
    private const string BD_DESCRIPTION = "Beat the game without losing a heart";
    private const string UPU_TITLE = "UPGRADES PEOPLE,UPGRADES";
    private const string UPU_DESCRIPTION = "Open every chest";
    private const string WBN_TITLE = "Where's Bigfoot now?";
    private const string WBN_DESCRIPTION = "Kill the yeti";
    private const string SPEEDRUNNER_TITLE = "Speedrunner";
    private const string SPEEDRUNNER_DESCRIPTION = "Finish the game within 20 minutes";
    private const string CW_TITLE = "Chest what?";
    private const string CW_DESCRIPTION = "Finish the game without opening a single chest";
    private const string BB_TITLE = "Behind bars";
    private const string BB_DESCRIPTION = "Escape from prison";
    private const string FA_TITLE = "Fresh air";
    private const string FA_DESCRIPTION = "Reach the village";
    private const string GAC_TITLE = "Got a cold?";
    private const string GAC_DESCRIPTION = "Reach the castle";
    private const string BOT_TITLE = "Back on track";
    private const string BOT_DESCRIPTION = "Reach Scarlet Mountains";
    private const string TPC_TITLE = "The Priest's calling";
    private const string TPC_DESCRIPTION = "Defeat Darkhen";

    public enum AchievementsEnum
    {
        BOTB,
        TH,
        BD,
        UPU,
        WBN,
        SPEEDRUNNER,
        CW,
        BB,
        FA,
        GAC,
        BOT,
        TPC,
    };

    public static string GetTitleForAchievement(AchievementsEnum achievement)
    {
        switch (achievement)
        {
            case AchievementsEnum.BOTB:         return BOTB_TITLE;
            case AchievementsEnum.TH:           return TH_TITLE;
            case AchievementsEnum.BD:           return BD_TITLE;
            case AchievementsEnum.UPU:          return UPU_TITLE;
            case AchievementsEnum.WBN:          return WBN_TITLE;
            case AchievementsEnum.SPEEDRUNNER:  return SPEEDRUNNER_TITLE;
            case AchievementsEnum.CW:           return CW_TITLE;
            case AchievementsEnum.BB:           return BB_TITLE;
            case AchievementsEnum.FA:           return FA_TITLE;
            case AchievementsEnum.GAC:          return GAC_TITLE;
            case AchievementsEnum.BOT:          return BOT_TITLE;
            case AchievementsEnum.TPC:          return TPC_TITLE;
        }
        return null;
    }
    public static string GetDescriptionForAchievement(AchievementsEnum achievement)
    {
        switch (achievement)
        {
            case AchievementsEnum.BOTB:         return BOTB_DESCRIPTION;
            case AchievementsEnum.TH:           return TH_DESCRIPTION;
            case AchievementsEnum.BD:           return BD_DESCRIPTION;
            case AchievementsEnum.UPU:          return UPU_DESCRIPTION;
            case AchievementsEnum.WBN:          return WBN_DESCRIPTION;
            case AchievementsEnum.SPEEDRUNNER:  return SPEEDRUNNER_DESCRIPTION;
            case AchievementsEnum.CW:           return CW_DESCRIPTION;
            case AchievementsEnum.BB:           return BB_DESCRIPTION;
            case AchievementsEnum.FA:           return FA_DESCRIPTION;
            case AchievementsEnum.GAC:          return GAC_DESCRIPTION;
            case AchievementsEnum.BOT:          return BOT_DESCRIPTION;
            case AchievementsEnum.TPC:          return TPC_DESCRIPTION;
        }
        return null;
    }
}
