using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony;

public class SceneTransition : MonoBehaviour
{
    private const int PRISON_ACHIEVEMENT_INDEX = 7;
    private const int VILLAGE_ACHIEVEMENT_INDEX = 8;
    private const int CAVERN_ACHIEVEMENT_INDEX = 9;
    private const int MOUNTAIN_ACHIEVEMENT_INDEX =10;

    private PlayerInteraction interactionScript;
    private Animator transition;

    void Start()
    {
        interactionScript = GetComponent<PlayerInteraction>();
        transition = GameObject.FindGameObjectWithTag(Tags.Transition)?.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.Prison)
        {
            StartCoroutine(WaitToLoadNextScene(1));
        }
        if (collision.gameObject.tag == Tags.Cavern)
        {
            if (SceneSwitcher.Instance.GetActualLevel() == 1 && XMLManager.Instance.savedData.achievementDB.listOfAchievements[PRISON_ACHIEVEMENT_INDEX].achieved != true)
            {
                AchievementManager.Instance.UnlockPrisonAchievement();
            }
            StartCoroutine(WaitToLoadNextScene(2));
        }
        if (collision.gameObject.tag == Tags.Village)
        {
            if (XMLManager.Instance.savedData.achievementDB.listOfAchievements[VILLAGE_ACHIEVEMENT_INDEX].achieved != true)
            {
                AchievementManager.Instance.UnlockVillageAchievement();
            }
            StartCoroutine(WaitToLoadNextScene(3));
        }
        if (collision.gameObject.tag == Tags.Mountain)
        {
            if (XMLManager.Instance.savedData.achievementDB.listOfAchievements[CAVERN_ACHIEVEMENT_INDEX].achieved != true)
            {
                AchievementManager.Instance.UnlockGrottoAchievement();
            }
            StartCoroutine(WaitToLoadNextScene(4));
        }
        if (collision.gameObject.tag == Tags.Castle)
        {
            if (XMLManager.Instance.savedData.achievementDB.listOfAchievements[MOUNTAIN_ACHIEVEMENT_INDEX].achieved != true)
            {
                AchievementManager.Instance.UnlockMountainAchievement();
            }
            StartCoroutine(WaitToLoadNextScene(5));
        }
    }

    IEnumerator WaitToLoadNextScene(int scene)
    {
        interactionScript?.DeactivatePlayerControls();
        transition?.SetTrigger("Transition");
        yield return new WaitForSeconds(2f);
        interactionScript?.ActivatePlayerControls();
        SceneSwitcher.Instance.LoadIntoSpecificLevel(scene, true);
    }
}
