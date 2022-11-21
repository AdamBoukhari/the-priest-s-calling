using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Harmony;

public class DialogueManager : MonoBehaviour
{
    private const float TEXT_INCREMENT_TIMER = 0.05f;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject abilityBox;
    [SerializeField] private TextMeshProUGUI abilityNameText;
    [SerializeField] private Text abilityDescriptionText;
    [SerializeField] private GameObject coolSword;
    [SerializeField] private GameObject dashRing;
    [SerializeField] private GameObject hermesBoots;
    [SerializeField] private GameObject fireballRing;
    [SerializeField] private GameObject lifesteal;
    [SerializeField] private Animator transition;
    [SerializeField] private GameObject endGameTrigger = null;

    private MusicAltern music;
    private AudioSource audioSource;
    private PlayerInteraction playerInteraction;
    private PlayerUpgrades playerUpgrades;
    private DialogueEvent dialogueEvent;

    private Queue<DialogueEvent> dialogueSentences;
    private List<GameObject> interactableObjects;
    private bool dialogueProcessStarted = false;
    private bool sentenceEnded = true;
    private bool isActing;
    private bool endGame = false;
    private bool isAbility = false;

    public static DialogueManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        // Ceating the queue for dialogue sentences - First In First Out
        dialogueSentences = new Queue<DialogueEvent>();

