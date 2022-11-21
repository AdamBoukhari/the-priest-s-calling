using System;
using UnityEngine;
using Harmony;

public class PlayerSwordAttack : MonoBehaviour
{
    private const int SWORD_INDEX = 0;
    private const int MAX_ATTACKS_IN_COMBO = 3;

    [SerializeField] private Transform attackPointLeft;
    [SerializeField] private Transform attackPointRight;
    [SerializeField] private float damage = 1;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private float cooldownIncrement = .33f;
    [SerializeField] private bool swordUnlocked = false;

    private Rigidbody2D rigidBody;
    private Animator anim;
    private PlayerMovement playerMovement;
    private SpriteRenderer sprite;
    private LayerMask enemyLayers;
    private LayerMask yetiLayer;
    private LayerMask boxLayer;
    private LayerMask bagLayer;
    private LayerMask necromancerLayers;
    private AudioSource audioSource;

    private int currentAttacksInCombo = 3;
    private int currentAerialAttacks = 0;
    private int currentAttacks = 0;
    private bool[] attacksQueue;
    private string[] attacksAnimation;
    private bool comboAvailable = true;
    private float cooldownCount = 0;

    //Remove condition if too broken
    private bool stopAerialVelocity = false;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        enemyLayers = Layers.Enemies;
        necromancerLayers = Layers.Necromancer;
        yetiLayer = Layers.Yeti;
        boxLayer = Layers.Box;
        bagLayer = Layers.IgnorePlayer;
        SeedAttackQueue();
        audioSource = GetComponent<AudioSource>();

