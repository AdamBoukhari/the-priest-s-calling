using UnityEngine;
using System.Collections;

//Pour XML
using System.Collections.Generic; 
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

public class XMLManager : MonoBehaviour
{
    private const string SAVE_FILE_1 = "1.xml";
    private const string SAVE_FILE_2 = "2.xml";
    private const string SAVE_FILE_3 = "3.xml";
    private const string XML = ".xml";

    private const int SWORD_ID = 0;
    private const float BASE_SWORD_DAMAGE = 1f;
    private const float BASE_SWORD_COOLDOWN = 0.33f;
    private const int BASE_SWORD_COMBO = 3;
    private const float BASE_SWORD_RANGE = 0.5f;

    private const int DASH_ID = 1;
    private const float BASE_DASH_RANGE = 0.3f;
    private const float BASE_DASH_SPEED = 8f;

    private const int FIREBALL_ID = 2;
    private const float BASE_FIREBALL_DAMAGE = 2f;
    private const float BASE_FIREBALL_SPEED = 10f;
    private const float BASE_FIREBALL_COOLDOWN = 5f;

    private const int BASE_PLAYER_DEATH = 0;

    private const int BASE_PLAYER_HEALTH = 3;

    private const int BASE_TIMER = 0;

    private const float BASE_PLAYER_SPEED = 5.5f;

    private const int BASE_LEVEL = 1;

    private const float PLAYER_BASE_POS_X = 2.22f;
    private const float PLAYER_BASE_POS_Y = -4.05f;
    private const float PLAYER_BASE_POS_Z = 0;

    public static XMLManager Instance;
    public int saveFile = 1;
    private Publisher publisher;
    public SavedData savedData;

    void Awake()
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
        publisher = GetComponent<Publisher>();
        CreateEmptySaveFiles();
        AudioListener.volume = savedData.masterSound;
    }

    private void CreateEmptySaveFiles()
    {
        if (!File.Exists(Application.persistentDataPath + SAVE_FILE_1))
        {
            CreateBaseSave(1);
        }
        if (!File.Exists(Application.persistentDataPath + SAVE_FILE_2))
        {
            CreateBaseSave(2);
        }
        if (!File.Exists(Application.persistentDataPath + SAVE_FILE_3))
        {
            CreateBaseSave(3);
        }
    }

    public void CreateBaseSave(int save)
    {
        if (!File.Exists(Application.persistentDataPath + save + XML))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SavedData));
            FileStream stream = new FileStream(Application.persistentDataPath + save + XML, FileMode.Create);
            serializer.Serialize(stream, savedData);
            stream.Close();
        }
    }

    public void Save(bool switchScenes)
    {
        //Make publishers async await and change coroutine in sceneswitcher
        publisher.CallFetchDataToSave(switchScenes);
        XmlSerializer serializer = new XmlSerializer(typeof(SavedData));
        FileStream stream = new FileStream(Application.persistentDataPath + saveFile + XML, FileMode.Create);
        serializer.Serialize(stream, savedData);
        stream.Close();
    }

    public void Load(bool loadPosition, bool fromMenu, bool playerDead)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SavedData));
        FileStream stream = new FileStream(Application.persistentDataPath + saveFile + XML, FileMode.Open);
        savedData = serializer.Deserialize(stream) as SavedData;
        stream.Close();

        if (!fromMenu)
        {
            StartCoroutine(WaitForTransition(loadPosition, playerDead));
        }
    }

    public void ResetSave()
    {
        savedData.playerDeaths = BASE_PLAYER_DEATH;
        savedData.playerInfo.levelId = BASE_LEVEL;
        savedData.playerInfo.moveSpeed = BASE_PLAYER_SPEED;
        savedData.timer = BASE_TIMER;
        savedData.playerInfo.playerHealth = BASE_PLAYER_HEALTH;
        savedData.playerInfo.playerMaxHealth = BASE_PLAYER_HEALTH;
        savedData.playerInfo.playerPosition = new Vector3(PLAYER_BASE_POS_X, PLAYER_BASE_POS_Y, PLAYER_BASE_POS_Z);

        foreach (Capacity capacity in savedData.playerInfo.listOfCapacities)
        {
            capacity.acquired = false;
        }

        savedData.playerInfo.listOfCapacities[SWORD_ID].cooldown = BASE_SWORD_COOLDOWN;
        savedData.playerInfo.listOfCapacities[SWORD_ID].damage = BASE_SWORD_DAMAGE;
        savedData.playerInfo.listOfCapacities[SWORD_ID].currentAttacksInCombo = BASE_SWORD_COMBO;
        savedData.playerInfo.listOfCapacities[SWORD_ID].range = BASE_SWORD_RANGE;
        savedData.playerInfo.listOfCapacities[SWORD_ID].specialUpgrade = false;

        savedData.playerInfo.listOfCapacities[DASH_ID].range = BASE_DASH_RANGE;
        savedData.playerInfo.listOfCapacities[DASH_ID].speed = BASE_DASH_SPEED;

        savedData.playerInfo.listOfCapacities[FIREBALL_ID].cooldown = BASE_FIREBALL_COOLDOWN;
        savedData.playerInfo.listOfCapacities[FIREBALL_ID].damage = BASE_FIREBALL_DAMAGE;
        savedData.playerInfo.listOfCapacities[FIREBALL_ID].speed = BASE_FIREBALL_SPEED;

        savedData.yetiInfo.dead = false;
        savedData.yetiInfo.isOutOfAscensionZone = false;

        savedData.bossFightInfo.dead = false;

        foreach (Collectible collectible in savedData.collectibleDB.listOfCollectibles)
        {
            collectible.collected = false;
        }

        foreach (Chest chest in savedData.chestsDB.listOfChest)
        {
            chest.closed = true;
        }
        savedData.darkhenBeaten = false;

        XmlSerializer serializer = new XmlSerializer(typeof(SavedData));
        FileStream stream = new FileStream(Application.persistentDataPath + saveFile + XML, FileMode.Create);
        serializer.Serialize(stream, savedData);
        stream.Close();
    }

    public void SetSaveSelected(int save)
    {
        saveFile = save;
    }

    public void DeleteSave(int save)
    {
        saveFile = save;
        foreach(Achievement achievement in savedData.achievementDB.listOfAchievements)
        {
            achievement.achieved = false;
        }
        ResetSave();
    }

    IEnumerator WaitForTransition(bool loadPosition, bool playerDead)
    {    
        if (!GameManager.Instance.newGMLoaded)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(0);
            GameManager.Instance.newGMLoaded = false;
        }
        publisher.CallPushLoadedData(loadPosition, playerDead);
    }
}