        music = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<MusicAltern>();
        abilityBox.SetActive(false);
    }

    public void StartDialogue(DialogueEvent[] dialogue, GameObject player, List<GameObject> interectableObjects)
    {
        playerUpgrades = player.GetComponent<PlayerUpgrades>();
        playerInteraction = player.GetComponent<PlayerInteraction>();
        dialogueProcessStarted = true;

        // Manage camera here
        playerInteraction.DeactivatePlayerControls();

        animator.SetBool(AnimatorParameters.IsOpen, true);
        dialogueSentences.Clear();

        interactableObjects = interectableObjects;

        foreach (DialogueEvent dialogueFrame in dialogue)
        {
            // Puts every sentence into the queue
            dialogueSentences.Enqueue(dialogueFrame);
        }

        DisplayNextSentence();
    }

    public void OnSubmit()
    {
        if(dialogueProcessStarted && !isActing)
        {
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        // End of the entire dialogue
        if (dialogueSentences.Count == 0 && sentenceEnded)
        {
            EndDialogue();
            return;
        }

        // Sentence is not ended
        if (!sentenceEnded)
        {
            sentenceEnded = true;
            StopAllCoroutines();
            if (dialogueEvent is DialogueFrame)
            {
                DialogueFrame dialogueFrame = (DialogueFrame)dialogueEvent;
                dialogueText.text = dialogueFrame.sentence;
                abilityDescriptionText.text = dialogueFrame.sentence;
            }
            else
            {
                // Skip next dialogue because it puts an empty dialogue box
                OnSubmit();
            }
            return;
        }
        
        // Sentence is ended
        sentenceEnded = false;
        StopAllCoroutines();
        dialogueEvent = dialogueSentences.Dequeue();

        switch (dialogueEvent)
        {
            case DialogueFrame dialogueFrame:
                ManageDialogueFrame(dialogueFrame);
                break;

            case DialogueMove dialogueAction:
                ManageDialogueMove(dialogueAction);
                break;

            case DialogueWait dialogueWait:
                ManageDialogueWait(dialogueWait);
                break;

            case DialogueSound dialogueSound:
                ManageDialogueSound(dialogueSound);
                break;

            case DialogueMusic dialogueMusic:
                ManageDialogueMusic(dialogueMusic);
                break;

            case DialogueEndGame:
                endGame = true;
                break;

            case DialogueSlimeBeaten:
                ManageDialogueSlimeBeaten();
                break;

            case DialogueBoss:
                ManageDialogueBoss();
                break;
        }
    }

    private void ManageDialogueFrame(DialogueFrame dialogueFrame)
    {
        if (dialogueFrame.isAbility)
        {
            ActivateAbilitDialogue(dialogueFrame);
        }
        else
        {
            ActivateNormalDialogue(dialogueFrame);
        }
    }

    private void ActivateAbilitDialogue(DialogueFrame dialogueFrame)
    {
        isAbility = true;
        abilityBox.SetActive(true);
        animator.SetBool(AnimatorParameters.IsOpen, false);
        abilityNameText.text = dialogueFrame.name;

        ActivateItemAnimation(dialogueFrame.name);

        playerUpgrades.UnlockAbility(dialogueFrame.name);

        TypeSentence(dialogueFrame.sentence, isAbility);
    }

    private void ActivateNormalDialogue(DialogueFrame dialogueFrame)
    {
        abilityBox.SetActive(false);
        animator.SetBool(AnimatorParameters.IsOpen, true);
        characterNameText.text = dialogueFrame.name;
        characterImage.sprite = dialogueFrame.image;
        TypeSentence(dialogueFrame.sentence, false);
    }

    private void ManageDialogueMove(DialogueMove dialogueAction)
    {
        animator.SetBool(AnimatorParameters.IsOpen, false);
        StartCoroutine(MoveToDestinationCoroutine(interactableObjects[dialogueAction.targetIndex], dialogueAction.moveTarget, dialogueAction.movespeed));
    }

    private void ManageDialogueWait(DialogueWait dialogueWait)
    {
        animator.SetBool(AnimatorParameters.IsOpen, false);
        StartCoroutine(wait(dialogueWait.time));
    }

    private void ManageDialogueSound(DialogueSound dialogueSound)
    {
        audioSource?.PlayOneShot(dialogueSound.audio);
        SkipAction();
    }

    private void ManageDialogueMusic(DialogueMusic dialogueMusic)
    {
        music.fadeMusic(dialogueMusic.boss, dialogueMusic.volume, dialogueMusic.play, dialogueMusic.stop);
        SkipAction();
    }

    private void ManageDialogueSlimeBeaten()
    {
        XMLManager.Instance.Save(true);
        GameManager.Instance.StopPlayerMovementAfterDialogueEndGame();
        transition.SetTrigger(AnimatorParameters.Transition);
        StartCoroutine(WaitForTransition());
    }

    private void ManageDialogueBoss()
    {
        animator.SetBool(AnimatorParameters.IsOpen, false);
        interactableObjects[0].GetComponent<Animator>().SetBool(AnimatorParameters.Slime_transformation, true);
        OnSubmit();
    }

    private void SkipAction()
    {
        // On Submit is called twice to automatically skip this event
        OnSubmit();
        OnSubmit();
    }

    IEnumerator WaitForTransition()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.ActivatePlayerMovementAfterButtonClick();
        SceneSwitcher.Instance.LoadIntoVillageEndGame();
    }

    private void TypeSentence (string sentence, bool isAbility)
    {
        if (isAbility)
        {
            abilityDescriptionText.text = "";
            StartCoroutine(TypeLetter(sentence, abilityDescriptionText));
        }
        else
        {
            dialogueText.text = "";
            StartCoroutine(TypeLetter(sentence, dialogueText));
        }
    }

    private IEnumerator TypeLetter(string text, Text textBox)
    {
        int letterIncrement = 0;

        foreach (char letter in text.ToCharArray())
        {
            if(letterIncrement == 2)
            {
                letterIncrement = 0;
                PlayRandomDialogueSound();
            }

            textBox.text += letter;
            yield return new WaitForSeconds(TEXT_INCREMENT_TIMER);
            letterIncrement++;
        }
        sentenceEnded = true;
    }

    private void PlayRandomDialogueSound()
    {
        AudioClip clip = null;
        int rand = Random.Range(0, 3);
        if (rand == 0) clip = Sounds.dialogue1;
        if (rand == 1) clip = Sounds.dialogue2;
        if (rand == 2) clip = Sounds.dialogue3;

        audioSource.PlayOneShot(clip);
    }

    private void EndDialogue()
    {
        playerInteraction.ActivatePlayerControls();

        dialogueProcessStarted = false;
        animator.SetBool(AnimatorParameters.IsOpen, false);

        // Only saves if the dialogue gave an ability
        if (isAbility)
        {
            XMLManager.Instance.Save(false);
            isAbility = false;
        } 
 
        // Deactivate all animations
        coolSword.SetActive(false);
        dashRing.SetActive(false);
        hermesBoots.SetActive(false);
        fireballRing.SetActive(false);
        lifesteal.SetActive(false);

        if (endGame)
        {
            GameManager.Instance.StartEndGameTransition();
            endGame = false;
            XMLManager.Instance.savedData.darkhenBeaten = true;
            endGameTrigger?.SetActive(false);
        }

        StartCoroutine(CleanUpDialogueBox());
    }

    private IEnumerator MoveToDestinationCoroutine(GameObject gameObject, Vector2 targetPosition, float speed)
    {
        isActing = true;
        // while this object is not at the destination
        while (gameObject.transform.position.x != targetPosition.x|| gameObject.transform.position.y != targetPosition.y)
        {
            // move it towards the destination, never moving farther than "moveSpeed" in one frame.
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPosition, speed*Time.deltaTime*100 );

            // wait until next frame to continue
            yield return null;
        }
        isActing = false;
        OnSubmit();
    }

    private IEnumerator wait(float time)
    {
        isActing = true;
        yield return new WaitForSeconds(time);
        isActing = false;
        OnSubmit();
    }

    private IEnumerator CleanUpDialogueBox()
    {
        yield return new WaitForSeconds(1);

        characterNameText.text = "";
        dialogueText.text = "";
        characterImage.sprite = null;
    }

    private void ActivateItemAnimation(string itemName)
    {
        coolSword.SetActive(false);
        dashRing.SetActive(false);
        fireballRing.SetActive(false);
        hermesBoots.SetActive(false);
        lifesteal.SetActive(false);

        audioSource?.PlayOneShot(Sounds.upgrade);

        switch (itemName)
        {
            case Consts.ABILITY_SWORD:
                coolSword.SetActive(true);
                break;
            case Consts.ABILITY_LIFESTEAL:
                lifesteal.SetActive(true);
                break;
            case Consts.ABILITY_DASH:
                dashRing.SetActive(true);
                break;
            case Consts.ABILITY_FIREBALL:
                fireballRing.SetActive(true);
                break;
            case Consts.ABILITY_DOUBLE_JUMP:
                hermesBoots.SetActive(true);
                break;
        }
    }
}