        anim.SetBool(AnimatorParameters.HasSword, swordUnlocked);
    }

    private void Update()
    {
        if(cooldownCount > 0)
        {
            if(!playerMovement.IsGrounded() && stopAerialVelocity)
            {
                rigidBody.velocity = new Vector2(0, 0);
            }
            cooldownCount -= Time.deltaTime;
        }
        else
        {
            ResetSwordComboState();
            cooldownCount = 0;
        }

        if(currentAttacks != 0)
        {
            GameManager.Instance.UpdateSwordCoolDown(cooldownIncrement * currentAttacks, cooldownCount);
        }
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadSword;
        Publisher.FetchData += SaveSword;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadSword;
        Publisher.FetchData -= SaveSword;
    }

    private void SeedAttackQueue()
    {
        attacksQueue = new bool[MAX_ATTACKS_IN_COMBO];
        attacksAnimation = new string[MAX_ATTACKS_IN_COMBO];
        for (int i = 0; i < MAX_ATTACKS_IN_COMBO; i++)
        {
            attacksQueue[i] = false;
            attacksAnimation[i] = "Slash" + (i + 1);
        }
    }

    private void ResetSwordComboState()
    {
        for (int i = 0; i < MAX_ATTACKS_IN_COMBO; i++)
        {
            if (attacksQueue[i])
            {
                attacksQueue[i] = false;
                currentAttacks = 0;
                anim.SetBool(attacksAnimation[i], false);
            }
        }
    }

    private void PlaySlash()
    {
        audioSource.PlayOneShot(Sounds.sword);
    }

    public void OnSwordSlash()
    {
        if(!swordUnlocked || !comboAvailable || !VerifyIfAnimationIsInGoodState())
        {
            return;
        }
        if (!playerMovement.IsGrounded())
        {
            currentAerialAttacks++;
            if(currentAerialAttacks >= MAX_ATTACKS_IN_COMBO)
            {
                comboAvailable = false;
            }
        }
        QueueNextAttackAnimation();
    }

    //Checks if animation state is coherent.
    private bool VerifyIfAnimationIsInGoodState()
    {
        AnimationClip animationClip = anim.GetCurrentAnimatorClipInfo(0)[0].clip;

        if(!attacksQueue[0] || animationClip.name.Contains("Slash"))
        {
            return true;
        }
        return false;
    }

    //Puts next attack in queue
    private void QueueNextAttackAnimation()
    {
        for (int i = 0; i < MAX_ATTACKS_IN_COMBO; i++)
        {
            if (!attacksQueue[i] && (i + 1) <= currentAttacksInCombo)
            {
                cooldownCount += cooldownIncrement;
                currentAttacks++;
                anim.SetBool(attacksAnimation[i], true);
                attacksQueue[i] = true;
                break;
            }
        }
    }

    //Method called by slashX animation frames
    public void DamageEnnemies()
    {
        Collider2D[] hitEnemies;
        Collider2D[] hitYeti;
        Collider2D[] hitNecromancer;
        Collider2D[] hitBoxes;
        Collider2D[] hitBags;
        Collider2D[] projectiles;
        if (sprite.flipX)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, enemyLayers);
            hitYeti = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, yetiLayer);
            hitBoxes = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, boxLayer);
            hitBags = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, bagLayer);
            projectiles = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, Harmony.Layers.GhostProjectile);
            hitNecromancer = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, necromancerLayers);
        }
        else
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, enemyLayers);
            hitYeti = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, yetiLayer);
            hitBoxes = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, boxLayer);
            hitBags = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, bagLayer);
            projectiles = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, Harmony.Layers.GhostProjectile);
            hitNecromancer = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, necromancerLayers);
        }

        //Refactor to let player float if enemy hit, else let hit fall
        //Aerial stabilisation when fighting, not in parkour.
        foreach (Collider2D enemy in hitEnemies)
        {
            SlimeDamage slimeDamage = enemy.GetComponent<SlimeDamage>();
            if (slimeDamage && slimeDamage.isActiveAndEnabled)
            {
                slimeDamage.TakeDamage();

            }
            DemonHealth demonHealth = enemy.GetComponent<DemonHealth>();
            if (demonHealth && demonHealth.isActiveAndEnabled)
            {
                demonHealth.TakeDamage(damage);
            }

            enemy.GetComponent<LifeManager>()?.LoseLife(damage, transform.position.x > enemy.transform.position.x);
        }
        foreach (Collider2D enemy in hitYeti)
        {
            enemy.GetComponent<YetiHealth>()?.LoseLife(damage, transform.position.x > enemy.transform.position.x);
        }
        foreach (Collider2D enemy in hitNecromancer)
        {
            enemy.GetComponent<LifeManager>()?.LoseLife(damage, transform.position.x > enemy.transform.position.x);
        }

        foreach (Collider2D box in hitBoxes)
        {
            box.GetComponent<PushableObject>()?.Reset();
        }

        foreach(Collider2D bag in hitBags)
        {
            bag.GetComponent<Bag>()?.Fall();
        }

        foreach (Collider2D projectile in projectiles)
        {
            projectile.gameObject.SetActive(false);
        }
    }

    public void UnlockSword()
    {
        swordUnlocked = true;
        GameManager.Instance.ShowAbilityInUI(SWORD_INDEX);
        anim.SetBool(AnimatorParameters.HasSword, true);
    }

    public bool IsSwordUnlocked()
    {
        return swordUnlocked;
    }

    public void UpgradeSwordDamage(float value)
    {
        damage += value;
    }

    public void UlockSwordCombo()
    {
        currentAttacksInCombo = 3;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player just hit the ground
        if (playerMovement.IsOnJumpableSurface())
        {
            currentAerialAttacks = 0;
            comboAvailable = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPointLeft == null || attackPointRight == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);
        Gizmos.DrawWireSphere(attackPointRight.position, attackRange);
    }

    private void SaveSword(bool switchScene)
    {
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[SWORD_INDEX].acquired = swordUnlocked;
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[SWORD_INDEX].damage = damage;
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[SWORD_INDEX].currentAttacksInCombo = currentAttacksInCombo;
    }

    private void LoadSword(bool loadPosition, bool playerDead)
    {
        swordUnlocked = XMLManager.Instance.savedData.playerInfo.listOfCapacities[SWORD_INDEX].acquired;
        damage = XMLManager.Instance.savedData.playerInfo.listOfCapacities[SWORD_INDEX].damage;
        currentAttacksInCombo = XMLManager.Instance.savedData.playerInfo.listOfCapacities[SWORD_INDEX].currentAttacksInCombo;
        anim.SetBool(AnimatorParameters.HasSword, swordUnlocked);
    }
}
