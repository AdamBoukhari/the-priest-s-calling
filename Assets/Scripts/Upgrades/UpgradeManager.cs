using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UpgradeManager : MonoBehaviour
{
    private const int POSSIBLE_UPGRADES = 3;
    private const float SELECTION_COOLDOWN = 0.1f;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator[] cardsAnimators;
    [SerializeField] private Animator[] templateAnimators;
    [SerializeField] private UpgradeCardSelector[] upgradeCardSelectors;
    [SerializeField] private TextMeshProUGUI[] upgradeTitleTexts;
    [SerializeField] private TextMeshProUGUI[] upgradeDescriptionTexts;

    public static UpgradeManager Instance;

    private PlayerUpgrades playerUpgrades;
    private PlayerInteraction playerInteraction;
    private UpgradeCard[] upgradeCards;
    
    private float selectionCooldown = 0f;
    private bool choiceProcessStarted = false;
    private int oldSelectedCardIndex;
    private int newSelectedCardIndex;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        if(selectionCooldown > 0)
        {
            selectionCooldown -= Time.deltaTime;
        }
    }

    public void StartUpgradeChoiceSelection(UpgradeCard[] newUpgradeCards, GameObject player)
    {
        playerUpgrades = player.GetComponent<PlayerUpgrades>();
        playerInteraction = player.GetComponent<PlayerInteraction>();
        upgradeCards = newUpgradeCards;

        // Manage camera here
        playerInteraction.DeactivatePlayerControls();

        StartCoroutine(EnterCards());

        for (int i = 0; i < POSSIBLE_UPGRADES; i++)
        {
            // Puts correct card upgrade title into its slot.
            upgradeTitleTexts[i].text = Upgrades.GetTitleFromCard(upgradeCards[i]);

            // Set card texture on the back of the card.
            upgradeCardSelectors[i].SetTexture(upgradeCards[i].displayedCard);
        }
    }

    private IEnumerator EnterCards()
    {
        yield return new WaitForSeconds(1);

        int[] enterOrder = { 0, 2, 1 };
        foreach (int i in enterOrder)
        {
            templateAnimators[i].SetBool(Harmony.AnimatorParameters.IsEntered, true);
            yield return new WaitForSeconds(0.75f);

            upgradeCardSelectors[i].InvertTemplateArrival();

            // Format text to dynamically put card value into its description.
            string cardText = string.Format(upgradeCards[i].upgradeDescription, upgradeCards[i].upgradeValue);
            upgradeDescriptionTexts[i].text = cardText;
        }

        yield return new WaitForSeconds(1f);
        choiceProcessStarted = true;
        UpdateSelectedCardAnimation();
    }

    public void OnNavigate(InputValue input)
    {
        if (choiceProcessStarted && selectionCooldown <= 0f)
        {
            selectionCooldown = SELECTION_COOLDOWN;

            //Scanning the direction of the input
            float direction = input.Get<Vector2>().x;
            if (direction < 0) newSelectedCardIndex--;
            if (direction > 0) newSelectedCardIndex++;

            //Dealing with array boundaries
            if (newSelectedCardIndex < 0) newSelectedCardIndex = POSSIBLE_UPGRADES - 1;
            if (newSelectedCardIndex > POSSIBLE_UPGRADES - 1) newSelectedCardIndex = 0;

            //Updating selected animation
            UpdateSelectedCardAnimation();
            oldSelectedCardIndex = newSelectedCardIndex;
        }
    }

    private void UpdateSelectedCardAnimation()
    {
        cardsAnimators[oldSelectedCardIndex].SetBool(Harmony.AnimatorParameters.IsHovered, false);
        cardsAnimators[newSelectedCardIndex].SetBool(Harmony.AnimatorParameters.IsHovered, true);
    }

    public void OnSubmit()
    {
        if(choiceProcessStarted)
        {
            playerUpgrades.RedirectUpgradeToPlayerStats(upgradeCards[newSelectedCardIndex]);
            audioSource.PlayOneShot(Sounds.powerUp);
            XMLManager.Instance.Save(false);
            AchievementManager.Instance.UnlockAllChestsAchievements();
            StartCoroutine(EndUpgradeChoiceSelection());
        }
    }

    private IEnumerator EndUpgradeChoiceSelection()
    {
        choiceProcessStarted = false;
        for (int i = 0; i < POSSIBLE_UPGRADES; i++)
        {
            if(i != newSelectedCardIndex)
            {
                DeactivateCard(i);
            }
        }
        yield return new WaitForSeconds(2);

        DeactivateCard(newSelectedCardIndex);

        for (int i = 0; i < POSSIBLE_UPGRADES; i++)
        {
            templateAnimators[i].SetBool(Harmony.AnimatorParameters.IsEntered, false);
        }

        // Manage camera here
        playerInteraction.ActivatePlayerControls();
    }

    private void DeactivateCard(int index)
    {
        cardsAnimators[index].SetBool(Harmony.AnimatorParameters.IsHovered, false);
        upgradeDescriptionTexts[index].text = "";
        upgradeCardSelectors[index].InvertTemplateArrival();
    }
}
