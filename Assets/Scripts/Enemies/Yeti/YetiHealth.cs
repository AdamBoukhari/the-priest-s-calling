using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class YetiHealth : MonoBehaviour
{
    [SerializeField] private float maxLife = 30;
    private float life;

    private YetiFightStateManager yetiFightStateManager;
    private LootEnemies loot;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private Slider healthBar;

    private MusicAltern music;
    [SerializeField] private Material flashingMaterial;
    private Material originalMaterial;
    private const float HIT_VISUAL_DURATION = 0.1f;

    private bool isDead = false;
    private float touchDamage = 0.5f;
    private float force = 50000;

    private void Awake()
    {
        yetiFightStateManager = GetComponent<YetiFightStateManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalMaterial = spriteRenderer.material;

        life = maxLife;

        music = GameObject.FindGameObjectWithTag(Harmony.Tags.MainCamera).GetComponent<MusicAltern>();
    }

    private void Start()
    {
        loot = GameManager.Instance.GetComponent<LootEnemies>();
    }

    public void LoseLife(float amount, bool left)
    {
        if (isDead || yetiFightStateManager.currentState == YetiFightStateManager.YetiStateToSwitch.Defense || yetiFightStateManager.currentState == YetiFightStateManager.YetiStateToSwitch.RangeAttack)
            return;

        life -= amount;
        UpdateHealthBar();
        ManageKnockback(left);
        StartCoroutine(FlashSpriteOnDamage());
        
        if (life <= 0)
        {
            music.PlayBossMusic(false);
            loot.DropHeart(gameObject.transform.position);
            DeactivateHealthBar();
            yetiFightStateManager.ChangeYetiState(YetiFightStateManager.YetiStateToSwitch.Death);
            animator.SetTrigger(Harmony.AnimatorParameters.isDead);
            isDead = true;
            AchievementManager.Instance.UnlockYetiAchievement();
        }
    }

    private IEnumerator FlashSpriteOnDamage()
    {
        spriteRenderer.material = flashingMaterial;
        yield return new WaitForSeconds(HIT_VISUAL_DURATION);
        spriteRenderer.material = originalMaterial;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetLife()
    {
        return life;
    }

    private void ManageKnockback(bool left)
    {
        int directionMultiplier = left ? -1 : 1;
        rb.AddForce(new Vector2(directionMultiplier * force, 2 * force), ForceMode2D.Impulse);
    }

    public void ActivateHealthBar()
    {
        healthBar.gameObject.SetActive(true);
        healthBar.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = yetiFightStateManager.BOSS_NAME;
        healthBar.maxValue = maxLife;
        healthBar.value = life;
    }

    private void UpdateHealthBar()
    {
        healthBar.value = life;
    }

    public void DeactivateHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    public void RestoreLife()
    {
        life = maxLife;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player) && !isDead)
        {
            PlayerHealth playerHP = collision.gameObject.GetComponent<PlayerHealth>();
            bool left = transform.position.x < collision.transform.position.x;
            playerHP.TakeDamageKnockBack(touchDamage, !left);
        }
    }
}
