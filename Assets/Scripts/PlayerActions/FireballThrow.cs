using System.Collections.Generic;
using UnityEngine;


public class FireballThrow : MonoBehaviour
{
    private const int FIREBALL_INDEX = 2;
    private Vector3 OFFSET = new Vector3(0f, 0.8f, 0f);
    private const float BASE_FIREBALL_CD = 4f;

    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private bool fireballUnlocked = false;

    private SpriteRenderer sprite;
    private Animator anim;
    private AudioSource audioSource;

    private List<GameObject> fireballs = new List<GameObject>();
    private float cooldownCount = 0;
    private bool UIUpdaterCalled = false;
    private int NB_FIREBALLS = 3;
    private float throwCooldown = BASE_FIREBALL_CD;

    private void Awake()
    {
        for (int i = 0; i < NB_FIREBALLS; i++)
        {
            GameObject fireball = Instantiate(fireballPrefab);
            fireball.SetActive(false);
            fireballs.Add(fireball);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadFireball;
        Publisher.FetchData += SaveFireball;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadFireball;
        Publisher.FetchData -= SaveFireball;
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(cooldownCount > 0)
        {
            GameManager.Instance.UpdateFireballCoolDown(throwCooldown, cooldownCount);
            cooldownCount -= Time.deltaTime;
        }
        else
        {
            cooldownCount = 0;

            if(!UIUpdaterCalled)
            {
                GameManager.Instance.UpdateFireballCoolDown(throwCooldown, cooldownCount);
                UIUpdaterCalled = true;
            }
        }
    }

    public void OnFireballThrow()
    {
        if(!fireballUnlocked || cooldownCount > 0)
        {
            return;
        }


        GameObject fireball = GetNextFireball();
        if (fireball != null)
        {
            audioSource.PlayOneShot(Sounds.fireball);
            anim.SetTrigger("Throw");
            UIUpdaterCalled = false;
            cooldownCount = throwCooldown;
            fireball.GetComponent<Fireball>().SetOrientation(sprite.flipX);
            fireball.transform.position = transform.position + OFFSET;
            fireball.SetActive(true);
        }
    }

    public void UnlockFireball()
    {
        fireballUnlocked = true;
        GameManager.Instance.ShowAbilityInUI(FIREBALL_INDEX);
    }

    public bool FireballObtained()
    {
        return fireballUnlocked;
    }

    public void UpgradeFireballsDamage(float value)
    {
        foreach (GameObject fireball in fireballs)
        {
            fireball.GetComponent<Fireball>().UpgradeFireballDamage(value);
        }
    }

    public void UpgradeFireballsSpeed(float value)
    {
        foreach (GameObject fireball in fireballs)
        {
            fireball.GetComponent<Fireball>().UpgradeFireballSpeed(value);
        }
    }

    public void DecreaseFireballCooldown(float value)
    {
        throwCooldown -= (BASE_FIREBALL_CD * (value / 100));
    }

    private GameObject GetNextFireball()
    {
        foreach(GameObject fireball in fireballs)
        {
            if (!fireball.activeInHierarchy)
            {
                return fireball;
            }
        }
        return null;
    }

    private void SaveFireball(bool switchScene)
    {
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[FIREBALL_INDEX].acquired = fireballUnlocked;
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[FIREBALL_INDEX].cooldown = throwCooldown;
    }

    private void LoadFireball(bool loadPosition, bool playerDead)
    {
        fireballUnlocked = XMLManager.Instance.savedData.playerInfo.listOfCapacities[FIREBALL_INDEX].acquired;
        throwCooldown = XMLManager.Instance.savedData.playerInfo.listOfCapacities[FIREBALL_INDEX].cooldown;
    }
}
