using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlimeDamage : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Material flashingMaterial;
    [SerializeField] private SlimeManager manager;

    private const float MaxSlimeLife = 5f;
    private const float HitVisualDuration = 0.1f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    private DialogueTrigger dialogueTrigger;
    private Material originalMaterial;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        life = MaxSlimeLife;
        originalMaterial = spriteRenderer.material;
    }

    private void OnEnable()
    {
        RestoreLife();
        UpdateHealthBar();
    }

    public void TakeDamage()
    {
        if ((manager.stage2Started && manager.allAddsInactive) || !manager.stage2Started)
        {
            CanTakeDamage();
        }
    }

    private void CanTakeDamage()
    {
        ManageSpriteFlip();
        manager.StopMove();
        ManageHitAnimation();
        StartCoroutine(FlashSpriteOnDamage());

        DamageTaken();
    }

    private void ManageHitAnimation()
    {
        animator.SetBool(Harmony.AnimatorParameters.Slime_move, false);
        animator.SetBool(Harmony.AnimatorParameters.Slime_hit, true);
    }

    private void ManageSpriteFlip()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private void DamageTaken()
    {
        life--;
        UpdateHealthBar();
        StartCoroutine(DeactivateHitAnimation());
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (life <= 0)
        {
            DeactivateHealthBar();
            animator.SetBool(Harmony.AnimatorParameters.Slime_die, true);
        }
        else
        {
            manager.CanMove();
        }
    }

    private IEnumerator DeactivateHitAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool(Harmony.AnimatorParameters.Slime_hit, false);
        spriteRenderer.material = originalMaterial;
    }

    internal void RestoreLife()
    {
        life = MaxSlimeLife;
    }

    private IEnumerator FlashSpriteOnDamage()
    {
        spriteRenderer.material = flashingMaterial;
        yield return new WaitForSeconds(HitVisualDuration);
        spriteRenderer.material = originalMaterial;
    }
    public void ActivateHealthBar()
    {
        healthBar.gameObject.SetActive(true);
        healthBar.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = manager.BOSS_NAME;
        healthBar.maxValue = MaxSlimeLife;
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

    private void IsDead()
    {
        RepulsePlayerBeforeTranformation();

        if (!manager.stage2Started)
            dialogueTrigger.TriggerDialogue(GameObject.FindGameObjectWithTag(Harmony.Tags.Player));
        else
        {
            life = MaxSlimeLife;
            animator.SetBool(Harmony.AnimatorParameters.Slime_transformation, true);
        }

        playerTransform.gameObject.GetComponent<PlayerInteraction>().DeactivatePlayerControls();
    }

    private void RepulsePlayerBeforeTranformation()
    {
        PlayerHealth playerHP = playerTransform.gameObject.GetComponent<PlayerHealth>();
        bool left = transform.position.x < playerTransform.position.x;
        playerHP.TakeDamageKnockBack(0f, !left);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            PlayerHealth playerHP = collision.GetComponent<PlayerHealth>();
            bool left = transform.position.x < collision.transform.position.x;
            playerHP.TakeDamageKnockBack(0.5f, !left);
        }
    }
}
