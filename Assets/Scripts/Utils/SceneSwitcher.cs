using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    //Rich presence stuff
    private Discord.Discord discord;
    private long timestamp;
    private List<string> stateMapName = new List<string>();

    private Vector3 PRISON_CAVERN_POS = new Vector3(117, 22.90f, 0);
    private Vector3 CAVERN_PRISON_POS = new Vector3(-78f, -3.18f, 0);
    private Vector3 CAVERN_VILLAGE_POS = new Vector3(77.84f, -3.05f, 0);
    private Vector3 VILLAGE_CAVERN_POS = new Vector3(-22.14f, -5.50f, 0);
    private Vector3 CAVERN_MOUNTAIN_POS = new Vector3(-77.07f, 26.90f, 0);
    private Vector3 MOUNTAIN_CAVERN_POS = new Vector3(36.26f, -8.20f, 0);
    private Vector3 MOUNTAIN_CASTLE_POS = new Vector3(132.72f, 88.00f, 0);
    private Vector3 CASTLE_MOUNTAIN_POS = new Vector3(-81.46f, 16.90f, 0);
    private Vector3 TAVERN_POS = new Vector3(142f, -43.5f, 0);
    [HideInInspector] public Vector3 playerPositionOnNewSceneStart;
    private Animator transition;

    private int actualLevel = 0;

    private enum ActualLevel:int {Prison = 1, Cavern = 2, Village = 3, Mountain = 4, Castle = 5};
    public static SceneSwitcher Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        SeedMapNames();
        UpdateTimestamp();
        SetupRichPresence();

    }

    private void SeedMapNames()
    {
        stateMapName.Add("In Menus");
        stateMapName.Add("Escaping the Devil's End Prison");
        stateMapName.Add("Exploring the Creepy Grottos");
        stateMapName.Add("Strolling in Oakvale Village");
        stateMapName.Add("Climbing the Scarlet Mountain");
        stateMapName.Add("Dying in Evil Castle");
    }

    private void SetupRichPresence()
    {
        //DiscordRichPresence
        discord = new Discord.Discord(945322825068204092, (System.UInt64)Discord.CreateFlags.Default);
        UpdateActivity();   
    }

    public void UpdateTimestamp()
    {
        timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        UpdateActivity();
    }

    private void UpdateActivity()
    {
        if (discord == null)
        {
            return ;
        }
        Discord.ActivityManager activityManager = discord.GetActivityManager();
        Discord.Activity activity = new Discord.Activity
        {
            Details = stateMapName[actualLevel],
            State = "Struggling in the game",
            Timestamps = { Start = timestamp},
            Assets =
            {
                LargeImage = "logo"
            }
        };
        activityManager.UpdateActivity(activity, (res)=> { });
    }


    private void Update()
    {
        discord.RunCallbacks();
    }

    private void OnEnable()
    {
        Publisher.PushData += FetchActualLevel;
        Publisher.FetchData += SaveActualLevel;
    }

    private void OnDisable()
    {
        Publisher.PushData -= FetchActualLevel;
        Publisher.FetchData -= SaveActualLevel;
    }

    public void GoBackToMainMenu(float delay)
    {
        StartCoroutine(GoBackToMainMenuDelay(delay));
    }

    private IEnumerator GoBackToMainMenuDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        actualLevel = 0;
        SceneManager.LoadScene(actualLevel);
        UpdateActivity();
    }

    public void LoadIntoVillageEndGame()
    {
        StartCoroutine(PlayVillageEndGameDelay(2f));
    }

    IEnumerator PlayVillageEndGameDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        XMLManager.Instance.savedData.playerInfo.playerPosition = TAVERN_POS;
        actualLevel = 3;
        XMLManager.Instance.savedData.darkhenBeaten = true;
        XMLManager.Instance.Save(true);
        SceneManager.LoadScene(actualLevel);
        UpdateActivity();
    }

    public int GetActualLevel()
    {
        return actualLevel;
    }

    public void LoadIntoActualLevel()
    {
        StartCoroutine(PlayActualLevelDelay(1f));
    }

    IEnumerator PlayActualLevelDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(actualLevel);
        UpdateActivity();
    }

    public void LoadIntoSpecificLevel(int level, bool save)
    {
        if(save)
        {
            SetPlayerPositionInNextLevel(level);
        }
        actualLevel = level;
        XMLManager.Instance.Save(true);

        //Transition entre les scenes pour la beta
        StartCoroutine(LoadScene(level, save));
    }

    private IEnumerator LoadScene(int level, bool save)
    {
        if(save)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
        SceneManager.LoadScene(level);
        UpdateActivity();
    }

    public void LoadIntoSpecificLevel()
    {
        actualLevel = 1;
        SceneManager.LoadScene(1);
        UpdateActivity();
    }

    private void SetPlayerPositionInNextLevel(int level)
    {
        switch (actualLevel)
        {
            case (int)ActualLevel.Prison:
                if(level == (int)ActualLevel.Cavern)
                {
                    SavePlayerPosition(CAVERN_PRISON_POS);
                }
                break;
            case (int)ActualLevel.Cavern:
                if (level == (int)ActualLevel.Prison)
                {
                    SavePlayerPosition(PRISON_CAVERN_POS);
                }
                if (level == (int)ActualLevel.Village)
                {
                    SavePlayerPosition(VILLAGE_CAVERN_POS);
                }
                if (level == (int)ActualLevel.Mountain)
                {
                    SavePlayerPosition(MOUNTAIN_CAVERN_POS);
                }
                break;
            case (int)ActualLevel.Village:
                if (level == (int)ActualLevel.Cavern)
                {
                    SavePlayerPosition(CAVERN_VILLAGE_POS);
                }
                break;
            case (int)ActualLevel.Mountain:
                if (level == (int)ActualLevel.Cavern)
                {
                    SavePlayerPosition(CAVERN_MOUNTAIN_POS);
                }
                if (level == (int)ActualLevel.Castle)
                {
                    SavePlayerPosition(CASTLE_MOUNTAIN_POS);
                }
                break;
            case (int)ActualLevel.Castle:
                if (level == (int)ActualLevel.Mountain)
                {
                    SavePlayerPosition(MOUNTAIN_CASTLE_POS);
                }
                break;
        }
    }

    private void SavePlayerPosition(Vector3 pos)
    {
        XMLManager.Instance.savedData.playerInfo.playerPosition = pos;
    }

    private void FetchActualLevel(bool loadPosition, bool playerDead)
    {
        if (loadPosition)
        {
            actualLevel = XMLManager.Instance.savedData.playerInfo.levelId;
        }
    }

    private void SaveActualLevel(bool switchScene)
    {
        XMLManager.Instance.savedData.playerInfo.levelId = actualLevel;
    }
}