[System.Serializable]
public class Achievement
{
    public int id;
    public bool achieved;
}

[System.Serializable]
public class AchievementDatabase
{
    public List<Achievement> listOfAchievements = new List<Achievement>();
}

[System.Serializable]
public class Capacity
{
    public string capacityName;
    public bool acquired;
    public bool specialUpgrade;
    public float cooldown;
    public float damage;
    public int currentAttacksInCombo;
    public float range;
    public float speed;
}

[System.Serializable]
public class PlayerInfo
{
    public int levelId;
    public Vector3 playerPosition;
    public float playerHealth;
    public int playerMaxHealth;
    public List<Capacity> listOfCapacities = new List<Capacity>();
    public float moveSpeed;
}

[System.Serializable]
public class YetiInfo
{
    public bool isOutOfAscensionZone;
    public bool dead;
}

[System.Serializable]
public class BossFightInfo
{
    public bool dead;
}

public class Dialogue
{
    public int dialogueId;
    public bool done;
}

[System.Serializable]
public class DialogueDatabase
{
    public List<Dialogue> listOfDialogues = new List<Dialogue>();
}

[System.Serializable]
public class Collectible
{
    public int id;
    public int levelId;
    public bool collected;
}

[System.Serializable]
public class CollectibleDatabase
{
    public List<Collectible> listOfCollectibles = new List<Collectible>();
}

[System.Serializable]
public class Chest
{
    public int id;
    public int levelId;
    public bool closed;
}

[System.Serializable]
public class ChestsDatabase
{
    public List<Chest> listOfChest = new List<Chest>();
}

[System.Serializable]
public class SavedData
{
    public PlayerInfo playerInfo;
    public YetiInfo yetiInfo;
    public BossFightInfo bossFightInfo;
    public ChestsDatabase chestsDB;
    public AchievementDatabase achievementDB;
    public CollectibleDatabase collectibleDB;
    public int playerDeaths;
    public float timer;
    public float masterSound;
    public bool darkhenBeaten;
}
