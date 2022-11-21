using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private const float INVINCIBILTY_TIMER_MAX = 1.5f;
    private const float SPRITE_FLASHING_RATE = 0.15f;
    private const int HEART_LIMIT = 8;
    private const int INITIAL_HEARTS = 3;
    private const float HEART_REGEN_VALUE = 1;
    private const float LIFESTEAL_REGEN_VALUE = 0.5f;
    private const float KNOCKBACK_TIMER_MAX = 0.5f;
    private const float BASE_KB_SPEED = 3f;
    private const float BASE_KB_VFORCE = 10f;
    private const float YETI_KB_SPEED = 15f;
    private const float YETI_KB_VFORCE = 8f;
    private const int SWORD_INDEX = 0;

    [SerializeField] private Material flashingMaterial;
    [SerializeField] public float currentHearts = INITIAL_HEARTS;
    [SerializeField] private int currentMaxHearts = INITIAL_HEARTS;

    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private Material originalMaterial;
    private Animator animator;
    private AudioSource audioSource;

    private float knockbackSpeedToApply;
    private float knockbackVForceToApply;
    private float invincibilityTimer = 0;
    private float spriteFlashTimer = 0;
    private bool flashingActivated = false;
    private bool playerStopped = false;
    private float knockbackTimer;
    private float knockbackSpeed;
    private bool canCollideWithEnnemies = true;
    private bool lifeStealUnlocked = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        originalMaterial = playerSprite.material;
        animator = GetComponent<Animator>();

        knockbackSpeedToApply = BASE_KB_SPEED;
        knockbackVForceToApply = BASE_KB_VFORCE;

        GameManager.Instance.UpdateHealthUI(currentMaxHearts, currentHearts);
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Publisher.PushData += LoadHealth;
        Publisher.FetchData += SaveHealth;
        Publisher.EnemyDied += RegenThroughLifesteal;
    }

    private void OnDisable()
    {
        Publisher.PushData -= LoadHealth;
        Publisher.FetchData -= SaveHealth;
        Publisher.EnemyDied -= RegenThroughLifesteal;
    }

    private void Update()
    {
        if (knockbackTimer > 0)
        {
            rb.velocity = new Vector2(knockbackSpeed, rb.velocity.y);
            knockbackTimer -= Time.deltaTime;
        }
        FlashSpriteOnInvicibility();
    }

    private void RegenThroughLifesteal()
    {
        if (lifeStealUnlocked)
        {
            AddHeart(LIFESTEAL_REGEN_VALUE);
        }
    }

    private void FlashSpriteOnInvicibility()
    {
        if (invincibilityTimer <= 0)
        {
            ActivateCollisionsWithDamageable();
            playerSprite.material = originalMaterial;
            return;
        }

        invincibilityTimer -= Time.deltaTime;
        spriteFlashTimer -= Time.deltaTime;

        if (spriteFlashTimer <= 0)
        {
            playerSprite.material = flashingActivated ? originalMaterial : flashingMaterial;
            flashingActivated = !flashingActivated;
            spriteFlashTimer = SPRITE_FLASHING_RATE;
        }
    }

    public void StartInvincibilityTimer()
    {
        invincibilityTimer = INVINCIBILTY_TIMER_MAX;
        spriteFlashTimer = SPRITE_FLASHING_RATE;
        DeactivateCollisionsWithDamageable();
    }

    public void AddHeart(float amount)
    {
        if(currentHearts < currentMaxHearts)
        {
            currentHearts += amount;
            audioSource.PlayOneShot(Sounds.healSnd);
            if (currentHearts > currentMaxHearts)
            {
                currentHearts = currentMaxHearts;
            }
        }
        GameManager.Instance.UpdateHealthUI(currentMaxHearts, currentHearts);
    }

    public void ResetHealth()
    {
        currentHearts = currentMaxHearts;
        GameManager.Instance.UpdateHealthUI(currentMaxHearts, currentHearts);
    }

    public void AddMaxHeart(float value)
    {
        int addedHearts = (int)value;

        if (currentMaxHearts + addedHearts <= HEART_LIMIT)
        {
            currentMaxHearts += (int)value;
        }
        else
        {
            currentMaxHearts += HEART_LIMIT - currentMaxHearts;
        }

        currentHearts = currentMaxHearts;
        GameManager.Instance.AddMaxHeartUI(currentMaxHearts, currentHearts);
    }

    private void TakeDamage(float damage)
    {
        if(playerStopped)
        {
            return;
        }

        currentHearts -= damage;
        GameManager.Instance.UpdateHealthUI(currentMaxHearts, currentHearts);

        if (currentHearts <= 0)
        {
            animator.SetTrigger(Harmony.AnimatorParameters.Died);
            GameManager.Instance.PlayerDead();
            audioSource.PlayOneShot(Sounds.deathPlayerSnd);
        }
        else
        {
            audioSource.PlayOneShot(Sounds.hitPlayerSnd);
            StartInvincibilityTimer();
        }
    }

    public void TakePlainDamage(float damage)
    {
        if(invincibilityTimer <= 0 && currentHearts > 0 && !playerStopped)
        {
            TakeDamage(damage);
        }
    }

    public void TakeDamageKnockBack(float damage, bool left)
    {
        if (invincibilityTimer > 0 || currentHearts <= 0 || playerStopped)
        {
            return;
        }

        if(damage > 0)
        {
            TakeDamage(damage);
        }

        int directionMultiplier = left ? -1 : 1;
        Vector2 direction = new Vector2(directionMultiplier * knockbackSpeedToApply, knockbackVForceToApply);
        rb.AddForce(direction, ForceMode2D.Impulse);
        knockbackSpeed = directionMultiplier * knockbackSpeedToApply;
        knockbackTimer = KNOCKBACK_TIMER_MAX;

        knockbackSpeedToApply = BASE_KB_SPEED;
        knockbackVForceToApply = BASE_KB_VFORCE;
    }

    public void HitByYetiAttack(float damage, bool left)
    {
        knockbackSpeedToApply = YETI_KB_SPEED;
        knockbackVForceToApply = YETI_KB_VFORCE;
        TakeDamageKnockBack(damage, left);
    }

    private void DeactivateCollisionsWithDamageable()
    {
        canCollideWithEnnemies = false;
        ManageCollisions();
    }

    private void ActivateCollisionsWithDamageable()
    {
        canCollideWithEnnemies = true;
        ManageCollisions();
    }

    private void ManageCollisions()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("GhostProjectile"), !canCollideWithEnnemies);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Snowball"), !canCollideWithEnnemies);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Yeti"), !canCollideWithEnnemies);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), !canCollideWithEnnemies);
    }

    public void UnlockLifeSteal()
    {
        lifeStealUnlocked = true;
    }

    public bool LifestealUnlocked()
    {
        return lifeStealUnlocked;
    }

    public void SetPlayerStopped(bool stopped)
    {
        playerStopped = stopped;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Heart))
        {
            AddHeart(HEART_REGEN_VALUE);
            collision.gameObject.SetActive(false);
        }
    }

    private void LoadHealth(bool loadPosition, bool playerDead)
    {
        currentHearts = XMLManager.Instance.savedData.playerInfo.playerHealth;
        currentMaxHearts = XMLManager.Instance.savedData.playerInfo.playerMaxHealth;
        lifeStealUnlocked = XMLManager.Instance.savedData.playerInfo.listOfCapacities[SWORD_INDEX].specialUpgrade;
        GameManager.Instance.UpdateHealthUI(currentMaxHearts, currentHearts);
    }

    private void SaveHealth(bool switchScene)
    {
        XMLManager.Instance.savedData.playerInfo.playerHealth = currentHearts;
        XMLManager.Instance.savedData.playerInfo.playerMaxHealth = currentMaxHearts;
        XMLManager.Instance.savedData.playerInfo.listOfCapacities[SWORD_INDEX].specialUpgrade = lifeStealUnlocked;
    }
}
