using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemonHealth : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Material flashingMaterial;
    [SerializeField] private DemonManager demonManager;
    [SerializeField] private float life;

    private const float MAX_DEMON_LIFE = 35f;
    private const float HIT_VISUAL_DURATION = 0.1f;

    private SlimeManager slimeManager;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private MusicAltern music;
    private Material originalMaterial;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private const float Force = 300f;
    private float playerDamageAmount;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag(Harmony.Tags.Player).GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        slimeManager = GetComponent<SlimeManager>();
        music = GameObject.FindGameObjectWithTag(Harmony.Tags.MainCamera).GetComponent<MusicAltern>();
        originalMaterial = spriteRenderer.material;
    }
    private void OnEnable()
    {
        RestoreLife();
        UpdateHealthBar();
        ActivateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        ManageHitAnimation();
        ManageSpriteFlip();
        playerDamageAmount = damage;
        GetComponent<AudioSource>().PlayOneShot(Sounds.hitSnd);
        DemonDamageTaken();
    }

    private void ManageHitAnimation()
    {
        animatorinfo = this.animator.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        if (current_animation == Harmony.AnimatorStates.Demon_idle || current_animation == Harmony.AnimatorStates.Demon_move)
        {
            animator.SetBool(Harmony.AnimatorParameters.Demon_hit, true);
        }
        StartCoroutine(FlashSpriteOnDamage());
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

    private IEnumerator FlashSpriteOnDamage()
    {
        spriteRenderer.material = flashingMaterial;
        yield return new WaitForSeconds(HIT_VISUAL_DURATION);
        spriteRenderer.material = originalMaterial;
    }
    private void DemonDamageTaken()
    {
        life -= playerDamageAmount;
        StartCoroutine(DeactivateHitAnimation());
        UpdateHealthBar();
        GetKnockBack();
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (life <= 0)
        {
            DeactivateHealthBar();
            animator.SetBool(Harmony.AnimatorParameters.Demon_die, true);
            DemonIsDead();
        }
    }

    private void GetKnockBack()
    {
        bool left = false;
        if (playerTransform.position.x > transform.position.x)
        {
            left = true;
        }
        ManageKnockback(left);
    }

    private IEnumerator DeactivateHitAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool(Harmony.AnimatorParameters.Demon_hit, false);

    }
    private IEnumerator DeactivateDeadAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool(Harmony.AnimatorParameters.Demon_die, false);
    }
    internal void RestoreLife()
    {
        life = MAX_DEMON_LIFE;
    }
    private IEnumerator WaitForTransformation()
    {
        yield return new WaitForSeconds(1f);
    }

    private void DemonIsDead()
    {
        KnockBackPlayer();

        if (!slimeManager.stage2Started)
        {
            PrepareStage2();
        }
        else
        {
            ManageDeath();
        }
    }

    private void ManageDeath()
    {
        music.PlayBossMusic(false);
        demonManager.IsFullDead();
        AchievementManager.Instance.UnlockEndGameAchievement();
        StartCoroutine(WaitToStartDialogue());
    }

    private void PrepareStage2()
    {
        StartCoroutine(DeactivateDeadAnimation());
        animator.SetBool(Harmony.AnimatorParameters.Demon_transformation, true);
        StartCoroutine(WaitForTransformation());
        life = MAX_DEMON_LIFE;
    }

    private void KnockBackPlayer()
    {
        PlayerHealth playerHP = playerTransform.gameObject.GetComponent<PlayerHealth>();
        bool left = transform.position.x < playerTransform.position.x;
        playerHP.TakeDamageKnockBack(0f, !left);
    }

    IEnumerator WaitToStartDialogue()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponents<DialogueTrigger>()[1].TriggerDialogue(GameObject.FindGameObjectWithTag(Harmony.Tags.Player));
    }

    private void ManageKnockback(bool left)
    {
        int directionMultiplier = left ? -1 : 1;
        rb.AddForce(new Vector2(directionMultiplier * Force, 2 * Force), ForceMode2D.Impulse);
    }

    public void ActivateHealthBar()
    {
        healthBar.gameObject.SetActive(true);
        healthBar.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = demonManager.BOSS_NAME;
        healthBar.maxValue = MAX_DEMON_LIFE;
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

    internal bool HealthBarIsActive()
    {
        return healthBar.isActiveAndEnabled;
    }
}
