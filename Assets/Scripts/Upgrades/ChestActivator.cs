using UnityEngine;

public class ChestActivator : MonoBehaviour
{
    [SerializeField] public int id;

    private UpgradeTrigger upgradeTrigger;
    private Animator animator;
    private AudioSource audioSource;

    private bool closed = true;

    private void Awake()
    {
        upgradeTrigger = GetComponent<UpgradeTrigger>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadChest;
        Publisher.FetchData += SaveChest;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadChest;
        Publisher.FetchData -= SaveChest;
    }

    public void Interact(GameObject player)
    {
        closed = false;
        animator.SetBool(Harmony.AnimatorParameters.OpenChest, true);
        audioSource.PlayOneShot(Sounds.interactionSnd);

        upgradeTrigger.TriggerUpgradeSelection(player);
    }

    public bool isClosed()
    {
        return closed;
    }

    private void SaveChest(bool switchScene)
    {
        foreach (Chest chest in XMLManager.Instance.savedData.chestsDB.listOfChest)
        {
            if (chest.levelId == SceneSwitcher.Instance.GetActualLevel() && chest.id == id)
            {
                chest.closed = closed;
                break;
            }
        }
    }

    private void LoadChest(bool loadPosition, bool playerDead)
    {
        foreach (Chest chest in XMLManager.Instance.savedData.chestsDB.listOfChest)
        {
            if (chest.levelId == SceneSwitcher.Instance.GetActualLevel() && chest.id == id)
            {
                if (!chest.closed)
                {
                    animator.SetBool(Harmony.AnimatorParameters.ChestOpenedWithSave, true);
                }
                closed = chest.closed;
            }
        }
    }
}
