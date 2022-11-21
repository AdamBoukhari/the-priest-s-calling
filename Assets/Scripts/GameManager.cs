using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const float MAX_TIME_TO_GET_ACHIEVEMENT = 1200;

    public GameObject resumeButton;
    private GameObject player;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private bool transitionFinished = true;
    [HideInInspector] public bool newGMLoaded;

    private Canvas transitionCanvas;
    private UIHealthUpdater uiHealthUpdater;
    private UICooldownUpdater uiCooldownUpdater;
    private GameObject playerHUD;
    private PlayerInteraction playerInteraction;
    private GameObject endGameCanvas;

    public static GameManager Instance;

    private float timer = 1f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        Cursor.visible = false;
        playerHUD = GameObject.FindGameObjectWithTag(Harmony.Tags.PlayerHUD);
        player = GameObject.FindGameObjectWithTag(Harmony.Tags.Player);
        transitionCanvas = GameObject.FindGameObjectWithTag(Harmony.Tags.TransitionCanvas).GetComponent<Canvas>();

        playerInteraction = player.GetComponent<PlayerInteraction>();
        uiHealthUpdater = playerHUD.GetComponent<UIHealthUpdater>();
        uiCooldownUpdater = playerHUD.GetComponent<UICooldownUpdater>();

        newGMLoaded = false;
        endGameCanvas = GameObject.FindGameObjectWithTag(Harmony.Tags.EndGameCanvas);
    }

    private void OnEnable()
    {
        Publisher.FetchData += SavePlayerPositionAndTimer;
        Publisher.PushData += LoadPlayerPositionAndTimer;
    }

    private void OnDisable()
    {
        Publisher.FetchData -= SavePlayerPositionAndTimer;
        Publisher.PushData -= LoadPlayerPositionAndTimer;
    }

    private void Start()
    {
        newGMLoaded = true;
        timer = XMLManager.Instance.savedData.timer;

        if(SceneSwitcher.Instance.GetActualLevel() != SceneManager.GetActiveScene().buildIndex)
        {
            XMLManager.Instance.Save(false);
        }
        XMLManager.Instance.Load(true, false, false);

        if (endGameCanvas)
        {
            endGameCanvas.SetActive(false);
        }
        if (SceneSwitcher.Instance.GetActualLevel() == 3 && XMLManager.Instance.savedData.darkhenBeaten)
        {
            
        }
        AudioListener.volume = XMLManager.Instance.savedData.masterSound;
    }

    private void Update()
    {
        if (timer + Time.deltaTime <= MAX_TIME_TO_GET_ACHIEVEMENT)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = MAX_TIME_TO_GET_ACHIEVEMENT;
        }
    }

    public void UpdateHealthUI(int currentMaxHearts, float currentHearts)
    {
        uiHealthUpdater.UpdateHealthUI(currentMaxHearts, currentHearts);
    }

    public void AddMaxHeartUI(int currentMaxHearts, float currentHearts)
    {
        uiHealthUpdater.AddMaxHeartUI(currentMaxHearts, currentHearts);
    }

    public void UpdateSwordCoolDown(float maxCooldown, float currentCooldown)
    {
        uiCooldownUpdater.UpdateSwordCoolDown(maxCooldown, currentCooldown);
    }

    public void UpdateDashCoolDown(bool dashAvailable)
    {
        uiCooldownUpdater.UpdateDashCoolDown(dashAvailable);
    }

    public void UpdateFireballCoolDown(float maxCooldown, float currentCooldown)
    {
        uiCooldownUpdater.UpdateFireballCoolDown(maxCooldown, currentCooldown);
    }

    public void OnPause()
    {
        if (pauseCanvas.activeSelf && pauseMenu.activeSelf)
        {
            playerInteraction.ActivatePlayerControls();
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
        }
        else if(transitionFinished && !playerInteraction.PlayerStopped())
        {
            playerInteraction.DeactivatePlayerControls();
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
    }

    public void StartLoadingTransition()
    {
        transitionFinished = false;
        playerInteraction.DeactivatePlayerControls();
        StartCoroutine(WaitForTransitionToPause());
        transitionCanvas.GetComponent<Transition>().LoadTransition();
    }

    public void StartDeathTransition()
    {
        transitionFinished = false;
        playerInteraction.DeactivatePlayerControls();
        transitionCanvas.GetComponent<Transition>().DeathTransition();
        StartCoroutine(WaitForTransitionToPause());
    }

    IEnumerator WaitForTransitionToPause()
    {
        yield return new WaitForSeconds(3f);
        transitionFinished = true;
        playerInteraction.ActivatePlayerControls();
    }

    private void SavePlayerPositionAndTimer(bool switchScene)
    {
        if(!switchScene)
        {
            XMLManager.Instance.savedData.playerInfo.playerPosition = player.transform.position;
        }
        XMLManager.Instance.savedData.timer = timer;
    }

    private void LoadPlayerPositionAndTimer(bool loadPosition, bool playerDead)
    {
        if(loadPosition)
        {
            player.transform.position = XMLManager.Instance.savedData.playerInfo.playerPosition;
        }
        if(!playerDead)
        {
            timer = XMLManager.Instance.savedData.timer;
        }
    }
    public void ShowAbilityInUI(int index)
    {
        uiCooldownUpdater.ShowAbilityCooldown(index);
    }

    public void PlayerDead()
    {
        XMLManager.Instance.Load(true, false, true);
        XMLManager.Instance.savedData.playerDeaths++;
        StartCoroutine(WaitForSaveDeath());
        StartDeathTransition();
        StartCoroutine(ResetPlayerHealth());
    }

    IEnumerator ResetPlayerHealth()
    {
        yield return new WaitForSeconds(3f);
        transitionFinished = true;
        playerInteraction.ActivatePlayerControls();
        player.GetComponent<PlayerHealth>().ResetHealth();
    }

    public float GetTimer()
    {
        return timer;
    }

    internal void ManageEndGame()
    {
        playerInteraction.DeactivatePlayerControls();
        Time.timeScale = 0;
        endGameCanvas.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void StopPlayerMovementAfterDialogueEndGame()
    {
        playerInteraction.DeactivatePlayerControls();
    }

    public void ActivatePlayerMovementAfterButtonClick()
    {
        playerInteraction.ActivatePlayerControls();
    }

    public void StartEndGameTransition()
    {
        transitionCanvas.GetComponent<Transition>().EndGameTransition();
    }
    IEnumerator WaitForSaveDeath()
    {
        yield return new WaitForSeconds(2f);
        XMLManager.Instance.Save(false);
    }
}
