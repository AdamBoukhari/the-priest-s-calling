using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenuScript : MonoBehaviour
{
    private const string SPEEDRUNNER_DESC = "Finish the game within 20 minutes. Time left: ";
    private const string COLLECTIBLE_DESC = "Collect every collectible ";
    private const string CHEST_DESC = "Open every chest ";
    private const float TIME_ALLOCATED_FOR_ACHIEVEMENT = 1201f;

    [SerializeField] private TextMeshProUGUI collectiblesDescription;
    [SerializeField] private TextMeshProUGUI chestsDescription;
    [SerializeField] private TextMeshProUGUI speedRunnerDescription;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject achievementMenu;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject controllerControlsMenu;
    [SerializeField] private GameObject computerControlsMenu;

    public GameObject settingsButton, settingsFirstButton, scrollBar, achievementButton, computerControlsButton, controllerControlsButton, audioButton, audioFirstSlider;

    private int collectibleCollected = 0;
    private int chestsOpened = 0;
    private string chestCount = "/9";
    private string collectibleCount = "/11";

    private void Update()
    {
        speedRunnerDescription.text = SPEEDRUNNER_DESC + Mathf.FloorToInt((TIME_ALLOCATED_FOR_ACHIEVEMENT - GameManager.Instance.GetTimer()) / 60) + ":" + Mathf.FloorToInt((TIME_ALLOCATED_FOR_ACHIEVEMENT - GameManager.Instance.GetTimer()) % 60);
    }

    public void OnBack()
    {
        if(pauseMenu.activeSelf)
        {
            Resume();
        }
        else
        {
            if (settingsMenu.activeSelf)
            {
                if (audioMenu.activeSelf)
                {
                    ChangeMenu(settingsPanel, audioMenu, audioButton);
                }
                else if (controllerControlsMenu.activeSelf)
                {
                    ChangeMenu(settingsPanel, controllerControlsMenu, controllerControlsButton);
                }
                else if (computerControlsMenu.activeSelf)
                {
                    ChangeMenu(settingsPanel, computerControlsMenu, computerControlsButton);
                }
                else
                {
                    ChangeMenu(pauseMenu, settingsMenu, settingsButton);
                }
            }
            else if (achievementMenu.activeSelf)
            {
                ChangeMenu(pauseMenu, achievementMenu, achievementButton);
            }
        }
    }

    public void LeaveToMainMenu()
    {
        Resume();

        SceneSwitcher.Instance.GoBackToMainMenu(1);
    }

    public void Resume()
    {
        GameManager.Instance.OnPause();
    }

    public void Settings()
    {
        ChangeMenu(settingsMenu, pauseMenu, audioButton);
    }

    public void Audio()
    {
        ChangeMenu(audioMenu, settingsPanel, audioFirstSlider);
    }

    public void ControllerControls()
    {
        ChangeMenu(controllerControlsMenu, settingsPanel, null);
    }

    public void ComputerControls()
    {
        ChangeMenu(computerControlsMenu, settingsPanel, null);
    }

    public void LoadToLastSave()
    {
        Resume();
        GameManager.Instance.StartLoadingTransition();
        XMLManager.Instance.Load(true, false, false);
    }

    public void Achievements()
    {
        ChangeMenu(achievementMenu, pauseMenu, scrollBar);
        SetTextCountForAchievements();
    }

    private void SetTextCountForAchievements()
    {
        chestsOpened = 0;
        collectibleCollected = 0;
        foreach(Chest chest in XMLManager.Instance.savedData.chestsDB.listOfChest)
        {
            if(!chest.closed)
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
    }

    private void ChangeMenu(GameObject menuToSwitchTo, GameObject menuToSwitchFrom, GameObject firstButtonSelected)
    {
        menuToSwitchTo?.SetActive(true);
        menuToSwitchFrom?.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButtonSelected);
    }
}
