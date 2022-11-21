using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string SPEEDRUNNER_DESC = "Finish the game within ";
    private const string COLLECTIBLE_DESC = "Collect every collectible ";
    private const string CHEST_DESC = "Open every chest ";

    private const string PRISON_TITLE = "Devil End's Prison";
    private const string GROTTO_TITLE = "Creepy Grottos";
    private const string VILLAGE_TITLE = "Oakvale Village";
    private const string MOUNTAIN_TITLE = "Scarlet Mountains";
    private const string CASTLE_TITLE = "Evil Castle";

    private const float TIME_ALLOCATED_FOR_ACHIEVEMENT = 1201f;

    [SerializeField] private TextMeshProUGUI speedRunnerDescription;
    [SerializeField] private TextMeshProUGUI chestsDescription;
    [SerializeField] private TextMeshProUGUI collectiblesDescription;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject achievementMenu;
    [SerializeField] private GameObject aboutMenu;
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject controllerControlsMenu;
    [SerializeField] private GameObject computerControlsMenu;
    [SerializeField] private TextMeshProUGUI saveNumber, zoneText;
    [SerializeField] private Animator bookAnimator;
    [SerializeField] private Image zoneImage;
    [SerializeField] private Sprite[] zoneImages;
    string[] zoneTexts = { PRISON_TITLE, GROTTO_TITLE, VILLAGE_TITLE, MOUNTAIN_TITLE, CASTLE_TITLE };

    private AudioSource audioSource;

    public GameObject playFirstButton, playButton, achievementButton, scrollBar, saveFirstButton, aboutButton, settingsFirstButton, settingsButton, computerControlsButton, controllerControlsButton, audioButton, audioFirstSlider, quitButton;

    private int collectibleCollected = 0;
    private int chestsOpened = 0;

    private string chestCount = " / 9";
    private string collectibleCount = " / 11";

    private void Start()
    {
        Cursor.visible = false;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySelect()
    {
        audioSource.PlayOneShot(Sounds.select);
    }

    public void OnBack()
    {
        if (settingsMenu.activeSelf)
        {
            if (audioMenu.activeSelf)
            {
                audioSource.PlayOneShot(Sounds.cancel);
                ChangeMenu(settingsPanel, audioMenu, audioButton, true);
            }
            else if (controllerControlsMenu.activeSelf)
            {
                audioSource.PlayOneShot(Sounds.cancel);
                ChangeMenu(settingsPanel, controllerControlsMenu, controllerControlsButton, true);
            }
            else if (computerControlsMenu.activeSelf)
            {
                audioSource.PlayOneShot(Sounds.cancel);
                ChangeMenu(settingsPanel, computerControlsMenu, computerControlsButton, true);
            }
            else
            {
                audioSource.PlayOneShot(Sounds.cancel);
                ChangeMenu(mainMenu, settingsMenu, settingsButton, true);
            }
        }
        else if (playMenu.activeSelf)
        {
            audioSource.PlayOneShot(Sounds.cancel);
            ChangeMenu(mainMenu, playMenu, playButton, true);
        }
        else if (saveMenu.activeSelf)
        {
            audioSource.PlayOneShot(Sounds.cancel);
            ChangeMenu(playMenu, saveMenu, playFirstButton, true);
        }
        else if (achievementMenu.activeSelf)
        {
            audioSource.PlayOneShot(Sounds.cancel);
            ChangeMenu(saveMenu, achievementMenu, achievementButton, true);
        }
        else if (aboutMenu.activeSelf)
        {
            audioSource.PlayOneShot(Sounds.cancel);
            ChangeMenu(mainMenu, aboutMenu, aboutButton, true);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(quitButton);
        }
    }

    public void Play()
    {
        ChangeMenu(playMenu, mainMenu, playFirstButton, false);
    }

    public void StartSave()
    {
        if (File.Exists(Application.persistentDataPath + XMLManager.Instance.saveFile + ".xml"))
        {
            XMLManager.Instance.Load(true, true, false);
            SceneSwitcher.Instance.LoadIntoSpecificLevel(XMLManager.Instance.savedData.playerInfo.levelId, false);
        }
        else
        {
            SceneSwitcher.Instance.LoadIntoSpecificLevel();
        }
    }
    public void OpenSave(int save)
    {
        XMLManager.Instance.SetSaveSelected(save);
        if (File.Exists(Application.persistentDataPath + save + ".xml"))
        {
            XMLManager.Instance.Load(true, true, false);
        }
        ChangeMenu(saveMenu, playMenu, saveFirstButton, false);
        saveNumber.text = "Save " + save;
        zoneText.text = GetZoneNameWithLevel();
        zoneImage.sprite = GetZoneSpriteWithLevel();
    }

    private string GetZoneNameWithLevel()
    {
        int level = XMLManager.Instance.savedData.playerInfo.levelId;
        if(level == 0)
        {
            return zoneTexts[level];
        }
        else
        {
            return zoneTexts[level - 1];
        }
    }

    private Sprite GetZoneSpriteWithLevel()
    {
        int level = XMLManager.Instance.savedData.playerInfo.levelId;
        if (level == 0)
        {
            return zoneImages[level];
        }
        else
        {
            return zoneImages[level - 1];
        }
    }

    public void OpenAchievementMenu()
    {
        ChangeMenu(achievementMenu, saveMenu, scrollBar, false);
        SetTextCountChestAndCollecible();
    }

    public void About()
    {
        ChangeMenu(aboutMenu, mainMenu, null, false);
    }

    //Methodes pour le menu Settings
    public void Settings()
    {
        ChangeMenu(settingsMenu, mainMenu, settingsFirstButton, false);
    }

    public void Audio()
    {
        ChangeMenu(audioMenu, settingsPanel, audioFirstSlider, false);
    }

    public void ControllerControls()
    {
        ChangeMenu(controllerControlsMenu, settingsPanel, null, false);
    }

    public void ComputerControls()
    {
        ChangeMenu(computerControlsMenu, settingsPanel, null, false);
    }

    public void DeleteSave(int save)
    {
        if (File.Exists(Application.persistentDataPath + save + ".xml"))
        {
            XMLManager.Instance.DeleteSave(save);
            XMLManager.Instance.CreateBaseSave(save);
        }
    }
    private void SetTextCountChestAndCollecible()
    {
        collectibleCollected = 0;
        chestsOpened = 0;
        foreach (Chest chest in XMLManager.Instance.savedData.chestsDB.listOfChest)
        {
            if (!chest.closed)
            {
                chestsOpened++;
            }
        }
        foreach (Collectible collectible in XMLManager.Instance.savedData.collectibleDB.listOfCollectibles)
        {
            if (collectible.collected)
            {
                collectibleCollected++;
            }
        }
        collectiblesDescription.text = COLLECTIBLE_DESC + collectibleCollected + collectibleCount;
        chestsDescription.text = CHEST_DESC + chestsOpened + chestCount;
        speedRunnerDescription.text = SPEEDRUNNER_DESC + Mathf.FloorToInt((TIME_ALLOCATED_FOR_ACHIEVEMENT - XMLManager.Instance.savedData.timer) / 60) + ":" + Mathf.FloorToInt((TIME_ALLOCATED_FOR_ACHIEVEMENT - XMLManager.Instance.savedData.timer) % 60);
    }

    public void ResetSave()
    {
        XMLManager.Instance.ResetSave();
        zoneText.text = GetZoneNameWithLevel();
        zoneImage.sprite = GetZoneSpriteWithLevel();
    }

    private void ChangeMenu(GameObject menuToSwitchTo, GameObject menuToSwitchFrom, GameObject firstButtonSelected, bool turnRight)
    {
        menuToSwitchFrom?.SetActive(false);
        
        if (turnRight)
        {
            bookAnimator.SetTrigger("TurnRight");  
        }
        else
        {
            bookAnimator.SetTrigger("TurnLeft");
        }
        StartCoroutine(WaitForPage(menuToSwitchTo, firstButtonSelected));
    }
    IEnumerator WaitForPage(GameObject menuToSwitchTo, GameObject firstButtonSelected)
    {
        yield return new WaitForSeconds(0.5f);
        menuToSwitchTo?.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButtonSelected);
    }

    //Methode pour le bouton QUIT
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
